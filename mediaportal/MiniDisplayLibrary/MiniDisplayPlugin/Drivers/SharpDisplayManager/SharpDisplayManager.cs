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
    [ServiceContract(CallbackContract = typeof(IDisplayServiceCallback))]
    public interface IDisplayService
    {
        [OperationContract(IsOneWay = true)]
        void Connect(string aClientName);

        [OperationContract(IsOneWay = true)]
        void SetText(int aLineIndex, string aText);

        [OperationContract(IsOneWay = true)]
        void SetTexts(System.Collections.Generic.IList<string> aTexts);
    }


    public interface IDisplayServiceCallback
    {
        [OperationContract(IsOneWay = true)]
        void OnConnected();

        [OperationContract]
        void OnServerClosing();
    }


    public partial class ClientInput : IDisplayServiceCallback
    {
        public void OnConnected()
        {
            //Debug.Assert(Thread.CurrentThread.IsThreadPoolThread);
            //Trace.WriteLine("Callback thread = " + Thread.CurrentThread.ManagedThreadId);

            //MessageBox.Show("OnConnected()", "Client");
        }


        public void OnServerClosing()
        {
            //Debug.Assert(Thread.CurrentThread.IsThreadPoolThread);
            //Trace.WriteLine("Callback thread = " + Thread.CurrentThread.ManagedThreadId);

            //MessageBox.Show("OnServerClosing()", "Client");
        }
    }



    public partial class ClientOutput : DuplexClientBase<IDisplayService>, IDisplayService
    {
        public ClientOutput(InstanceContext callbackInstance)
            : base(callbackInstance, new NetTcpBinding(), new EndpointAddress("net.tcp://localhost:8001/DisplayService"))
        { }

        public void Connect(string aClientName)
        {
            Channel.Connect(aClientName);
        }

        public void SetText(int aLineIndex, string aText)
        {
            Channel.SetText(aLineIndex, aText);
        }


        public void SetTexts(System.Collections.Generic.IList<string> aTexts)
        {
            Channel.SetTexts(aTexts);
        }


    }

    
    /// <summary>
    /// SoundGraph iMON MiniDisplay implementation.
    /// Provides access to iMON Display API.
    /// </summary>
    public class SharpDisplayManager : BaseDisplay
    {
        ClientOutput iClientOutput;
        ClientInput iClientInput;
        InstanceContext iInstanceContext;

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
            iClientInput = new ClientInput();
            iInstanceContext = new InstanceContext(iClientInput);
            iClientOutput = new ClientOutput(iInstanceContext);

            iClientOutput.Connect("MediaPortal");

            Initialized = true;
        }

        //From IDisplay
        public override void CleanUp()
        {
            iClientOutput = null;
            iInstanceContext = null;
            iClientInput = null;
            Initialized = false;
        }

        //From IDisplay
        public override void SetLine(int line, string message)
        {
            CheckDisplay();
            //Pass on that call to our actual display
            //iDisplay.SetLine(line, message);

            //TODO: save it and commit on update
            iClientOutput.SetText(line, message);
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

