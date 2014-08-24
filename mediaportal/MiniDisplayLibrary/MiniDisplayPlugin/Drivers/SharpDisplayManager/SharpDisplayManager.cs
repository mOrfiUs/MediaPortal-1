using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using MediaPortal.GUI.Library;
using System.Windows.Forms;
using System.ServiceModel;
using System.Runtime.Serialization;
using SharpDisplayInterface;

namespace SharpDisplayInterface
{
    //That contract need to be in the same namespace than the original assembly
    //otherwise our parameter won't make to the server.
    //See: http://stackoverflow.com/questions/14956377/passing-an-object-using-datacontract-in-wcf/25455292#25455292
    [DataContract]
    public class TextField
    {
        public TextField()
        {
            Index = 0;
            Text = "";
            Alignment = ContentAlignment.MiddleLeft;
        }

        public TextField(int aIndex, string aText = "", ContentAlignment aAlignment = ContentAlignment.MiddleLeft)
        {
            Index = aIndex;
            Text = aText;
            Alignment = aAlignment;
        }

        [DataMember]
        public int Index { get; set; }

        [DataMember]
        public string Text { get; set; }

        [DataMember]
        public ContentAlignment Alignment { get; set; }
    }


    [ServiceContract(CallbackContract = typeof(IDisplayServiceCallback),
                        SessionMode = SessionMode.Required)]
    public interface IDisplayService
    {
        /// <summary>
        /// Set the name of this client.
        /// Name is a convenient way to recognize your client.
        /// Naming you client is not mandatory.
        /// In the absence of a name the session ID is often used instead.
        /// </summary>
        /// <param name="aClientName"></param>
        [OperationContract(IsOneWay = true)]
        void SetName(string aClientName);

        /// <summary>
        /// Put the given text in the given field on your display.
        /// Fields are often just lines of text.
        /// </summary>
        /// <param name="aTextFieldIndex"></param>
        [OperationContract(IsOneWay = true)]
        void SetText(TextField aTextField);

        /// <summary>
        /// Allows a client to set multiple text fields at once.
        /// </summary>
        /// <param name="aTexts"></param>
        [OperationContract(IsOneWay = true)]
        void SetTexts(System.Collections.Generic.IList<TextField> aTextFields);

        /// <summary>
        /// Provides the number of clients currently connected
        /// </summary>
        /// <returns></returns>
        [OperationContract()]
        int ClientCount();

    }


    public interface IDisplayServiceCallback
    {
        [OperationContract(IsOneWay = true)]
        void OnConnected();

        /// <summary>
        /// Tell our client to close its connection.
        /// Notably sent when the server is shutting down.
        /// </summary>
        [OperationContract(IsOneWay = true)]
        void OnCloseOrder();
    }
}

//////////////////////////////////////////////////////////////////////////

namespace MediaPortal.ProcessPlugins.MiniDisplayPlugin.Drivers.SharpDisplayManager
{

    /// <summary>
    ///
    /// </summary>
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class Client : DuplexClientBase<IDisplayService>
    {
        public string Name { get; set; }
        public string SessionId { get { return InnerChannel.SessionId; } }

        public Client(InstanceContext callbackInstance)
            : base(callbackInstance, new NetTcpBinding(SecurityMode.None, true), new EndpointAddress("net.tcp://localhost:8001/DisplayService"))
        { }

        public void SetName(string aClientName)
        {
            Name = aClientName;
            Channel.SetName(aClientName);
        }

        public void SetText(TextField aTextField)
        {
            Channel.SetText(aTextField);
        }

        public void SetTexts(System.Collections.Generic.IList<TextField> aTextFields)
        {
            Channel.SetTexts(aTextFields);
        }

        public int ClientCount()
        {
            return Channel.ClientCount();
        }
    }


    public class Callback : IDisplayServiceCallback, IDisposable
    {
        Display iDisplay;

        public Callback(Display aDisplay)
        {
            iDisplay = aDisplay;
        }

        public void OnConnected()
        {
            //Debug.Assert(Thread.CurrentThread.IsThreadPoolThread);
            //Trace.WriteLine("Callback thread = " + Thread.CurrentThread.ManagedThreadId);

            //MessageBox.Show("OnConnected()", "Client");
        }


        public void OnCloseOrder()
        {
            iDisplay.CloseConnection();
            //Debug.Assert(Thread.CurrentThread.IsThreadPoolThread);
            //Trace.WriteLine("Callback thread = " + Thread.CurrentThread.ManagedThreadId);

            //MessageBox.Show("OnServerClosing()", "Client");
        }

        //From IDisposable
        public void Dispose()
        {

        }
    }


    /// <summary>
    /// SoundGraph iMON MiniDisplay implementation.
    /// Provides access to iMON Display API.
    /// </summary>
    public class Display : BaseDisplay
    {
        Client iClient;
        Callback iCallback;
        TextField iTextFieldTop;
        TextField iTextFieldBottom;
        TextField[] iTextFields;

        public Display()
        {
            Initialized = false;

            iTextFieldTop = new TextField(0);
            iTextFieldBottom = new TextField(1);
            iTextFields = new TextField[] { iTextFieldTop , iTextFieldBottom };

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
            if (iClient == null || iClient.State==CommunicationState.Faulted)
            {
                //Attempt to recover
                //LogDebug("SoundGraphDisplay.CheckDisplay(): Trying to recover");
                CleanUp();
                Initialize();
            }

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
            try
            {
                iCallback = new Callback(this);
                InstanceContext instanceContext = new InstanceContext(iCallback);
                iClient = new Client(instanceContext);
                iClient.SetName("MediaPortal");
                Initialized = true;
            }
            catch (System.Exception ex)
            {
                Log.Error(
                "SharpDisplayManager.Display.Initialize(): CAUGHT EXCEPTION {0}\n\n{1}\n\n", ex.Message,
                new object[] { ex.StackTrace });

                //Rollback
                iClient = null;
                iCallback = null;
                Initialized = false;
            }



        }

        //From IDisplay
        public override void CleanUp()
        {
            if (iClient != null)
            {
                try
                {
                    iTextFieldTop.Text="Bye Bye!";
                    iTextFieldBottom.Text="See you next time!";
                    iClient.SetTexts(iTextFields);
                    iClient.Close();
                }
                catch (System.Exception ex)
                {
                    Log.Error(
                    "SharpDisplayManager.Display.CleanUp(): CAUGHT EXCEPTION {0}\n\n{1}\n\n", ex.Message,
                    new object[] { ex.StackTrace });
                }

                iClient = null;
                iCallback = null;
                Initialized = false;
            }
        }

        //From IDisplay
        public override void SetLine(int line, string message, ContentAlignment aAlignment)
        {
            CheckDisplay();

            if (!Initialized)
            {
                return;
            }

            //Pass on that call to our actual display
            //iDisplay.SetLine(line, message);

            //TODO: save it and commit on update            
            //TODO: set a change flag and send stuff to driver on update
            if (line==0 && iTextFieldTop.Text!=message)
            {
                iTextFieldTop.Text = message;
                iTextFieldTop.Alignment = aAlignment;
                iClient.SetText(iTextFieldTop);
            }
            else if (line == 1 && iTextFieldBottom.Text != message)
            {
                iTextFieldBottom.Text = message;
                iTextFieldBottom.Alignment = aAlignment;
                iClient.SetText(iTextFieldBottom);
            }

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

        public void CloseConnection()
        {
            if (iClient != null)
            {
                try
                {
                    iClient.Close();
                }
                catch (System.Exception ex)
                {
                    Log.Error(
                    "SharpDisplayManager.Display.CloseConnection(): CAUGHT EXCEPTION {0}\n\n{1}\n\n", ex.Message,
                    new object[] { ex.StackTrace });
                }

                iClient = null;
                iCallback = null;
                Initialized = false;
            }
        }

    }
}

