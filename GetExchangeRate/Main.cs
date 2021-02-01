using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Data.SqlClient;

namespace GetExchangeRate
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            timer1.Start();
            try
            {
                XmlDocument xml = new XmlDocument();
                xml.Load("https://portal.vietcombank.com.vn/Usercontrols/TVPortal.TyGia/pXML.aspx");
                /*
                string connectionString;
                SqlConnection cnn;
                connectionString = "Data Source=LAPTOP310\\SQLLUANTT;Initial Catalog='';Integrated Security=True";
                cnn = new SqlConnection(connectionString);
                cnn.Open();
                */
                XmlNodeList Datexml;
                Datexml = xml.SelectNodes("/ExrateList/DateTime");
                tbDateTime.Text = Datexml.Item(0).InnerText;

                XmlNodeList noXml;
                noXml = xml.SelectNodes("/ExrateList/Exrate");

                for (int i = 0; i <= noXml.Count - 1; i++)
                {
                    ListViewItem item = new ListViewItem(noXml.Item(i).Attributes["CurrencyCode"].InnerText);
                    item.SubItems.Add(noXml.Item(i).Attributes["CurrencyName"].InnerText);
                    item.SubItems.Add(noXml.Item(i).Attributes["Buy"].InnerText);
                    item.SubItems.Add(noXml.Item(i).Attributes["Transfer"].InnerText);
                    item.SubItems.Add(noXml.Item(i).Attributes["Sell"].InnerText);
                    listView1.Items.Add(item);
                    /*
                    SqlDataAdapter _sqldataadapter = new SqlDataAdapter("INSERT INTO TEST..VCB_EXCHANGE_RATE(DATETIME, CODE, NAME, BUY, TRANSFER, SELL) values('" + Datexml.Item(0).InnerText + "','" + noXml.Item(i).Attributes["CurrencyCode"].InnerText + "','" + noXml.Item(i).Attributes["CurrencyName"].InnerText + "'," + Double.Parse(noXml.Item(i).Attributes["Buy"].InnerText.Replace("-", "0")) + "," + Double.Parse(noXml.Item(i).Attributes["Transfer"].InnerText.Replace("-", "0")) + "," + Double.Parse(noXml.Item(i).Attributes["Sell"].InnerText.Replace("-", "0")) + ")", cnn);
                    DataSet ds = new DataSet();
                    _sqldataadapter.Fill(ds);
                    */

                }
                //MessageBox.Show("Data were added to sql server successful!");
                //cnn.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btRefresh_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            Main_Load(sender, e);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            Main_Load(sender, e);
            this.Refresh();
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.UserClosing)
                return;

            DialogResult result;
            result = MessageBox.Show("Are you sure you want to exit?", "Exit Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes)
            {
                e.Cancel = true;
            }
        }
    }
}
