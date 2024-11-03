using CFChat.Controls;
using CFChat.Models;
using CFConnectionMessaging.Models;
using System.Net;

namespace CFChat
{
    /// <summary>
    /// Main form for chat.
    /// 
    /// Currently we only handle a single conversation but it would be easy to extend this by adding a container
    /// that we can select the relevant conversation (IConversation) from ChatConnection.Conversations
    /// </summary>
    public partial class MainForm : Form
    {
        private ChatConnection _chatConnection = new ChatConnection();
        public MainForm()
        {
            InitializeComponent();

            // Get our IP
            var hostName = Dns.GetHostName();

            //var result = Dns.GetHostEntry(hostName);
            string myIP = Dns.GetHostEntry(hostName).AddressList.FirstOrDefault(a => a.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).ToString();

            // Set default remote settings. Assume other Chat client is running locally
            //txtRemoteIP.Text = myIP;
            //txtRemotePort.Text = "11000";

            tstxtLocalPort.Text = "11000";      // Default            
            tstxtUserName.Text = Environment.UserName;

            // Add single conversation control (IConversation) to list.            
            var conversationControl = new ConversationControl() { ChatConnection = _chatConnection };
            _chatConnection.Conversations.Add(conversationControl);
            
            // Display conversation list (Only 1 item)
            DisplayConversationList(0);

            this.Text = $"Chat - [Local IP: {myIP}]";
        }

        //private ConversationControl CreateConversationControl(string conversationId, EndpointInfo remoteEndpointInfo)
        //{

        //    var conversationControl = new ConversationControl()
        //    {
        //        ConversationId = conversationId,
        //        RemoteEndpointInfo = new EndpointInfo()
        //        {
        //            Ip = remoteEndpointInfo.Ip,
        //            Port = remoteEndpointInfo.Port,
        //        }
        //    };
        //    return conversationControl;
        //}

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void DisplayConversationList(int selectedIndex)
        {
            tscbConversation.Items.Clear();
            foreach (var conversation in _chatConnection.Conversations)
            {
                if (String.IsNullOrEmpty(conversation.RemoteEndpointInfo.Ip))
                {
                    tscbConversation.Items.Add("[New]");
                }
                else
                {
                    tscbConversation.Items.Add($"{conversation.RemoteEndpointInfo.Ip}:{conversation.RemoteEndpointInfo.Port}");
                }
            }

            tscbConversation.SelectedIndex = selectedIndex;
        }

        private void startListeningToolStripMenuItem_Click(object sender, EventArgs e)
        {
            switch (startListeningToolStripMenuItem.Text)
            {
                case "Start listening":
                    tstxtLocalPort.Enabled = false;   // Prevent user changing local port

                    _chatConnection.StartListening(Convert.ToInt32(tstxtLocalPort.Text));

                    startListeningToolStripMenuItem.Text = "Stop listening";
                    break;
                case "Stop listening":
                    _chatConnection.StopListening();

                    startListeningToolStripMenuItem.Text = "Start listening";
                    break;
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_chatConnection != null)
            {
                _chatConnection.StopListening();
            }
        }

        private void tstxtUserName_TextChanged(object sender, EventArgs e)
        {
            if (_chatConnection != null)
            {
                _chatConnection.LocalUserName = tstxtUserName.Text;
            }
        }

        private void tscbConversation_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Display conversation user control
            pnlConversation.Controls.Clear();

            var conversation = _chatConnection.Conversations[tscbConversation.SelectedIndex];
            var userControl = (UserControl)conversation;
            userControl.Dock = DockStyle.Fill;
            pnlConversation.Controls.Add(userControl);
        }
    }
}
