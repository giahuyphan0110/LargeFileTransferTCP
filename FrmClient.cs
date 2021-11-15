using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;


namespace TcpFileTransferGUI
{
    public partial class FrmClient : Form
    {
        //variable
        Socket sckClient;
        IPEndPoint ep;
        int BUFFER_SIZE = 1024;
        int tick;
        public FrmClient()
        {
            InitializeComponent();
            button3.Enabled = false;
            timer1.Start();
        }

        void ConectionProcess(IAsyncResult result)
        {
            //update the Connect to ... button into connected for status update
            button2.BeginInvoke(new MethodInvoker(() =>
            {
                button2.Text = "Connected";
                button2.Enabled = false;
            }));
            sckClient.EndConnect(result);
            //Update the status and then send file
            //Status update
            statusStrip1.Invoke(new UiUpdate(StatusUpdate), new object[] { "Connected" });
        }
        //variable
        byte[] data = new byte[1024];
        string pathToSend;
        string fullPath;
        FileInfo file;

        //File send
        public void FileSend()
        {
            pathToSend = textBox3.Text;
            fullPath = pathToSend.Replace("\\", "/"); //convert backslah into forwardslash for socket file transfer
            file = new FileInfo(pathToSend);
            if (!file.Exists) //check if file exist or not
            {
                //MessageBox.Show("FILE DOESN'T EXIST");
                FileNotFound frm = new FileNotFound();
                frm.Show();
            }
            else
            {
                string fileName = fullPath.Substring(fullPath.LastIndexOf("/") + 1);
                statusStrip1.Invoke(new UiUpdate(StatusUpdate), new object[] { "Sending file " + fullPath + " IP: " + ep.ToString() });
                sckClient.Send(Encoding.ASCII.GetBytes(file.Length.ToString()));
                sckClient.Send(Encoding.ASCII.GetBytes(fileName));
                Thread a = new Thread(FileSendProcess);
                // Set cursor as hourglass
                Application.UseWaitCursor = true;
                //Change the status of Connect to ... button into sending for status update
                button2.BeginInvoke(new MethodInvoker(() =>
                {
                    button2.Text = "Sending...";
                    SucessfullySent frm = new SucessfullySent();
                    frm.Show();
                }));
                a.Start();
            }

        }

        //process file 
        public void FileSendProcess()
        {
            byte[] dataFile = new byte[file.Length];
            long n = dataFile.Length / BUFFER_SIZE;
            if (n * BUFFER_SIZE < dataFile.Length) n += 1;
            FileStream fs = new FileStream(fullPath, FileMode.Open); // process the input file
            for (long i = 0; i < n; i++)
            {
                byte[] dataSend = new byte[BUFFER_SIZE];
                int size = fs.Read(dataSend, 0, BUFFER_SIZE);
                progressBar1.Invoke(new UiUpdate(ProgressUpdate), new object[] { (i + 1) * 100 / n + "" });
                sckClient.Send(dataSend, 0, size, SocketFlags.None);
            }
            //fs.Close();
            // Set cursor as default arrow
            Application.UseWaitCursor = false;
            //sckClient.Close();
            //Change the status of Connect to ... button into sending for status update
            
            button2.BeginInvoke(new MethodInvoker(() =>
            {
                button2.Text = "Done";
                button2.Enabled = true;
                button2.Text = "Connect to receiver";

            }));
            statusStrip1.Invoke(new UiUpdate(StatusUpdate), new object[] { "Done" });
        }
        delegate void UiUpdate(string s);
        void StatusUpdate(string s)
        {
            toolStripStatusLabel1.Text = s;
            toolStripStatusLabel1.Text = s;
            if (s == "Done")
            {
                button3.Enabled = false;
                //sckClient.Close();
            }
            else
                button3.Enabled = true;
        }

        //update the progressbar %
        void ProgressUpdate(string s)
        {
            label_progress.Text = s + "%";
            progressBar1.Value = Convert.ToInt32(s);
        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {

        }

        //Connect to Receiver butotn - process the connection to the receiver



        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        //open directory finding, taken form FrmServer
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog v1 = new OpenFileDialog();

            if (v1.ShowDialog() == DialogResult.OK)
            {
                textBox3.Text = v1.FileName;
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            sckClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            ep = new IPEndPoint(IPAddress.Parse(textBox1.Text), (int)numericUpDown1.Value);
            sckClient.BeginConnect(ep, new AsyncCallback(ConectionProcess), null);

        }

            //Send button
            private void button3_Click(object sender, EventArgs e)
        {
            FileSend();
            button3.Enabled = false;
        }

        //Allow drag and drop to get file path
        private void textBox3_DragDrop(object sender, DragEventArgs e) 
        {
            string[] FileList = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            textBox3.Text = FileList[0];
        }

        //Drag and drop effect
        private void textBox3_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            tick++;
        }

        private void FrmClient_Load(object sender, EventArgs e)
        {

        }
    }

}
