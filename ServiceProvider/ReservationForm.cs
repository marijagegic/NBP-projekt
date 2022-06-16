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
    public partial class ReservationForm : Form
    {
        private DatabaseService databaseService;
        private List<string> cities;

        public ReservationForm()
        {
            InitializeComponent();
            databaseService = new DatabaseService();
            this.InitializeAutoComplete();
        }

        public void InitializeAutoComplete()
        {
            AutoCompleteStringCollection allowedTypes = new AutoCompleteStringCollection();
            cities = databaseService.GetCities(textBox1.Text);
            allowedTypes.AddRange(cities.ToArray());
            textBox1.AutoCompleteCustomSource = allowedTypes;
            textBox1.AutoCompleteMode = AutoCompleteMode.Suggest;
            textBox1.AutoCompleteSource = AutoCompleteSource.CustomSource;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!cities.Contains(textBox1.Text))
            {
                MessageBox.Show("Please choose valid city!");
                return;
            }
            else if (dateTimePicker1.Value > dateTimePicker2.Value)
            {
                MessageBox.Show("Please choose valid dates!");
                return;
            }

            List<Hotel> hotels = databaseService.GetHotels(textBox1.Text, dateTimePicker1.Value, dateTimePicker2.Value, (int)numericUpDown1.Value);
            foreach (Hotel hotel in hotels)
            {
                var kontrola = new HotelControl()
                {
                    name = hotel.name,
                    stars = hotel.stars,
                    halfBoard = hotel.halfBoard,
                    fullBoard = hotel.fullBoard,
                    allInclusive = hotel.allInclusive
                };
                kontrola.pohranaKlik += (sender_, e_) =>
                {
                    textBox2.Text = e_;
                };
                kontrola.Margin = new Padding(5, 5, 5, 5);
                flowLayoutPanel1.Controls.Add(kontrola);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            dateTimePicker1.Value = DateTime.Now;
            dateTimePicker2.Value = DateTime.Now;
            numericUpDown1.Value = 1;
        }
    }
}
