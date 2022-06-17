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
    public partial class Recommendations : Form
    {
        private DatabaseService databaseService;
        string clientEmail;
        public Recommendations(string email)
        {
            InitializeComponent();
            databaseService = new DatabaseService();
            clientEmail = email;
            textBox1.Text = "any";
            textBox2.Text = "5";
            textBox3.Text = "100000";
            textBox5.Text = "5";
        }


        void GetGeneralRecommendation(string email)
        {

            DataTable hotels = new DataTable("Hotels");

            //Create the Columns in the DataTable

            DataColumn c0 = new DataColumn("Name");

            DataColumn c1 = new DataColumn("Phone");

            DataColumn c2 = new DataColumn("Address");

            //Add the Created Columns to the Datatable

            hotels.Columns.Add(c0);

            hotels.Columns.Add(c1);

            hotels.Columns.Add(c2);

            //Create 3 rows

            DataRow row;

            row = hotels.NewRow();

            row["Name"] = "Fred";

            row["Phone"] = "555-1234";

            row["Address"] = "Hollywood CA";


            dataGridView1.DataSource = hotels;
    }

        private void dataGridView1_Load(object sender, EventArgs e)
        {
            string city = textBox1.Text;
            int ageDiff = Int32.Parse(textBox2.Text);
            int dist = Int32.Parse(textBox3.Text) * 10003;
            int limit = Int32.Parse(textBox5.Text);
            List<Tuple<string, string, string>> hotelList;
            if (city == "any") {
                hotelList = databaseService.GetHotelRecommendationsForClient(clientEmail, ageDiff, limit);
            }
            else
            {
                hotelList = databaseService.GetHotelRecommendations(clientEmail, city, ageDiff, dist, limit);
            }

            DataTable Hotels = new DataTable("Hotels");
            
            DataColumn c0 = new DataColumn("Name");
            DataColumn c1 = new DataColumn("Stars");
            DataColumn c2 = new DataColumn("City");

            Hotels.Columns.Add(c0);
            Hotels.Columns.Add(c1);
            Hotels.Columns.Add(c2);

            for(int i = 0; i < hotelList.Count; i++)
            {
                DataRow row;
                row = Hotels.NewRow();
                row["Name"] = hotelList[i].Item1;
                row["Stars"] = hotelList[i].Item2;
                row["City"] = hotelList[i].Item3;
                Hotels.Rows.Add(row);
            }

            
            dataGridView1.DataSource = Hotels;

            dataGridView1.AutoGenerateColumns = true;
            
            dataGridView1.Refresh();
        }
    private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1_Load(sender, e);
        }


    }
}
