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
        public Recommendations(string email)
        {
            InitializeComponent();
            databaseService = new DatabaseService();
            GetGeneralRecommendation(email);
        }


        void GetGeneralRecommendation(string email)
        {
            List<string> hotels = databaseService.GetHotelRecommendationsForClient(email, 5, 3);
            string rec = "";
            foreach(string s in hotels)
            {
                rec += s + "\n";
            }
            hotelList.Text = rec;
        }

    }
}
