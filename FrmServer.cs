using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace TcpFileTransferGUI
{
    public partial class FrmServer : Form
    {
        //variable
        int BUFFER_SIZE = 1024;
        Socket sckClient;
        Socket server;
        IPEndPoint ep;
        public FrmServer()
        {
            InitializeComponent();
        }
        void ConnectionProcess(IAsyncResult result)
        {
            sckClient = server.EndAccept(result);
            //Status update

            //update the Connect to ... button into connected for status update
            button2.BeginInvoke(new MethodInvoker(() =>
            {
                button2.Text = "Connected";
            }));

            statusStrip1.Invoke(new UiUpdate(StatusUpdate), new object[] { "Connected" });

            //Starting file transfer
            pathToSave = ("D:\\"); //the current path we are using (D:\)

            //to choose the path to save the file, I disable this function

            if (!new DirectoryInfo(pathToSave + "/").Exists)
            {
                pathToSave = ("C:\\");
            }
            else
            {
                sckClient.BeginReceive(data, 0, 1024, SocketFlags.None, new AsyncCallback(ProcessReceiveData), null);
            }
        }
        //variable
        byte[] data = new byte[1024];
        string pathToSave;
        FileStream fs;
        string fileName = "";
        long dataReceive = 0;
        long n;
        int i = 0;
        void ProcessReceiveData(IAsyncResult result) //Processing received data
        {

            int n_data = sckClient.EndReceive(result);
            if (dataReceive == 0)
            {
                dataReceive = long.Parse(Encoding.ASCII.GetString(data, 0, n_data));
                n = dataReceive / BUFFER_SIZE;
                if (n * BUFFER_SIZE < dataReceive) n += 1;
                sckClient.BeginReceive(data, 0, BUFFER_SIZE, SocketFlags.None, new AsyncCallback(ProcessReceiveData), null);
            }
            else if (fileName == "")
            {
                fileName = Encoding.UTF8.GetString(data, 0, n_data);
                fs = new FileStream(pathToSave + "/" + fileName, FileMode.Create);
                sckClient.BeginReceive(data, 0, BUFFER_SIZE, SocketFlags.None, new AsyncCallback(ProcessReceiveData), null);
                statusStrip1.Invoke(new UiUpdate(StatusUpdate), new object[] { "Receiving file " + fileName + " IP: " + ep.ToString() });
                //Change the status of Connect to ... button into receiving for status update
                button2.BeginInvoke(new MethodInvoker(() =>
                {
                    button2.Text = "Receiving...";
                }));
            }
            else if (i <= n)
            {
                progressBar1.Invoke(new UiUpdate(ProgressUpdate), new object[] { (i + 1) * 100 / n + "" });
                fs.Write(data, 0, n_data);
                i++;
                if (i == n)
                {
                    //Change the status of Connect to ... button into done for status update
                    button2.BeginInvoke(new MethodInvoker(() =>
                    {
                        button2.Text = "Done";
                        FileReceived frm = new FileReceived(); ///show the filereceived form
                        frm.Show();
                    }));
                }
                sckClient.BeginReceive(data, 0, BUFFER_SIZE, SocketFlags.None, new AsyncCallback(ProcessReceiveData), null);

            }
            else
            {


            }
            //Waiting to receive
        }
        delegate void UiUpdate(string s);
        void StatusUpdate(string s) //update the transfer status (currently not use)
        {
            toolStripStatusLabel1.Text = s;
            if (s.Equals("Done"))
            {
               // fs.Close();
               //server.Close();
            }
        }
        void ProgressUpdate(string s) //process the progress percent 
        {
            label_progress.Text = s + "%";
            progressBar1.Value = Convert.ToInt32(s);
        }

        private void textBox1_TextChanged(object sender, EventArgs e) //stored the save path
        {

        }

        private void FileDrop_Click(object sender, EventArgs e)
        {

        }

        public void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e) //open directory finding - for testing purpose (Not in use)
        {
            OpenFileDialog v1 = new OpenFileDialog(); //open window exploer 

        }

        public void button2_Click(object sender, EventArgs e) //process the connect to sender button
        {
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            ep = new IPEndPoint(IPAddress.Any, (int)numericUpDown1.Value);
            //textBox1.Text = ep.ToString();
            server.Bind(ep);
            server.Listen(5);
            //string pathToSend = textBox1.Text;
            string pathToSend = ("D:\\");
            FileInfo file = new FileInfo(pathToSend);
            button2.Text = "Waiting for sender...";
            button2.Enabled = false;
            //toolStripStatusLabel1.Text = "Waiting for sender...";
            server.BeginAccept(new AsyncCallback(ConnectionProcess), null);
        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void FrmServer_FromClosed(object sender, FormClosedEventArgs e) //close socket and fs when form closed
        {
            fs.Close();
            server.Close();
        }
    }
}
