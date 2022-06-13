﻿using System;
using System.Linq;
using System.Windows.Forms;
using Neo4j.Driver;

namespace ServiceProvider
{
    public partial class Form1 : Form
    {
        private DatabaseService databaseService;
        public Form1()
        {
            InitializeComponent();
            databaseService = new DatabaseService();
            Client client = databaseService.GetClientByName("Marijo", "Kibbye");
            Console.WriteLine(client.firstName + " " + client.lastName + ", " + client.country);
        }
    }
}
