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
            if (textBox1.Text.Equals("") || textBox2.Text.Equals("") || textBox3.Text.Equals("") ||
                textBox4.Text.Equals("") || textBox5.Text.Equals("") || textBox7.Text.Equals("")) {
                MessageBox.Show("All fields are obligatory.");
                return;
            }

            bool okGender = this.ValidateGender(comboBox1.Text);
            if (!okGender) {
                MessageBox.Show("Please select gender.");
                return;
            }

            bool okPin = this.ValidatePin(textBox6.Text);
            if (!okPin)
            {
                MessageBox.Show("Invalid PIN.");
                return;
            }

            bool okEmail = this.databaseService.ValidateEmail(textBox3.Text);
            if (okEmail)
            {
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
                MessageBox.Show("Please use another email.");
                return;
            }
        }

        private bool ValidateGender(string gender) {
            List<string> genders = new List<string> {"Male", "Female", "Other"};
            return genders.Any(s => gender.Equals(s));
        }

        private bool ValidatePin(string pin) {
            try
            {
                Int64.Parse(pin);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
