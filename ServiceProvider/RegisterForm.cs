using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Neo4j.Driver;

namespace ServiceProvider
{
    public partial class RegisterForm : Form
    {
        private DatabaseService databaseService;
        public RegisterForm()
        {
            InitializeComponent();
            databaseService = new DatabaseService();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool okPassword = this.databaseService.ValidatePassword(textBox7.Text);
            if (okPassword)
            {
                // TODO try catch ako netko za spol unese nešto ručno osim M,F
                // TODO try catch ako netko za PIN unese string umjesto int
                this.databaseService.Register(textBox1.Text, textBox2.Text,
                            comboBox1.SelectedItem.ToString(), textBox3.Text, textBox4.Text,
                            dateTimePicker1.Value.ToString("yyyy-MM-dd"), textBox5.Text,
                            Int64.Parse(textBox6.Text), textBox7.Text
                            );
                MessageBox.Show("Registration successful.");
                this.Close();

                MainForm m = new MainForm();
                m.Show();
            }
            else
            {
                MessageBox.Show("Please set another password.");
                return;
            }
        }
    }
}
