using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using Neo4j.Driver;

namespace ServiceProvider
{
    public partial class LoginForm : Form
    {
        private DatabaseService databaseService;

        public LoginForm()
        {
            InitializeComponent();
            textBox2.UseSystemPasswordChar = true;
            databaseService = new DatabaseService();

            var request = WebRequest.Create("https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRlyou9zwZUVNkPxk4TSR6uEWHBT6-KBfGsHMUcZqZprM3Lj7IbBJ3yhJeEftLLoFEe3DI&usqp=CAU");

            using (var response = request.GetResponse())
            using (var stream = response.GetResponseStream())
            {
                
                pictureBox1.Image = Bitmap.FromStream(stream);
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("Please type in your username!");
                return;
            }

            else if (textBox2.Text == "")
            {
                MessageBox.Show("Please type in your password!");
                return;
            }

            else if (!this.databaseService.Login(textBox1.Text, textBox2.Text))
            {
                MessageBox.Show("Please type in valid username and password!");
                return;
            }

            this.Hide();
            MainForm m = new MainForm(textBox1.Text);
            m.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            RegisterForm r = new RegisterForm();
            r.Show();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }
    }
}
