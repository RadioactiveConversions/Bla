using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Metro2033Client
{
    public partial class Form1 : Form
    {

        public string ChatHistory
        {
            get;
            set;
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                textBox2.AppendText(textBox1.Text + System.Environment.NewLine);
                this.ChatHistory = textBox2.Text;
                textBox1.Text = "";
                textBox1.Focus();
            }
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            AboutBox1 info = new AboutBox1();

            info.ShowDialog();
        }
    }
}
