﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace 调用类
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            UpdateClass.UpdateFrom("KTE");
            MessageBox.Show("11111111");
        }

        private void button2_Click(object sender, EventArgs e)
        {

            label1.Text= Process.GetCurrentProcess().ProcessName;
            foreach (System.Diagnostics.Process p in System.Diagnostics.Process.GetProcesses())
            {
                if (p.ProcessName ==label1.Text)
                {
                    MessageBox.Show("dfjasdklasdfklklasdfjklsdjklasdfk");
                 }
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
