﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TableFootball
{
    public partial class Cntrls : Form
    {
        public Cntrls()
        {
            InitializeComponent();
        }

        private void Cntrls_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            var form = new StartMenu();
            form.Show();
            Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {

        }
    }
}
