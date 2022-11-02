using SuperSimpleTcp;
using System.Text;
namespace TCPServer
{
    public partial class ServerForm : Form
    {
        public ServerForm()
        {
            InitializeComponent();
        }
        SimpleTcpServer tcpServer;
        private void btnStart_Click(object sender, EventArgs e)
        {
            CreateServer();
        }

        private void CreateServer()
        {
            tcpServer = new SimpleTcpServer(txtServer.Text);
            tcpServer.Start();
            txtInfo.Text = $"Server started.. {Environment.NewLine}";
            btnSend.Enabled = true;
            tcpServer.Events.ClientConnected += OnClientConnected;
            tcpServer.Events.ClientDisconnected += OnClientDisconnected;
            tcpServer.Events.DataReceived += OnDataRecieved;
        }

        private void OnDataRecieved(object? sender, DataReceivedEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                txtInfo.Text += $"{e.IpPort}: {Encoding.UTF8.GetString(e.Data)} {Environment.NewLine}";
            });
        }

        private void OnClientDisconnected(object? sender, ConnectionEventArgs e)
        {
            // you have to call it like this because it can't be 
            //excuted out of its thread
            this.Invoke((MethodInvoker)delegate
            {
                txtInfo.Text += $"{e.IpPort} disconnected.{Environment.NewLine}";
                lstConnections.Items.Remove(e.IpPort);
            });
        }

        private void OnClientConnected(object? sender, ConnectionEventArgs e)
        {
            // you have to call it like this because it can't be 
            //excuted out of its thread
            this.Invoke((MethodInvoker)delegate
            {
                txtInfo.Text += $"{e.IpPort} connected.{Environment.NewLine}";
                int itemIndex = lstConnections.Items.Add(e.IpPort);

                // set the current selected item
                lstConnections.SelectedIndex = itemIndex;
            });
        }

        private void ServerForm_Load(object sender, EventArgs e)
        {
            btnSend.Enabled = false;
            CreateServer();

        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (tcpServer.IsListening)
            {
                //check if message not empty and there are clients in connection list
                if (!string.IsNullOrEmpty(txtMessage.Text) && lstConnections.Items.Count > 0)
                {
                    foreach (var item in lstConnections.Items)
                    {
                        tcpServer.Send(item.ToString(), txtMessage.Text);
                        txtInfo.Text += $"Server : {txtMessage.Text}{Environment.NewLine}";
                        txtMessage.Text = string.Empty;

                    }
                }
            }
        }
    }
}
