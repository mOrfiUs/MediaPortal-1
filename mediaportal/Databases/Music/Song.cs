using System;

namespace MediaPortal.Music.Database
{
	/// <summary>
	/// 
	/// </summary>
	public class Song
	{


    string m_strFileName="";
    string m_strTitle="";
    string m_strArtist="";
    string m_strAlbum="";
    string m_strGenre="";
    int m_iTrack=0;
    int m_iDuration=0;
    int m_iYear=0;
    int m_iTimedPlayed=0;
		int m_irating=0;
		int idGenre=-1;
		int idAlbum=-1;
		int idArtist=-1;
		int Id=-1;
		bool favorite=false;

    public Song()
		{
		}
    public Song Clone()
    {
      Song newsong = new Song();
      newsong.Album = Album;
      newsong.Artist = Artist;
      newsong.Duration = Duration;
      newsong.FileName = FileName;
      newsong.Genre = Genre;
      newsong.TimesPlayed = TimesPlayed;
      newsong.Title = Title;
      newsong.Track = Track;
      newsong.Year = Year;
			newsong.Rating=Rating;
			newsong.idGenre=idGenre;
			newsong.idAlbum=idAlbum;
			newsong.idArtist=idArtist;
			newsong.Id=Id;
			newsong.favorite=Favorite;
      return newsong;
    }

    public void Clear() 
		{
			favorite=false;
			idGenre=-1;
			idAlbum=-1;
			idArtist=-1;
			Id=-1;
      m_strFileName="";
      m_strTitle="";
      m_strArtist="";
      m_strAlbum="";
      m_strGenre="";
      m_iTrack=0;
      m_iDuration=0;
      m_iYear=0;
      m_iTimedPlayed=0;
			m_irating=0;
    }

    public string FileName
    {
      get { return m_strFileName;}
      set {m_strFileName=value;}
    }
    public string Title
    {
      get { return m_strTitle;}
      set {m_strTitle=value;}
    }
    public string Artist
    {
      get { return m_strArtist;}
      set {m_strArtist=value;}
    }
    public string Album
    {
      get { return m_strAlbum;}
      set {m_strAlbum=value;}
    }
    public string Genre
    {
      get { return m_strGenre;}
      set {m_strGenre=value;}
    }
    public int Track
    {
      get { return m_iTrack;}
      set {m_iTrack=value;}
    }
    public int Duration
    {
      get { return m_iDuration;}
      set {m_iDuration=value;}
    }
    public int Year
    {
      get { return m_iYear;}
      set {m_iYear=value;}
    }
    public int TimesPlayed
    {
      get { return m_iTimedPlayed;}
      set {m_iTimedPlayed=value;}
		}
		public int Rating
		{
			get { return m_irating;}
			set {m_irating=value;}
		}
		public bool Favorite
		{
			get { return favorite;}
			set {favorite=value;}
		}
		public int albumId
		{
			get { return idAlbum;}
			set {idAlbum=value;}
		}
		public int genreId
		{
			get { return idGenre;}
			set {idGenre=value;}
		}
		public int artistId
		{
			get { return idArtist;}
			set {idArtist=value;}
		}
		public int songId
		{
			get { return Id;}
			set {Id=value;}
		}
  }
	public class SongMap
	{
		public string m_strPath;
		public Song   m_song;
	}
}
