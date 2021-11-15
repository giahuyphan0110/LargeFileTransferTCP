using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TcpFileTransferGUI
{
    public partial class FileReceived : Form
    {
        public FileReceived()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close(); // close the current form 
            FrmServer obj = (FrmServer)Application.OpenForms["FrmServer"]; //find the FrmServer to close it
            obj.Close(); //close the frmserver
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void FileReceived_Load(object sender, EventArgs e)
        {

        }
    }
}
