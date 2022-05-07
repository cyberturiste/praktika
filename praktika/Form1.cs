using System;
using System.Collections.Generic;
using System.ComponentModel;

using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;
using System.Data;
using System.Data.SQLite;
using Newtonsoft.Json;



using System.Windows;


namespace praktika
{
    public partial class Form1 : Form
    {
        SerialPort port = new SerialPort("COM2", 9600, Parity.None, 8, StopBits.One);
        private SQLiteConnection m_dbConn;
        private SQLiteCommand m_sqlCmd;

        private int i = 0;

        public Form1()
        {
            InitializeComponent();

            port.DataReceived += new SerialDataReceivedEventHandler(OnDataReceived);
            port.Open();
            try
            {
                m_dbConn = new SQLiteConnection();
                m_sqlCmd = new SQLiteCommand();
                MessageBox.Show(
        "Выберите файл database SQLite",
        "Выбор файла");
                OpenFileDialog OPF = new OpenFileDialog();
                if (OPF.ShowDialog() == DialogResult.OK)
                {




                    m_dbConn = new SQLiteConnection("Data Source=" + OPF.FileName + ";Version=3;");
                    m_dbConn.Open();

                    DataTable dTable = new DataTable();

                    String selecet = " SELECT name FROM sqlite_master WHERE TYPE = 'table'";

                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(selecet, m_dbConn);

                    adapter.Fill(dTable);


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
        "Логическая ошибка SQL или выбранный файл не найден",
        "Ошибка");
            }
        }


        private void OnDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            var values = new List<string>();
            DateTime localDate = DateTime.Now;
            try
            {


                m_sqlCmd.Connection = m_dbConn;

                DataTable dTable = new DataTable();



                m_sqlCmd.CommandText = "INSERT INTO valueses values('" + i + "','" + localDate + "','" + port.ReadExisting() + "')";



                i++;

                m_sqlCmd.ExecuteNonQuery();

                Thread.Sleep(1500);
            }
            catch (Exception ex)
            {

            }

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            port.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {



        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dTable = new DataTable();

                dataGridView1.Rows.Clear();


                String sqlQuery = "SELECT * FROM valueses";

                SQLiteDataAdapter adapter = new SQLiteDataAdapter(sqlQuery, m_dbConn);

                adapter.Fill(dTable);

                if (dTable.Rows.Count > 0)
                {
                    dataGridView1.Rows.Clear();

                    for (int i = 0; i < dTable.Rows.Count; i++)
                        dataGridView1.Rows.Add(dTable.Rows[i].ItemArray);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Логическая ошибка SQL,проверьте написание команды",
        "Ошибка");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

            m_sqlCmd.Connection = m_dbConn;
            m_sqlCmd.CommandText = "DELETE FROM valueses";
            m_sqlCmd.ExecuteNonQuery();



        }
    }
}

