using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ServiceProvider
{
    public partial class HotelControl : UserControl
    {
        public HotelControl()
        {
            InitializeComponent();
        }

        public string name
        {
            get { return textBox1.Text; }
            set { textBox1.Text = value; }
        }

        public string stars
        {
            get { return textBox2.Text; }
            set { textBox2.Text = value; }
        }

        public string halfBoard
        {
            get { return textBox3.Text; }
            set { textBox3.Text = value; }
        }

        public string fullBoard
        {
            get { return textBox4.Text; }
            set { textBox4.Text = value; }
        }

        public string allInclusive
        {
            get { return textBox5.Text; }
            set { textBox5.Text = value; }
        }

        public event EventHandler<string> pohranaKlik;

        private void button1_Click(object sender, EventArgs e)
        {
            if (pohranaKlik != null)
            {
                pohranaKlik(this, name);
            }
        }
    }
}
