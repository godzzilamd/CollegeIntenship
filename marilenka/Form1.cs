using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;

namespace marilenka
{
    public partial class Form1 : Form
    {

        static string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\Market.mdf;Integrated Security=True";
        SqlCommand sCommand;
        SqlDataAdapter sAdapter;
        SqlCommandBuilder sBuilder;
        public DataSet sDs;
        DataTable sTable;

        SqlCommand sCommand2;
        SqlDataAdapter sAdapter2;
        SqlCommandBuilder sBuilder2;
        public DataSet sDs2;
        DataTable sTable2;

        public Form1()
        {
            InitializeComponent();
        }

        private void Here(string sql)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            sCommand2 = new SqlCommand(sql, connection);

            sAdapter2 = new SqlDataAdapter(sCommand2);
            sBuilder2 = new SqlCommandBuilder(sAdapter2);
            connection.Open();
            sDs2 = new DataSet();
            sAdapter2.Fill(sDs2, "Produse");
            sTable2 = sDs2.Tables["Produse"];
            connection.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string sql = "Select * FROM Produse";
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            sCommand = new SqlCommand(sql, connection);
            sAdapter = new SqlDataAdapter(sCommand);
            sBuilder = new SqlCommandBuilder(sAdapter);
            sDs = new DataSet();
            sAdapter.Fill(sDs, "Produse");
            sTable = sDs.Tables["Produse"];
            connection.Close();
            dataGridView1.DataSource = sDs.Tables["Produse"];
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataRow newlinie = sDs.Tables["Produse"].NewRow();

            newlinie["Id"] = textBox1.Text;
            newlinie["Denumire"] = textBox2.Text;
            newlinie["Categorie"] = textBox8.Text;
            newlinie["Data_producerii"] = textBox3.Text;
            newlinie["Data_expirarii"] = textBox4.Text;
            newlinie["Reducere"] = textBox5.Text;
            newlinie["Pretul"] = textBox6.Text;

            sDs.Tables["Produse"].Rows.Add(newlinie);

            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            textBox8.Text = "";

            sAdapter.Update(sDs.Tables["Produse"]);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DateTime lik = Convert.ToDateTime(textBox7.Text);
            string date = lik.ToString("yyyy-MM-dd");
            string sql = "Update Produse Set Pretul=0 Where Data_expirarii<" + "'" + date + "'";
            Here(sql);
            sql = "Update Produse Set Reducere=100 Where Data_expirarii<" + "'" + date + "'";
            Here(sql);

            sql = "Update Produse Set Pretul=Pretul*0.8 Where DATEDIFF(day,Data_producerii,'" + date + "') = DATEDIFF(day,'" + date + "',Data_expirarii);";
            Here(sql);
            sql = "Update Produse Set Reducere=20 Where DATEDIFF(day,Data_producerii,'" + date + "') = DATEDIFF(day,'" + date + "',Data_expirarii);";
            Here(sql);

            sql = "Update Produse Set Pretul=Pretul*0.5 Where DATEDIFF(day,Data_producerii,Data_expirarii)/4 > DATEDIFF(day,'" + date + "',Data_expirarii);";
            Here(sql);
            sql = "Update Produse Set Reducere=50 Where DATEDIFF(day,Data_producerii,Data_expirarii)/4 > DATEDIFF(day,'" + date + "',Data_expirarii);";
            Here(sql);

            string sql2 = "Select * From Produse;";
            Here(sql2);


            dataGridView1.DataSource = sDs2.Tables["Produse"];

            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DateTime lik = Convert.ToDateTime(textBox7.Text);
            string date = lik.ToString("yyyy-MM-dd");
            string sql = "Select * From Produse Where data_expirarii<" + "'" + date + "'";
            Here(sql);
            dataGridView1.DataSource = sDs2.Tables["Produse"];
            MessageBox.Show("Acestea sunt produsele cu termenul de valabilitate expirat");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string sql = "Select * From Produse Where Reducere=50 Order by Pretul DESC ;";
            Here(sql);
            dataGridView1.DataSource = sDs2.Tables["Produse"];
            MessageBox.Show("Acestea sunt produsele cu o reducere de 50% in ordine crescatoare");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string sql = "Select * From Produse Where Reducere=20 Order by Pretul DESC ;";
            Here(sql);
            dataGridView1.DataSource = sDs2.Tables["Produse"];
            MessageBox.Show("Acestea sunt produsele cu o reducere de 20% in ordine crescatoare");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            string sql = "SELECT Count(Id) From Produse Where DATEDIFF(year,Data_producerii,Data_expirarii)>=1;";
            Here(sql);
            dataGridView1.DataSource = sDs2.Tables["Produse"];
            MessageBox.Show("Acestea sunt produsele ci termenul de valabilitate de cel putin un an");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            string sql = "SELECT * From Produse Where DATEDIFF(month,Data_producerii,Data_expirarii)<=1;";
            Here(sql);
            dataGridView1.DataSource = sDs2.Tables["Produse"];
            MessageBox.Show("Acestea sunt produsele ci termenul de valabilitate de cel mult o luna");
        }

        private void button9_Click(object sender, EventArgs e)
        {
            string sql = "SELECT * From Produse Where DATEDIFF(day,Data_producerii,Data_expirarii)<=5;";
            Here(sql);
            dataGridView1.DataSource = sDs2.Tables["Produse"];
            MessageBox.Show("Acestea sunt produsele ci termenul de valabilitate de cel mult 5 zile");
        }

        private void button10_Click(object sender, EventArgs e)
        {
            DateTime lik = Convert.ToDateTime(textBox7.Text);
            string date = lik.ToString("yyyy-MM-dd");
            string sql = "Select * From Produse Where data_expirarii<" + "'" + date + "'";
            Here(sql);

            var lines = new List<string>();
            string[] columnNames = sTable2.Columns.Cast<DataColumn>().
                                              Select(column => column.ColumnName).
                                              ToArray();
            var header = string.Join(",", columnNames);
            lines.Add(header);
            var valueLines = sTable2.AsEnumerable()
                               .Select(row => string.Join(",", row.ItemArray));
            lines.AddRange(valueLines);
            File.WriteAllLines("Excel.csv", lines);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            string sql = "SELECT Categorie,Count(Id) From Produse Group by Categorie;";
            Here(sql);
            dataGridView1.DataSource = sDs2.Tables["Produse"];
        }
    }
}
