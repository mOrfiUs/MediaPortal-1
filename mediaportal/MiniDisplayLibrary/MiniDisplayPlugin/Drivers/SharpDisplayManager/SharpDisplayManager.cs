using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using MediaPortal.GUI.Library;
using System.Windows.Forms;
using System.ServiceModel;



namespace MediaPortal.ProcessPlugins.MiniDisplayPlugin.Drivers
{

    [ServiceContract]
    public interface IDisplayService
    {
        [OperationContract]
        void SetText(int aLineIndex, string aText);

        [OperationContract]
        void SetTexts(System.Collections.Generic.IList<string> aTexts);
    }
    
    /// <summary>
    /// SoundGraph iMON MiniDisplay implementation.
    /// Provides access to iMON Display API.
    /// </summary>
    public class SharpDisplayManager : BaseDisplay
    {
        ChannelFactory<IDisplayService> iChannelFactory;
        IDisplayService iClient;

        public SharpDisplayManager()
        {
            Initialized = false;
        }


        /// <summary>
        /// Tell whether or not our SoundGraph Display plug-in is initialized
        /// </summary>
        protected bool Initialized { get; set; }

        //From IDisplay
        public override string Description { get { return "Sharp Display Manager"; } }

        //From IDisplay
        //Notably used when testing to put on the screen
        public override string Name
        {
            get
            {
                return "Sharp Display Manager";
            }
        }

        //
        private void CheckDisplay()
        {
            /*
            if (iDisplay == null)
            {
                //Attempt to recover
                //LogDebug("SoundGraphDisplay.CheckDisplay(): Trying to recover");
                CleanUp();
                Initialize();
            }
             */
        }

        //From IDisplay
        public override void Update()
        {
            CheckDisplay();

        }

        //From IDisplay
        public override bool SupportsGraphics { get { return false; } }

        //From IDisplay
        public override bool SupportsText { get { return true; } }


        //
        protected string ClassErrorName { get; set; }

        protected string UnsupportedDeviceErrorMessage { get; set; }


        public override string ErrorMessage
        {
            get
            {
                if (IsDisabled)
                {
                    return "ERROR: SharpDisplayManager";
                }
                return string.Empty;
            }
        }

        //From IDisplay

        public override bool IsDisabled
        {
            get
            {
                //To check if our display is enabled we need to initialize it.
                Initialize();
                bool res = !Initialized;
                CleanUp();
                return res;
            }
        }

        //From IDisplay
        public override void Dispose()
        {
            CleanUp();
        }

        //From IDisplay
        public override void Initialize()
        {
            iChannelFactory = new ChannelFactory<IDisplayService>(
                                    new NetNamedPipeBinding(),
                                    new EndpointAddress(
                                    "net.pipe://localhost/DisplayService"));

            iClient = iChannelFactory.CreateChannel();

            Initialized = true;
        }

        //From IDisplay
        public override void CleanUp()
        {
            iClient = null;
            iChannelFactory = null;
            Initialized = false;
        }

        //From IDisplay
        public override void SetLine(int line, string message)
        {
            CheckDisplay();
            //Pass on that call to our actual display
            //iDisplay.SetLine(line, message);

            //TODO: save it and commit on update
            iClient.SetText(line,message);
        }

        //From IDisplay
        public override void Configure()
        {
            //We need to have an initialized display to be able to configure it
            Initialize();

            CleanUp();
        }

        //From IDisplay
        public override void DrawImage(Bitmap bitmap)
        {
            // Not supported
        }

        //From IDisplay
        public override void SetCustomCharacters(int[][] customCharacters)
        {
            // Not supported
        }

        //From IDisplay
        public override void Setup(string port,
            int lines, int cols, int delay,
            int linesG, int colsG, int timeG,
            bool backLight, int backLightLevel,
            bool contrast, int contrastLevel,
            bool BlankOnExit)
        {
            // iMON VFD/LCD cannot be setup
        }



    }
}

