using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MTV.Scheduler.App.UI
{
    public partial class Prompt : Form
    {
        public string Val;
        public Prompt()
        {
            InitializeComponent();
            button1.Text = "OK";
        }

        public Prompt(string label, string prefill)
        {
            InitializeComponent();
            Text = label;
            textBox1.Text = prefill;
        }

        public Prompt(string label, string prefill, bool isPassword)
        {
            InitializeComponent();
            Text = label;
            if (isPassword)
                textBox1.PasswordChar = '*';
            textBox1.Text = prefill;

        }

        public override sealed string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Val = textBox1.Text;
            Close();
        }




    }
}
