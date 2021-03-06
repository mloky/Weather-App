﻿/*Changelog:
    
    -Website URL changed from http://www.nea.gov.sg/api/WebAPI/ to http://api.nea.gov.sg/api/WebAPI/ since 1 Oct 2016

*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Net.Sockets;
using System.Net;

namespace WeatherApp
{
    public partial class Form1 : Form
    {
        XDocument doc;
        DataTable datatable = new DataTable();

        string NEA_Key= "";

        public Form1()
        {
            InitializeComponent();
        }
        public void Form1_Load(object sender,EventArgs e)
        {
            NEA_Key = WindowsFormsApplication2.Properties.Settings.Default.apiKey;
            
            label1.Text = "Weather App";
            datatable.Columns.Add("Place", typeof(string));
            datatable.Columns.Add("Status", typeof(string));            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                doc = XDocument.Load("http://api.nea.gov.sg/api/WebAPI/?dataset=2hr_nowcast&keyref=" + NEA_Key);
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
                return;
            }
            
            datatable.Rows.Clear();
            var areas = doc.Descendants("area");
            var forecast_period = doc.Descendants("validTime");
            foreach (var period in forecast_period)
            {
                textBox1.Text = "Valid for: "+period.Value;
            }
            var forecast_issue = doc.Descendants("forecastIssue");
            foreach (var issue in forecast_issue)
            {
                var date = issue.Attribute("date").Value;
                var time = issue.Attribute("time").Value;
                textBox1.Text = textBox1.Text + "\r\n\r\n\r\n" +"Issued on: " + date + "   " + time;
            }
            textBox1.Text = textBox1.Text + "\r\n\r\n\r\nCurrent Time: " + DateTime.Now.ToShortTimeString();
            foreach (var area in areas)
            {
                var name = area.Attribute("name").Value;
                var status = statusConverter(area.Attribute("forecast").Value);
                DataRow row = datatable.NewRow();
                row["Place"] = name;
                row["Status"] = status;
                datatable.Rows.Add(row);
            }
            dataGridView1.DataSource = datatable;
            dataGridView1.AutoResizeColumns();
            dataGridView1.Columns["Place"].Width += 10;
            dataGridView1.Columns["Status"].Width = dataGridView1.Columns["Place"].Width;
            dataGridView1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }
        private string statusConverter(string status)
        {
            switch (status)
            {
                case "BR":
                    return "Mist";
                case "CL":
                    return "Cloudy";
                case "DR":
                    return "Drizzle";
                case "FA":
                    return "Fair (Day)";
                case "FG":
                    return "Fog";
                case "FN":
                    return "Fair (Night)";
                case "FW":
                    return "Fair & Warm";
                case "HG":
                    return "Heavy Thundery Showers with Gusty Winds";
                case "HR":
                    return "Heavy Rain";
                case "HS":
                    return "Heavy Showers";
                case "HT":
                    return "Heavy Thundery Showers";
                case "HZ":
                    return "Hazy";
                case "LH":
                    return "Slightly Hazy";
                case "LR":
                    return "Light Rain";
                case "LS":
                    return "Light Showers";
                case "OC":
                    return "Overcast";
                case "PC":
                    return "Partly Cloudy (Day)";
                case "PN":
                    return "Partly Cloudy (Night)";
                case "PS":
                    return "Passing Showers";
                case "RA":
                    return "Moderate Rain";
                case "SH":
                    return "Showers";
                case "SK":
                    return "Strong Winds, Showers";
                case "SN":
                    return "Snow";
                case "SR":
                    return "Strong Winds, Rain";
                case "SS":
                    return "Snow Showers";
                case "SU":
                    return "Sunny";
                case "SW":
                    return "Strong Winds";
                case "TL":
                    return "Thundery Showers";
                case "WC":
                    return "Windy, Cloudy";
                case "WD":
                    return "Windy";
                case "WF":
                    return "Windy, Fair";
                case "WR":
                    return "Windy, Rain";
                case "WS":
                    return "Windy, Showers";
                default:
                    return "Invalid Status";               
            }

        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            textBox2.Text = dataGridView1[e.ColumnIndex, e.RowIndex].Value.ToString();
        }

        private void apiKeyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            apiKeyTextBox.Visible = true;
            button2.Visible = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button2.Visible = false;
            NEA_Key = apiKeyTextBox.Text;
            WindowsFormsApplication2.Properties.Settings.Default.apiKey = NEA_Key;
            WindowsFormsApplication2.Properties.Settings.Default.Save();
            apiKeyTextBox.Visible = false;
        }        
    }
}
