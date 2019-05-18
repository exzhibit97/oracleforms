using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OracleDbForms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string connStr = "DATA SOURCE = 192.168.60.3:1521/baza1;PASSWORD=patryk123;USER ID=c##patryk";


        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                OracleConnection connection = new OracleConnection(connStr);
                OracleCommand cmd = new OracleCommand("RESCHEDULED_VISITS", connection);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("students", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                connection.Open();
                cmd.ExecuteNonQuery();
                OracleDataReader reader = ((OracleRefCursor)cmd.Parameters["students"].Value).GetDataReader();
                dataGridView1.Columns.Clear();
                dataGridView1.Rows.Clear();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    dataGridView1.Columns.Add(reader.GetName(i), reader.GetName(i));
                }
                while (reader.Read())
                {
                    string[] row = new string[reader.FieldCount];
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        row[i] = reader.GetValue(i).ToString();
                    }
                    dataGridView1.Rows.Add(row);
                }
                reader.Close();

                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void createDoctor(string first_name, string last_name, string doc_pesel, int doc_gender, DateTime doc_birth_date, int doc_dep, string doc_phone, string doc_email)
        {
            using (OracleConnection connection = new OracleConnection(connStr))
            {
                OracleCommand cmd = new OracleCommand("CREATE_DOCTOR", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("DOCTOR_FIRST_NAME", OracleDbType.Varchar2).Value = textBox1;
                cmd.Parameters.Add("DOCTOR_LAST_NAME", OracleDbType.Varchar2).Value = textBox2;
                cmd.Parameters.Add("DOCTOR_PESEL", OracleDbType.Varchar2).Value = textBox3;
                cmd.Parameters.Add("GENDER_ID", OracleDbType.Int32).Value = 1;
                cmd.Parameters.Add("DOCTOR_BIRTHDATE", OracleDbType.Date).Value = textBox6;
                cmd.Parameters.Add("DEPARTMENT_ID", OracleDbType.Int32).Value = 1;
                cmd.Parameters.Add("DOCTOR_PHONE", OracleDbType.Varchar2).Value = textBox4;
                cmd.Parameters.Add("DOCTOR_EMAIL", OracleDbType.Varchar2).Value = textBox5;

                connection.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'dataSet2.DEPARTMENT' table. You can move, or remove it, as needed.
            this.dEPARTMENTTableAdapter.Fill(this.dataSet2.DEPARTMENT);
            // TODO: This line of code loads data into the 'gender.GENDER' table. You can move, or remove it, as needed.
            this.gENDERTableAdapter.Fill(this.gender.GENDER);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                createDoctor(textBox1.Text, textBox2.Text, textBox3.Text, 1, DateTime.Parse(textBox6.Text), 1, textBox4.Text, textBox5.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void fillToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                this.dEPARTMENTTableAdapter.Fill(this.dataSet2.DEPARTMENT);
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }

        }
    }
}
