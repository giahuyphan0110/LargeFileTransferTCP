using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TcpFileTransferGUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {
            
        }
        

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();  //Program exit
        }

        private void FileDrop_Click(object sender, EventArgs e)
        {

        }

        private void FileDrop_Click_1(object sender, EventArgs e)
        {

        }

        //Button process (aka jump to the right form)
        private void button3_Click(object sender, EventArgs e)
        {
            FrmServer frm = new FrmServer();
            frm.Show(); //Button to open FrmServer
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FrmClient frm = new FrmClient();
            frm.Show(); //Button  to open FrmClient
        }

        private void button4_Click(object sender, EventArgs e)
        {
            FrmCredit frm = new FrmCredit();
            frm.Show();
        }
    }
}
