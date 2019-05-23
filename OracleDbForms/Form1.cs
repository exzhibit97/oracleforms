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
        private void createDoctor(string textBox1, string textBox2, string textBox3, int comboBox1, string dateTimePicker1, int comboBox2, string textBox4, string textBox5)
        {
            try
            {
                using (OracleConnection connection = new OracleConnection(connStr))
                {
                    OracleCommand cmd = new OracleCommand("CREATE_DOCTOR", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("DOCTOR_FIRST_NAME", OracleDbType.Varchar2).Value = textBox1;
                    cmd.Parameters.Add("DOCTOR_LAST_NAME", OracleDbType.Varchar2).Value = textBox2;
                    cmd.Parameters.Add("DOCTOR_PESEL", OracleDbType.Varchar2).Value = textBox3;
                    cmd.Parameters.Add("GENDER_ID", OracleDbType.Int32).Value = comboBox1;
                    cmd.Parameters.Add("DOCTOR_BIRTHDATE", OracleDbType.Varchar2).Value = dateTimePicker1;
                    cmd.Parameters.Add("DEPARTMENT_ID", OracleDbType.Int32).Value = comboBox2;
                    cmd.Parameters.Add("DOCTOR_PHONE", OracleDbType.Varchar2).Value = textBox4;
                    cmd.Parameters.Add("DOCTOR_EMAIL", OracleDbType.Varchar2).Value = textBox5;

                    connection.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Succesfully created doctor!");
                    
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            createDoctor(textBox1.Text, textBox2.Text, textBox3.Text, comboBox1.SelectedIndex+1, dateTimePicker1.Text, comboBox2.SelectedIndex+1, textBox4.Text, textBox5.Text);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'visitSum.TEMP' table. You can move, or remove it, as needed.
            OracleConnection connection = new OracleConnection(connStr);
            OracleCommand cmd = new OracleCommand("MONTHLY_VISIT_SUMMARY", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("monthly_visits", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("monthly_skipped_due_to_patient", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("monthly_skipped_due_to_doctor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            connection.Open();
            cmd.ExecuteNonQuery();
            

            this.tEMPTableAdapter2.Fill(this.visitSum.TEMP);
            
            // TODO: This line of code loads data into the 'agechart.TEMP_AGE' table. You can move, or remove it, as needed.
            this.tEMP_AGETableAdapter.Fill(this.agechart.TEMP_AGE);

            // TODO: This line of code loads data into the 'department.DEPARTMENT' table. You can move, or remove it, as needed.
            this.dEPARTMENTTableAdapter.Fill(this.department.DEPARTMENT);
            // TODO: This line of code loads data into the 'gender.GENDER' table. You can move, or remove it, as needed.
            this.gENDERTableAdapter.Fill(this.gender.GENDER);
            

        }



        private void fillToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                //this.dEPARTMENTTableAdapter.Fill(this.dataSet2.DEPARTMENT);
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                using (OracleConnection connection = new OracleConnection(connStr))
                {
                    DataTable dt = new DataTable();
                    OracleCommand cmd = new OracleCommand("RESCHEDULING_EFFECT", connection);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("patients", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                    connection.Open();
                    cmd.ExecuteNonQuery();
                    OracleDataReader reader = ((OracleRefCursor)cmd.Parameters["patients"].Value).GetDataReader();
                    dataGridView2.Columns.Clear();
                    dataGridView2.Rows.Clear();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        dataGridView2.Columns.Add(reader.GetName(i), reader.GetName(i));
                    }
                    while (reader.Read())
                    {
                        string[] row = new string[reader.FieldCount];
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            row[i] = reader.GetValue(i).ToString();
                        }
                        dataGridView2.Rows.Add(row);
                    }
                    reader.Close();

                    connection.Close();
                }
            }               
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void tabControl3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void chart1_Load(object sender, EventArgs e)
        {
            //OracleConnection connection = new OracleConnection(connStr);
            //OracleCommand cmd = new OracleCommand("AGE_CHART", connection);
            //cmd.CommandType = CommandType.StoredProcedure;

            //cmd.Parameters.Add("AGEU18", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            //cmd.Parameters.Add("AGEU25", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            //cmd.Parameters.Add("AGEU40", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            //cmd.Parameters.Add("AGEU60", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            //cmd.Parameters.Add("AGEO60", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            

        }

        private void button5_Click(object sender, EventArgs e)
        {
            createMedicine(medicineName.Text, Decimal.Parse(medicinePrice.Text)); 
        }
        private void createMedicine(string medicineName, decimal medicinePrice)
        {
            try
            {
                using (OracleConnection connection = new OracleConnection(connStr))
                {
                    OracleCommand cmd = new OracleCommand("CREATE_MEDICINE", connection);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("MEDICINE_NAME", OracleDbType.Varchar2, 255).Value = medicineName;
                    cmd.Parameters.Add("MEDICINE_PRICE", OracleDbType.Decimal).Value = medicinePrice;

                    connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
