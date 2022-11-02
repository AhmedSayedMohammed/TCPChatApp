using SuperSimpleTcp;
using System.Text;

namespace TCPClient
{
    public partial class ClientForm : Form
    {
        public ClientForm()
        {
            InitializeComponent();
        }
        SimpleTcpClient client;

        private void OnDataRecieved(object? sender, DataReceivedEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                // if the sender is server set the name to server otherwise set it to IP
                string senderIp = e.IpPort;
                if (e.IpPort == txtServer.Text)
                {
                    senderIp = "Server";
                }
                txtInfo.Text += $"{senderIp}: {Encoding.UTF8.GetString(e.Data)} {Environment.NewLine}";
            });
        }

        private void OnDisconnected(object? sender, ConnectionEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                txtInfo.Text += $"{e.IpPort} disconnected.{Environment.NewLine}";
            });
        }

        private void OnConnected(object? sender, ConnectionEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                txtInfo.Text += $"{e.IpPort} connected.{Environment.NewLine}";
            });
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            ConnectToServer();
        }

        private void ConnectToServer()
        {
            client.Connect();
            txtInfo.Text = $"connected.. {Environment.NewLine}";
            btnSend.Enabled = true;
            client.Events.Connected += OnConnected;
            client.Events.Disconnected += OnDisconnected;
            client.Events.DataReceived += OnDataRecieved;
        }

        private void ClientForm_Load(object sender, EventArgs e)
        {
            client = new SimpleTcpClient(txtServer.Text);
            //  btnStart_Click.Enabled = true;
            btnSend.Enabled = false;
            ConnectToServer();
        }

        private void btnSend_Click_1(object sender, EventArgs e)
        {
            if (client.IsConnected)
            {
                if (!string.IsNullOrEmpty(txtMessage.Text))
                {
                    client.Send(txtMessage.Text);
                    txtInfo.Text += $"Client : {txtMessage.Text}{Environment.NewLine}";
                    txtMessage.Text = string.Empty;
                }
                else
                {

                }
            }
        }

    }
}