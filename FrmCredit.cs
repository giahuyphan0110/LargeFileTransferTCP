using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace TcpFileTransferGUI
{
    public partial class FrmCredit : Form
    {
        public FrmCredit()
        {
            InitializeComponent();
        }


        private void label6_Click(object sender, EventArgs e)
        {

        }

        //Jump to the url of of the member in this project

        private void button1_Click(object sender, EventArgs e)
        {
            var uri = "https://www.facebook.com/ghuy.phan/";
            var psi = new System.Diagnostics.ProcessStartInfo();
            psi.UseShellExecute = true;
            psi.FileName = uri;
            System.Diagnostics.Process.Start(psi);
            //Process.Start("https://www.facebook.com/ghuy.phan/");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var uri = "https://www.facebook.com/itngochiep";
            var psi = new System.Diagnostics.ProcessStartInfo();
            psi.UseShellExecute = true;
            psi.FileName = uri;
            System.Diagnostics.Process.Start(psi);
            //Process.Start("https://www.facebook.com/itngochiep");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var uri = "https://www.facebook.com/profile.php?id=100005303295077";
            var psi = new System.Diagnostics.ProcessStartInfo();
            psi.UseShellExecute = true;
            psi.FileName = uri;
            System.Diagnostics.Process.Start(psi);
            //Process.Start("https://www.facebook.com/profile.php?id=100005303295077");
        }
    }
}
