using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;
using MySql.Data.MySqlClient;
namespace WindowsFormsApp5
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public void button1_Click(object sender, EventArgs e) //Выводим новости на экран
        {
                string uri = textBox1.Text;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                string text = null;
                text = reader.ReadToEnd();
                string[] separatingChars = { "<title>", "</title>", "<link>", "</link>", "<description>", "</description>", "<pubDate>", "</pubDate>" };
                string[] words = text.Split(separatingChars, System.StringSplitOptions.RemoveEmptyEntries);
                for (int i = 11; i < words.Length; i = i + 2)
                {
                    richTextBox1.AppendText(words[i] + "\n\n");
                }
        }
        private void button2_Click(object sender, EventArgs e) //Читаем новости из БД
        {
            string connStr = "server = localhost; user = root; database = lab; password = ''; Charset = UTF8;";
            MySqlConnection conn = new MySqlConnection(connStr);
            conn.Open();
            string sql = "SELECT  * FROM lab6";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                richTextBox2.AppendText(reader[1].ToString() +  "\n\n"+ reader[2].ToString() + "\n\n" + reader[3].ToString() +"\n\n" + reader[4].ToString() + "\n\n");
            }
            reader.Close();
            conn.Close();
        }

        private void button3_Click(object sender, EventArgs e) //Грузим новости в БД
        {
            string uri = textBox1.Text;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            string text = null;
            text = reader.ReadToEnd();
            string[] separatingChars = { "<title>", "</title>", "<link>", "</link>", "<description>", "</description>", "<pubDate>", "</pubDate>" };
            string[] words = text.Split(separatingChars, System.StringSplitOptions.RemoveEmptyEntries);
            //Счётчики строк для айди, тайтла, линков, описания, пабдаты
            int idc = 0;
            int titlec = 0;
            int linkc = 2;
            int descc = 4;
            int pubdatec = 6;
            int linesc = 0;
            string connectionString = "server = localhost; user = root; database = lab; password = ''; Charset = UTF8;";
            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();
            string query1 = "DELETE FROM lab6 WHERE 1";
            MySqlCommand comanda = new MySqlCommand(query1, conn);
            comanda.ExecuteNonQuery();
            do
            {
                string query = "INSERT INTO lab6 ( id, title, link, description, pubdate) VALUES ( @id, @title, @link, @description, @pubdate)";
                MySqlCommand command = new MySqlCommand(query, conn);
                command.Parameters.Add("@id", MySqlDbType.Int32).Value = idc;
                command.Parameters.Add("@title", MySqlDbType.Text).Value = richTextBox1.Lines[titlec];
                command.Parameters.Add("@link", MySqlDbType.LongText).Value = richTextBox1.Lines[linkc];
                command.Parameters.Add("@description", MySqlDbType.Text).Value = richTextBox1.Lines[descc];
                command.Parameters.Add("@pubdate", MySqlDbType.Text).Value = richTextBox1.Lines[pubdatec];
                command.ExecuteNonQuery();
                idc = idc + 1;
                titlec = titlec + 8;
                linkc = linkc + 8;
                descc = descc + 8;
                pubdatec = pubdatec + 8;
                linesc = linesc + 9;
            } while (linesc < words.Length);
            conn.Close();
        }
    }
    }

    
