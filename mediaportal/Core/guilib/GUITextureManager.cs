#region Copyright (C) 2005-2010 Team MediaPortal

// Copyright (C) 2005-2010 Team MediaPortal
// http://www.team-mediaportal.com
// 
// MediaPortal is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 2 of the License, or
// (at your option) any later version.
// 
// MediaPortal is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with MediaPortal. If not, see <http://www.gnu.org/licenses/>.

#endregion

//#define DO_RESAMPLE
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using MediaPortal.Configuration;
using MediaPortal.Util;
using Microsoft.DirectX.Direct3D;
using MediaPortal.ExtensionMethods;


namespace MediaPortal.GUI.Library
{
  public class GUITextureManager
  {
    private const int MAX_THUMB_WIDTH = 512;
    private const int MAX_THUMB_HEIGHT = 512;

    private static CachedTextureCollection _cachedTextures = new CachedTextureCollection();
    private static Dictionary<string, DownloadedImage> _cachedDownloads = new Dictionary<string, DownloadedImage>();
    private static HashSet<CachedTexture> _cleanupQueue = new HashSet<CachedTexture>();

    private static TexturePacker _packer = new TexturePacker();
    private static readonly object _syncRoot = new object();

    // singleton. Dont allow any instance of this class
    private GUITextureManager() { }

    static GUITextureManager() { }

    ~GUITextureManager()
    {
      dispose(false);
    }

    public static void Dispose()
    {
      dispose(true);
    }

    private static void dispose(bool disposing)
    {
      lock (GUIGraphicsContext.RenderLock)
      {
        Log.Debug("TextureManager: Dispose()");
        _packer.SafeDispose();

        _cleanupQueue.DisposeAndClearCollection();
        _cachedTextures.DisposeAndClear();
        _cachedDownloads.DisposeAndClear();

        string[] files = null;

        try
        {
          files = Directory.GetFiles(Config.GetFolder(Config.Dir.Thumbs), "MPTemp*.*");
        }
        catch { }

        if (files != null)
        {
          foreach (string file in files)
          {
            try
            {
              File.Delete(file);
            }
            catch (Exception) { }
          }
        }
      }
    }

    public static CachedTexture GetCachedTexture(string filename)
    {
      CachedTexture cachedTexture;
      filename = GetFileName(filename);
      lock (_syncRoot)
      {
        if (_cachedTextures.TryGetValue(filename, out cachedTexture))
        {

          // if present in the the cleanup queue remove it cause it is still being used
          _cleanupQueue.Remove(cachedTexture);

          return cachedTexture;
        }
      }

      return null;
    }

    public static Image Resample(Image imgSrc, int iMaxWidth, int iMaxHeight)
    {
      int width = imgSrc.Width;
      int height = imgSrc.Height;
      while (width < iMaxWidth || height < iMaxHeight)
      {
        width *= 2;
        height *= 2;
      }
      float fAspect = ((float)width) / ((float)height);

      if (width > iMaxWidth)
      {
        width = iMaxWidth;
        height = (int)Math.Round(((float)width) / fAspect);
      }

      if (height > (int)iMaxHeight)
      {
        height = iMaxHeight;
        width = (int)Math.Round(fAspect * ((float)height));
      }

      Bitmap result = new Bitmap(width, height);
      using (Graphics g = Graphics.FromImage(result))
      {
        g.CompositingQuality = Thumbs.Compositing;
        g.InterpolationMode = Thumbs.Interpolation;
        g.SmoothingMode = Thumbs.Smoothing;
        g.DrawImage(imgSrc, new Rectangle(0, 0, width, height));
      }
      return result;
    }

    private static string GetFileName(string fileName)
    {
      string path = GetFullPath(fileName);

      string fixedPath = fileName.ToLower().Trim();
      if (fixedPath.IndexOf(@"http:") >= 0)
      {
        lock (_syncRoot)
        {
          DownloadedImage image;
          if (_cachedDownloads.TryGetValue(path, out image))
          {
            if (image.ShouldDownLoad)
            {
              image.Download();
            }
            return image.FileName;
          }
          else
          {
            image = new DownloadedImage(fileName);
            image.Download();
            _cachedDownloads.Add(fileName, image);
          }

          return image.FileName;
        }
      }

      return path;
    }

    private static string GetFullPath(string path)
    {

      if (path.Length == 0 || path == "-")
      {
        return string.Empty;
      }

      string fixedPath = path.ToLower().Trim();

      if (fixedPath.IndexOf(@"http:") >= 0)
      {
        return path;
      }

      if (!Path.IsPathRooted(fixedPath))
      {
        path = GUIGraphicsContext.Skin + @"\media\" + path;
      }

      return path;
    }

    public static int Load(string fileNameOrg, long lColorKey, int iMaxWidth, int iMaxHeight)
    {
      return Load(fileNameOrg, lColorKey, iMaxWidth, iMaxHeight, false);
    }

    public static int Load(string fileNameOrg, long lColorKey, int iMaxWidth, int iMaxHeight, bool persistent)
    {
      string fileName = GetFileName(fileNameOrg);
      if (String.IsNullOrEmpty(fileName))
      {
        return 0;
      }

      CachedTexture cachedTexture;
      lock (_syncRoot)
      {
        if (_cachedTextures.TryGetValue(fileName, out cachedTexture))
        {

          // if present in the the cleanup queue remove it cause it is still being used
          _cleanupQueue.Remove(cachedTexture);

          return cachedTexture.Frames;
        }

        string extension = Path.GetExtension(fileName).ToLower();
        if (extension == ".gif")
        {
          if (!File.Exists(fileName))
          {
            Log.Warn("TextureManager: texture: {0} does not exist", fileName);
            return 0;
          }

          Image theImage = null;
          try
          {
            try
            {
              theImage = ImageFast.FromFile(fileName);
            }
            catch (ArgumentException)
            {
              Log.Warn("TextureManager: Fast loading texture {0} failed using safer fallback", fileName);
              theImage = Image.FromFile(fileName);
            }
            if (theImage != null)
            {
              cachedTexture = new CachedTexture();

              cachedTexture.Name = fileName;
              FrameDimension oDimension = new FrameDimension(theImage.FrameDimensionsList[0]);
              cachedTexture.Frames = theImage.GetFrameCount(oDimension);
              int[] frameDelay = new int[cachedTexture.Frames];
              for (int num2 = 0; (num2 < cachedTexture.Frames); ++num2)
              {
                frameDelay[num2] = 0;
              }

              // Getting Frame duration of an animated Gif image            
              try
              {
                int num1 = 20736;
                PropertyItem item1 = theImage.GetPropertyItem(num1);
                if (item1 != null)
                {
                  byte[] buffer1 = item1.Value;
                  for (int num2 = 0; (num2 < cachedTexture.Frames); ++num2)
                  {
                    frameDelay[num2] = (((buffer1[(num2 * 4)] + (256 * buffer1[((num2 * 4) + 1)])) +
                                         (65536 * buffer1[((num2 * 4) + 2)])) + (16777216 * buffer1[((num2 * 4) + 3)]));
                  }
                }
              }
              catch (Exception) { }

              for (int i = 0; i < cachedTexture.Frames; ++i)
              {
                theImage.SelectActiveFrame(oDimension, i);

                //load gif into texture
                using (MemoryStream stream = new MemoryStream())
                {
                  theImage.Save(stream, ImageFormat.Png);
                  ImageInformation info2 = new ImageInformation();
                  stream.Flush();
                  stream.Seek(0, SeekOrigin.Begin);
                  Texture texture = TextureLoader.FromStream(
                    GUIGraphicsContext.DX9Device,
                    stream,
                    0, 0, //width/height
                    1, //mipslevels
                    0, //Usage.Dynamic,
                    Format.A8R8G8B8,
                    GUIGraphicsContext.GetTexturePoolType(),
                    Filter.None,
                    Filter.None,
                    (int)lColorKey,
                    ref info2);
                  cachedTexture.Width = info2.Width;
                  cachedTexture.Height = info2.Height;
                  cachedTexture[i] = new CachedTexture.Frame(fileName, texture, (frameDelay[i] / 5) * 50);
                }
              }

              theImage.SafeDispose();
              theImage = null;

              cachedTexture.Disposed += new EventHandler(cachedTexture_Disposed);

              if (persistent)
              {
                cachedTexture.Persistent = persistent;
              }

              _cachedTextures.Add(cachedTexture);

              //Log.Info("  TextureManager:added:" + fileName + " total:" + _cache.Count + " mem left:" + GUIGraphicsContext.DX9Device.AvailableTextureMemory.ToString());
              return cachedTexture.Frames;
            }
          }
          catch (Exception ex)
          {
            Log.Error("TextureManager: exception loading texture {0} - {1}", fileName, ex.Message);
          }
          return 0;
        }

        if (File.Exists(fileName))
        {
          int width, height;
          Texture dxtexture = LoadGraphic(fileName, lColorKey, iMaxWidth, iMaxHeight, out width, out height);
          if (dxtexture != null)
          {
            cachedTexture = new CachedTexture();
            cachedTexture.Name = fileName;
            cachedTexture.Frames = 1;
            cachedTexture.Width = width;
            cachedTexture.Height = height;
            cachedTexture.texture = new CachedTexture.Frame(fileName, dxtexture, 0);
            //Log.Info("  texturemanager:added:" + fileName + " total:" + _cache.Count + " mem left:" + GUIGraphicsContext.DX9Device.AvailableTextureMemory.ToString());
            cachedTexture.Disposed += new EventHandler(cachedTexture_Disposed);
            if (persistent)
            {
              cachedTexture.Persistent = persistent;
            }
            _cachedTextures.Add(cachedTexture);
            return 1;
          }
        }
      }
      return 0;
    }

    /// <summary>
    /// Load an image object with the specified name from cache or adds it
    /// </summary>
    /// <param name="memoryImage">the image object to load</param>
    /// <param name="name">name of the imag</param>
    /// <param name="lColorKey"></param>
    /// <param name="iMaxWidth"></param>
    /// <param name="iMaxHeight"></param>
    /// <returns>number of frames contained within the image</returns>
    public static int LoadFromMemory(Image memoryImage, string name, long lColorKey, int iMaxWidth, int iMaxHeight)
    {
      Log.Debug("TextureManager: load from memory: {0}", name);
      string textureName = name.ToLower();

      CachedTexture cachedTexture;
      lock (_syncRoot)
      {

        if (_cachedTextures.TryGetValue(textureName, out cachedTexture))
        {

          // if present in the the cleanup queue remove it cause it is still being used
          _cleanupQueue.Remove(cachedTexture);

          return cachedTexture.Frames;
        }

        if (memoryImage == null)
        {
          return 0;
        }
        if (memoryImage.FrameDimensionsList == null)
        {
          return 0;
        }
        if (memoryImage.FrameDimensionsList.Length == 0)
        {
          return 0;
        }

        try
        {
          cachedTexture = new CachedTexture();
          cachedTexture.Name = textureName;
          FrameDimension oDimension = new FrameDimension(memoryImage.FrameDimensionsList[0]);
          cachedTexture.Frames = memoryImage.GetFrameCount(oDimension);
          if (cachedTexture.Frames != 1)
          {
            return 0;
          }
          //load gif into texture
          using (MemoryStream stream = new MemoryStream())
          {
            memoryImage.Save(stream, ImageFormat.Png);
            ImageInformation info2 = new ImageInformation();
            stream.Flush();
            stream.Seek(0, SeekOrigin.Begin);
            Texture texture = TextureLoader.FromStream(
              GUIGraphicsContext.DX9Device,
              stream,
              0, 0, //width/height
              1, //mipslevels
              0, //Usage.Dynamic,
              Format.A8R8G8B8,
              GUIGraphicsContext.GetTexturePoolType(),
              Filter.None,
              Filter.None,
              (int)lColorKey,
              ref info2);
            cachedTexture.Width = info2.Width;
            cachedTexture.Height = info2.Height;
            cachedTexture.texture = new CachedTexture.Frame(textureName, texture, 0);
          }
          memoryImage.SafeDispose();
          memoryImage = null;
          cachedTexture.Disposed += new EventHandler(cachedTexture_Disposed);
          _cachedTextures.Add(cachedTexture);

          Log.Debug("TextureManager: added: memoryImage  " + " total count: " + _cachedTextures.Count + ", mem left (MB): " +
              ((uint)GUIGraphicsContext.DX9Device.AvailableTextureMemory / 1048576));
          return cachedTexture.Frames;
        }
        catch (Exception ex)
        {
          Log.Error("TextureManager: exception loading texture memoryImage");
          Log.Error(ex);
        }
      }

      return 0;
    }

    /// <summary>
    /// Load an image object with the specified name from cache or adds it
    /// </summary>
    /// <param name="memoryImage">the image to load</param>
    /// <param name="name">name of the image</param>
    /// <param name="lColorKey"></param>
    /// <param name="texture"></param>
    /// <returns>number of frames contained within the image, and outputs resulting texture</returns>
    public static int LoadFromMemoryEx(Image memoryImage, string name, long lColorKey, out Texture texture)
    {
      Log.Debug("TextureManagerEx: load from memory: {0}", name);

      texture = null;
      string cacheName = name.ToLower();

      CachedTexture cachedTexture;
      lock (_syncRoot)
      {

        if (_cachedTextures.TryGetValue(cacheName, out cachedTexture))
        {
          _cleanupQueue.Remove(cachedTexture);
          texture = cachedTexture.texture.Image;

          return cachedTexture.Frames;
        }

        if (memoryImage == null)
        {
          return 0;
        }
        try
        {
          cachedTexture = new CachedTexture();
          cachedTexture.Name = cacheName;
          cachedTexture.Frames = 1;

          //load gif into texture
          using (MemoryStream stream = new MemoryStream())
          {
            memoryImage.Save(stream, ImageFormat.Png);
            ImageInformation info2 = new ImageInformation();
            stream.Flush();
            stream.Seek(0, SeekOrigin.Begin);
            texture = TextureLoader.FromStream(
              GUIGraphicsContext.DX9Device,
              stream,
              0, 0, //width/height
              1, //mipslevels
              Usage.Dynamic, //Usage.Dynamic,
              Format.A8R8G8B8,
              Pool.Default,
              Filter.None,
              Filter.None,
              (int)lColorKey,
              ref info2);
            cachedTexture.Width = info2.Width;
            cachedTexture.Height = info2.Height;
            cachedTexture.texture = new CachedTexture.Frame(cacheName, texture, 0);
          }
          //memoryImage.SafeDispose();
          //memoryImage = null;
          cachedTexture.Disposed += new EventHandler(cachedTexture_Disposed);

          _cachedTextures.Add(cachedTexture);

          Log.Debug("TextureManager: added: memoryImage  " + " total count: " + _cachedTextures.Count + ", mem left (MB): " +
                    ((uint)GUIGraphicsContext.DX9Device.AvailableTextureMemory / 1048576));
          return cachedTexture.Frames;
        }
        catch (Exception ex)
        {
          Log.Error("TextureManager: exception loading texture memoryImage");
          Log.Error(ex);
        }
      }

      return 0;
    }

    private static void cachedTexture_Disposed(object sender, EventArgs e)
    {
      CachedTexture texture = (CachedTexture)sender;
      lock (_syncRoot)
      {
        texture.Disposed -= new EventHandler(cachedTexture_Disposed);
        _cleanupQueue.Add(texture);
      }
    }

    private static Texture LoadGraphic(string fileName, long lColorKey, int iMaxWidth, int iMaxHeight, out int width, out int height)
    {
      width = 0;
      height = 0;
      Image imgSrc = null;
      Texture texture = null;
      try
      {
#if DO_RESAMPLE
        imgSrc=Image.FromFile(fileName);   
        if (imgSrc==null) return null;
				//Direct3D prefers textures which height/width are a power of 2
				//doing this will increases performance
				//So the following core resamples all textures to
				//make sure all are 2x2, 4x4, 8x8, 16x16, 32x32, 64x64, 128x128, 256x256, 512x512
				int w=-1,h=-1;
				if (imgSrc.Width >2   && imgSrc.Width < 4)  w=2;
				if (imgSrc.Width >4   && imgSrc.Width < 8)  w=4;
				if (imgSrc.Width >8   && imgSrc.Width < 16) w=8;
				if (imgSrc.Width >16  && imgSrc.Width < 32) w=16;
				if (imgSrc.Width >32  && imgSrc.Width < 64) w=32;
				if (imgSrc.Width >64  && imgSrc.Width <128) w=64;
				if (imgSrc.Width >128 && imgSrc.Width <256) w=128;
				if (imgSrc.Width >256 && imgSrc.Width <512) w=256;
				if (imgSrc.Width >512 && imgSrc.Width <1024) w=512;


				if (imgSrc.Height >2   && imgSrc.Height < 4)  h=2;
				if (imgSrc.Height >4   && imgSrc.Height < 8)  h=4;
				if (imgSrc.Height >8   && imgSrc.Height < 16) h=8;				
				if (imgSrc.Height >16  && imgSrc.Height < 32) h=16;
				if (imgSrc.Height >32  && imgSrc.Height < 64) h=32;
				if (imgSrc.Height >64  && imgSrc.Height <128) h=64;
				if (imgSrc.Height >128 && imgSrc.Height <256) h=128;
				if (imgSrc.Height >256 && imgSrc.Height <512) h=256;
				if (imgSrc.Height >512 && imgSrc.Height <1024) h=512;
				if (w>0 || h>0)
				{
					if (h > w) w=h;
					Log.Info("TextureManager: resample {0}x{1} -> {2}x{3} {4}",
												imgSrc.Width,imgSrc.Height, w,w,fileName);

					Image imgResampled=Resample(imgSrc,w, h);
					imgSrc.SafeDispose();
					imgSrc=imgResampled;
					imgResampled=null;
				}
#endif

        Format fmt = Format.A8R8G8B8;

        ImageInformation info2 = new ImageInformation();
        texture = TextureLoader.FromFile(GUIGraphicsContext.DX9Device,
                                         fileName,
                                         0, 0, //width/height
                                         1, //mipslevels
                                         0, //Usage.Dynamic,
                                         fmt,
                                         GUIGraphicsContext.GetTexturePoolType(),
                                         Filter.None,
                                         Filter.None,
                                         (int)lColorKey,
                                         ref info2);
        width = info2.Width;
        height = info2.Height;
      }
      catch (Exception)
      {
        Log.Error("TextureManager: LoadGraphic - invalid thumb({0})", fileName);
        Format fmt = Format.A8R8G8B8;
        string fallback = GUIGraphicsContext.Skin + @"\media\" + "black.png";

        ImageInformation info2 = new ImageInformation();
        texture = TextureLoader.FromFile(GUIGraphicsContext.DX9Device,
                                         fallback,
                                         0, 0, //width/height
                                         1, //mipslevels
                                         0, //Usage.Dynamic,
                                         fmt,
                                         GUIGraphicsContext.GetTexturePoolType(),
                                         Filter.None,
                                         Filter.None,
                                         (int)lColorKey,
                                         ref info2);
        width = info2.Width;
        height = info2.Height;
      }
      finally
      {
        if (imgSrc != null)
        {
          imgSrc.SafeDispose();
        }
      }
      return texture;
    }

    public static CachedTexture.Frame GetTexture(string fileNameOrg, int iImage, out int iTextureWidth, out int iTextureHeight)
    {
      iTextureWidth = 0;
      iTextureHeight = 0;
      string fileName = "";
      if (!fileNameOrg.StartsWith("["))
      {
        fileName = GetFileName(fileNameOrg);
        if (fileName == "")
        {
          return null;
        }
      }
      else
      {
        fileName = fileNameOrg;
      }

      CachedTexture cachedTexture;
      lock (_syncRoot)
      {
        if (!_cachedTextures.TryGetValue(fileName, out cachedTexture))
        {
          return null;
        }

        // if present in the the cleanup queue remove it cause it is still being used
        _cleanupQueue.Remove(cachedTexture);

        iTextureWidth = cachedTexture.Width;
        iTextureHeight = cachedTexture.Height;
        return (CachedTexture.Frame)cachedTexture[iImage];
      }
    }

    public static void ReleaseTexture(string fileName)
    {
      if (string.IsNullOrEmpty(fileName))
      {
        return;
      }

      //dont dispose radio/tv logo's since they are used by the overlay windows
      if (fileName.ToLower().IndexOf(Config.GetSubFolder(Config.Dir.Thumbs, @"tv\logos")) >= 0)
      {
        return;
      }
      if (fileName.ToLower().IndexOf(Config.GetSubFolder(Config.Dir.Thumbs, "radio")) >= 0)
      {
        return;
      }

      CachedTexture texture;
      lock (_syncRoot)
      {
        if (_cachedTextures.TryGetValue(fileName, out texture))
        {
          _cleanupQueue.Add(texture);
        }
      }
    }

    /// <summary>
    /// Call this method from the main thread to release all textures that are pending to be released
    /// </summary>
    public static void ReleaseTextures()
    {
      lock (_syncRoot)
      {
        if (_cleanupQueue.Count == 0)
        {
          return;
        }

        foreach (CachedTexture texture in _cleanupQueue)
        {

          if (_cachedTextures.Contains(texture))
          {
            _cachedTextures.Remove(texture);
          }

          try
          {
            texture.SafeDispose();
          }
          catch (Exception ex)
          {
            Log.Error("TextureManager: Error in ReleaseTexture({0}) - {1}", texture.Name, ex.Message);
          }
        }

        _cleanupQueue.Clear();
      }
    }

    public static void CleanupThumbs()
    {
      Log.Debug("TextureManager: CleanupThumbs()");
      try
      {
        lock (_syncRoot)
        {
          List<CachedTexture> cleanup = _cachedTextures.Where(t => !t.Persistent).ToList();
          _cleanupQueue.UnionWith(cleanup);
        }
      }
      catch (Exception ex)
      {
        Log.Error("TextureManage: Error cleaning up thumbs - {0}", ex.Message);
      }
    }

    public static void Init()
    {
      _packer.PackSkinGraphics(GUIGraphicsContext.Skin);
    }

    public static bool GetPackedTexture(string fileName, out float uoff, out float voff, out float umax, out float vmax,
                                        out int textureWidth, out int textureHeight, out Texture tex,
                                        out int _packedTextureNo)
    {
      return _packer.Get(fileName, out uoff, out voff, out umax, out vmax, out textureWidth, out textureHeight, out tex,
                         out _packedTextureNo);
    }

    public static void Clear()
    {
      _packer.SafeDispose();
      _packer = new TexturePacker();
      _packer.PackSkinGraphics(GUIGraphicsContext.Skin);

      _cachedTextures.DisposeAndClear();
      _cachedDownloads.DisposeAndClear();
    }
  }



}