using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ServiceProvider
{
    public partial class MainForm : Form
    {
        private DatabaseService databaseService;
        string clientEmail;
        public MainForm(string email)
        {
            InitializeComponent();
            databaseService = new DatabaseService();
            clientEmail = email;

            var request = WebRequest.Create("https://hips.hearstapps.com/hmg-prod.s3.amazonaws.com/images/morrisadjmiarchitects-wythehotel-jimibillingsley-1-1180x791-1563201082.png?crop=0.674xw:1.00xh;0.254xw,0&resize=480:*");

            using (var response = request.GetResponse())
            using (var stream = response.GetResponseStream())
            {

                pictureBox1.Image = Bitmap.FromStream(stream);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            LoginForm f = new LoginForm();
            f.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ReservationForm p = new ReservationForm();
            p.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Recommendations r = new Recommendations(clientEmail);
            r.ShowDialog();
        }
    }
}
