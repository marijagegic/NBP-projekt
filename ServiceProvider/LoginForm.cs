using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
