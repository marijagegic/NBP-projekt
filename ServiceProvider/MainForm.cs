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
    public partial class MainForm : Form
    {
        private DatabaseService databaseService;

        public MainForm()
        {
            InitializeComponent();
            databaseService = new DatabaseService();
        }
    }
}
