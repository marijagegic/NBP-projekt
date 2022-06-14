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

        public ReservationForm()
        {
            InitializeComponent();
            databaseService = new DatabaseService();
            this.InitializeAutoComplete();
        }

        public void InitializeAutoComplete()
        {
            AutoCompleteStringCollection allowedTypes = new AutoCompleteStringCollection();
            List<string> cities = databaseService.GetCities(textBox1.Text);
            allowedTypes.AddRange(cities.ToArray());
            textBox1.AutoCompleteCustomSource = allowedTypes;
            textBox1.AutoCompleteMode = AutoCompleteMode.Suggest;
            textBox1.AutoCompleteSource = AutoCompleteSource.CustomSource;
        }
    }
}
