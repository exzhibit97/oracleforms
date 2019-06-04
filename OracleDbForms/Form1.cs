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
using System.Windows.Forms.DataVisualization.Charting;

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
                //DataTable dt = new DataTable();
                OracleConnection connection = new OracleConnection(connStr);
                OracleCommand cmd = new OracleCommand("RESCHEDULING_STATUS_DOCTOR", connection);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("doctors", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                connection.Open();
                cmd.ExecuteNonQuery();
                OracleDataReader reader = ((OracleRefCursor)cmd.Parameters["doctors"].Value).GetDataReader();
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

        //========================
        //CREATE DOCTOR
        //========================
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
                    this.dOCTORTableAdapter.Fill(this.visit_doctor.DOCTOR);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            createDoctor(textBox1.Text, textBox2.Text, textBox3.Text, comboBox1.SelectedIndex + 1, dateTimePicker1.Text, comboBox2.SelectedIndex + 1, textBox4.Text, textBox5.Text);
        }

        //========================
        //CREATE PATIENT
        //========================
        private void createPatient(string patient_firstname, string patient_lastname, string patient_pesel, int patient_gender, string patient_birthdate, string patient_phone, string patient_email)
        {
            try
            {
                using (OracleConnection connection = new OracleConnection(connStr))
                {
                    OracleCommand cmd = new OracleCommand("CREATE_PATIENT", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("PAT_FIRSTNAME", OracleDbType.Varchar2).Value = patient_firstname;
                    cmd.Parameters.Add("PAT_LASTNAME", OracleDbType.Varchar2).Value = patient_lastname;
                    cmd.Parameters.Add("PAT_PESEL", OracleDbType.Varchar2).Value = patient_pesel;
                    cmd.Parameters.Add("PAT_GENDER_ID", OracleDbType.Int32).Value = patient_gender;
                    cmd.Parameters.Add("PAT_BIRTHDATE", OracleDbType.Varchar2).Value = patient_birthdate;
                    cmd.Parameters.Add("PAT_PHONE", OracleDbType.Varchar2).Value = patient_phone;
                    cmd.Parameters.Add("PAT_EMAIL", OracleDbType.Varchar2).Value = patient_email;

                    connection.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Succesfully created patient " + patient_firstname + patient_lastname + "!");
                    this.pATIENTTableAdapter.Fill(this.patient_status.PATIENT);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            createPatient(patient_firstname.Text, patient_lastname.Text, patient_pesel.Text, patient_gender.SelectedIndex + 1, patient_birthdate.Text, patient_phone.Text, patient_email.Text);
        }
        //========================
        //CREATE DEPARTMENT
        //========================
        private void createDepartment(string dep_name, string dep_email, string dep_phone)
        {
            try
            {
                using (OracleConnection connection = new OracleConnection(connStr))
                {
                    OracleCommand cmd = new OracleCommand("CREATE_DEPARTMENT", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("dep_name", OracleDbType.Varchar2).Value = dep_name;
                    cmd.Parameters.Add("dep_email", OracleDbType.Varchar2).Value = dep_email;
                    cmd.Parameters.Add("dep_phone", OracleDbType.Varchar2).Value = dep_phone;


                    connection.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Succesfully created department!" + dep_name);
                    this.dEPARTMENTTableAdapter.Fill(this.department.DEPARTMENT);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void button12_Click(object sender, EventArgs e)
        {
            createDepartment(dep_name.Text, dep_email.Text, dep_phone.Text);
        }

        //========================
        //CREATE VISIT
        //========================
        private void createVisit(int visit_type, string scheduled_date, int status_id, int patient_id, int doctor_id, int employee_id)
        {
            try
            {
                using (OracleConnection connection = new OracleConnection(connStr))
                {
                    OracleCommand cmd = new OracleCommand("CREATE_VISIT", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("V_TYPE", OracleDbType.Int32).Value = visit_type;
                    cmd.Parameters.Add("V_SCHEDULED_DATE", OracleDbType.Varchar2).Value = scheduled_date;
                    cmd.Parameters.Add("V_STATUS_ID", OracleDbType.Int32).Value = status_id;
                    cmd.Parameters.Add("V_RESCHEDULED_ID", OracleDbType.Int32).Value = null;
                    cmd.Parameters.Add("V_PATIENT_ID", OracleDbType.Int32).Value = patient_id;
                    cmd.Parameters.Add("V_DOCTOR_ID", OracleDbType.Int32).Value = doctor_id;
                    cmd.Parameters.Add("V_EMPLOYEE_ID", OracleDbType.Int32).Value = employee_id;
                    cmd.Parameters.Add("V_PRESCRIPTION_ID", OracleDbType.Int32).Value = null;

                    connection.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Succesfully created visit!");
                    this.vISITTableAdapter.Fill(this.visit_grid.VISIT);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            createPrescription(medicine_combo.SelectedIndex + 1, instructions.Text);
            createVisit(visit_type.SelectedIndex + 1, scheduled_date.Text, status_id.SelectedIndex + 1, patient_id.SelectedIndex + 1, doctor_id.SelectedIndex + 1, employee_id.SelectedIndex + 1);
        }

        //========================
        //CREATE EMPLOYEE
        //========================
        private void createEmployee(string textBox1, string textBox2, string textBox3, int comboBox1, string dateTimePicker1, int comboBox2, string textBox4, string textBox5)
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
                    MessageBox.Show("Succesfully created employee!");
                    this.eMPLOYEETableAdapter.Fill(this.visit_emp.EMPLOYEE);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //========================
        //CREATE MEDICINE
        //========================
        private void createMedicine(string medicineName, decimal medicinePrice, int replacement_for)
        {
            try
            {
                using (OracleConnection connection = new OracleConnection(connStr))
                {
                    OracleCommand cmd = new OracleCommand("CREATE_MEDICINE", connection);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("MEDICINE_NAME", OracleDbType.Varchar2, 255).Value = medicineName;
                    cmd.Parameters.Add("MEDICINE_PRICE", OracleDbType.Decimal).Value = medicinePrice;
                    cmd.Parameters.Add("REAPLACEMENT_FOR", OracleDbType.Int32).Value = replacement_for;

                    connection.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Succesfully created medicine!");
                    this.mEDICINETableAdapter.Fill(this.medicine.MEDICINE);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            createMedicine(medicineName.Text, Decimal.Parse(medicinePrice.Text), replacement_for.SelectedIndex + 1);
        }

        //========================
        //CREATE PRESCRIPTION
        //========================
        private void createPrescription(int medicine_combo, string instructions)
        {
            try
            {
                using (OracleConnection connection = new OracleConnection(connStr))
                {
                    OracleCommand cmd = new OracleCommand("CREATE_PRESCRIPTION", connection);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("PRESCRIPTION_MEDICINE", OracleDbType.Int32).Value = medicine_combo;
                    cmd.Parameters.Add("PRESCRIPTION_INSTRUCTIONS", OracleDbType.Varchar2, 255).Value = instructions;

                    connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //========================
        //CREATE EMPLOYEE
        //========================

        private void createEmployee()
        {
            try
            {
                using (OracleConnection connection = new OracleConnection(connStr))
                {
                    OracleCommand cmd = new OracleCommand("CREATE_EMPLOYEE", connection);
                    cmd.CommandType = CommandType.StoredProcedure;


                    connection.Open();
                    cmd.ExecuteNonQuery();
                    this.eMPLOYEETableAdapter.Fill(this.visit_emp.EMPLOYEE);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            createEmployee();
        }

        //======================

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'visit_grid.VISIT' table. You can move, or remove it, as needed.
            this.vISITTableAdapter.Fill(this.visit_grid.VISIT);

            // TODO: This line of code loads data into the 'visit_emp.EMPLOYEE' table. You can move, or remove it, as needed.
            this.eMPLOYEETableAdapter.Fill(this.visit_emp.EMPLOYEE);
            // TODO: This line of code loads data into the 'visit_employee.EMPLOYEEROLE' table. You can move, or remove it, as needed.
            this.eMPLOYEEROLETableAdapter.Fill(this.visit_employee.EMPLOYEEROLE);
            // TODO: This line of code loads data into the 'visit_doctor.DOCTOR' table. You can move, or remove it, as needed.
            this.dOCTORTableAdapter.Fill(this.visit_doctor.DOCTOR);
            // TODO: This line of code loads data into the 'visit_status.STATUS' table. You can move, or remove it, as needed.
            this.sTATUSTableAdapter.Fill(this.visit_status.STATUS);
            // TODO: This line of code loads data into the 'visit_type1.VISITTYPE' table. You can move, or remove it, as needed.
            this.vISITTYPETableAdapter.Fill(this.visit_type1.VISITTYPE);
            scheduled_date.MinDate = DateTime.Now;
            // TODO: This line of code loads data into the 'medicine.MEDICINE' table. You can move, or remove it, as needed.
            this.mEDICINETableAdapter.Fill(this.medicine.MEDICINE);
            // TODO: This line of code loads data into the 'difference_orig_repl.ORIGINALS_REPLACEMENTS_DIFF' table. You can move, or remove it, as needed.
            this.oRIGINALS_REPLACEMENTS_DIFFTableAdapter.Fill(this.difference_orig_repl.ORIGINALS_REPLACEMENTS_DIFF);
            // TODO: This line of code loads data into the 'patient_status.PATIENT' table. You can move, or remove it, as needed.
            this.pATIENTTableAdapter.Fill(this.patient_status.PATIENT);
            // TODO: This line of code loads data into the 'amount_replace_orig.ORIGINALS_REPLACEMENTS_AMOUNT' table. You can move, or remove it, as needed.
            this.oRIGINALS_REPLACEMENTS_AMOUNTTableAdapter.Fill(this.amount_replace_orig.ORIGINALS_REPLACEMENTS_AMOUNT);
            // TODO: This line of code loads data into the 'originals_replacements_table.ORIGINALS_REPLACEMENTS' table. You can move, or remove it, as needed.
            this.oRIGINALS_REPLACEMENTSTableAdapter1.Fill(this.originals_replacements_table.ORIGINALS_REPLACEMENTS);
            // TODO: This line of code loads data into the 'medicine_orig_repl.ORIGINALS_REPLACEMENTS' table. You can move, or remove it, as needed.

            // TODO: This line of code loads data into the 'dataSet2.ORIGINALS_REPLACEMENTS' table. You can move, or remove it, as needed.

            // TODO: This line of code loads data into the 'most_presc_medicine_monthly.MOST_PRESC_MEDICINE_MONTHLY' table. You can move, or remove it, as needed.
            this.mOST_PRESC_MEDICINE_MONTHLYTableAdapter.Fill(this.most_presc_medicine_monthly.MOST_PRESC_MEDICINE_MONTHLY);

            chart4.ChartAreas[0].AxisX.MajorGrid.LineWidth = 0;
            chart4.ChartAreas[0].AxisY.MinorGrid.LineWidth = 0;
            chart4.ChartAreas[0].AxisY.MajorGrid.Interval = 0.5;
            chart4.ChartAreas[0].AxisX.Interval = 1;
            chart4.ChartAreas[0].AxisX2.Interval = 1;

            //chart4.Series["Prescribed amount of medicine"].ToolTip = "Medicine prescribed amount: #VAL";


            // TODO: This line of code loads data into the 'monthly_presc_avg_price.MONTHLY_PRESC_AVG_PRICE' table. You can move, or remove it, as needed.
            this.mONTHLY_PRESC_AVG_PRICETableAdapter.Fill(this.monthly_presc_avg_price.MONTHLY_PRESC_AVG_PRICE);
            chart3.Series["Series4"].ToolTip = "Ilość: #VALY, Miesiąc: #VALX";
            chart3.ChartAreas[0].AxisX.Interval = 1;
            chart3.ChartAreas[0].AxisY.Interval = 10;
            // TODO: This line of code loads data into the 'visit_summary.VISIT_SUMMARY' table. You can move, or remove it, as needed.
            this.vISIT_SUMMARYTableAdapter.Fill(this.visit_summary.VISIT_SUMMARY);
            chart2.Series["Series1"].ToolTip = "Ilość: #VALY, Miesiąc: #VALX";
            chart2.Series["Series2"].ToolTip = "Ilość: #VALY, Miesiąc: #VALX";
            chart2.Series["Series3"].ToolTip = "Ilość: #VALY, Miesiąc: #VALX";
            chart2.ChartAreas[0].AxisX.Interval = 1;
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
                    OracleCommand cmd = new OracleCommand("RESCHEDULING_STATUS_PATIENTS", connection);
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
        private void ageGroupsCalculate()
        {
            try
            {
                using (OracleConnection connection = new OracleConnection(connStr))
                {
                    OracleCommand cmd = new OracleCommand("AGE_CHART", connection);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("AGEU18", OracleDbType.Int32).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("AGEU25", OracleDbType.Int32).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("AGEU40", OracleDbType.Int32).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("AGEU60", OracleDbType.Int32).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("AGEO60", OracleDbType.Int32).Direction = ParameterDirection.Output;

                    connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        //odswiezenie widoku wykresu grup wiekowych
        private void chart1_Load(object sender, EventArgs e)
        {
            try
            {
                //OracleConnection connection = new OracleConnection(connStr);
                //OracleCommand cmd = new OracleCommand("AGE_CHART", connection);
                //cmd.CommandType = CommandType.StoredProcedure;
                ageGroupsCalculate();
                this.tEMP_AGETableAdapter.Fill(this.agechart.TEMP_AGE);
                chart1.DataSource = agechart;
                chart1.DataBind();
                chart1.Update();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }



        private void REFRESH_MONTHLY_VISITS_Click(object sender, EventArgs e)
        {
            //// TODO: This line of code loads data into the 'visitSum.TEMP' table. You can move, or remove it, as needed.
            //OracleConnection connection = new OracleConnection(connStr);
            //OracleCommand cmd = new OracleCommand("MONTHLY_VISIT_SUMMARY", connection);
            //cmd.CommandType = CommandType.StoredProcedure;
            //cmd.Parameters.Add("monthly_visits", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            //cmd.Parameters.Add("monthly_skipped_due_to_patient", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            //cmd.Parameters.Add("monthly_skipped_due_to_doctor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            //cmd.Parameters.Add("scope_to_year", OracleDbType.Int32).Value = textBox6.Text;
            //connection.Open();
            //cmd.ExecuteNonQuery();

        }



        private void button6_Click(object sender, EventArgs e)
        {
            try
            {

                monthlyVisitSummary(Int32.Parse(textBox6.Text));
                this.vISIT_SUMMARYTableAdapter.Fill(this.visit_summary.VISIT_SUMMARY);
                chart2.DataSource = vISITSUMMARYBindingSource;
                chart2.DataBind();
                chart2.Update();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                MessageBox.Show("Put in year in correct format! Eg. 2019");
            }

        }

        private void monthlyVisitSummary(int textBox6)
        {
            try
            {
                using (OracleConnection connection = new OracleConnection(connStr))
                {
                    OracleCommand cmd = new OracleCommand("MONTHLY_VISIT_SUMMARY", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("monthly_visits", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("monthly_skipped_due_to_patient", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("monthly_skipped_due_to_doctor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("scope_to_year", OracleDbType.Int32).Value = textBox6;

                    connection.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Succesfully summarised visits for year " + textBox6.ToString() + "!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void monthlyMedicineSummary(int textBox7)
        {
            try
            {
                using (OracleConnection connection = new OracleConnection(connStr))
                {
                    OracleCommand cmd = new OracleCommand("MOST_PRESCRIBED_MEDICINE", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("scope_to_year", OracleDbType.Int32).Value = textBox7;

                    connection.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Succesfully summarised monthly prescribed medicine for year " + textBox7.ToString() + "!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



        private void button7_Click(object sender, EventArgs e)
        {
            try
            {

                monthlyMedicineSummary(Int32.Parse(textBox7.Text));
                this.mOST_PRESC_MEDICINE_MONTHLYTableAdapter.Fill(this.most_presc_medicine_monthly.MOST_PRESC_MEDICINE_MONTHLY);
                chart4.DataSource = most_presc_medicine_monthly;
                chart4.DataBind();
                chart4.Update();
            }
            catch (Exception ex)
            {
                // MessageBox.Show(ex.Message);
                MessageBox.Show("Put in year in correct format! Eg. 2019");
            }
        }

        private void monthlyAvgPrescription(int textBox8)
        {
            try
            {
                using (OracleConnection connection = new OracleConnection(connStr))
                {
                    OracleCommand cmd = new OracleCommand("MONTHLY_AVG_PRESC_PRICE", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("scope_to_year", OracleDbType.Int32).Value = textBox8;

                    connection.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Succesfully summarised monthly prescribed medicine for year " + textBox8.ToString() + "!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            try
            {

                monthlyAvgPrescription(Int32.Parse(textBox8.Text));
                this.mONTHLY_PRESC_AVG_PRICETableAdapter.Fill(this.monthly_presc_avg_price.MONTHLY_PRESC_AVG_PRICE);
                chart3.DataSource = monthly_presc_avg_price;
                chart3.DataBind();
                chart3.Update();
            }
            catch (Exception ex)
            {
                // MessageBox.Show(ex.Message);
                MessageBox.Show("Put in year in correct format! Eg. 2019");
            }
        }

        private void originals_replacement_diff(int textBox10)
        {
            try
            {
                using (OracleConnection connection = new OracleConnection(connStr))
                {
                    OracleCommand cmd = new OracleCommand("ORIGINALS_REPLACEMENT_DIFF", connection);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("summary", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("scope_to_year", OracleDbType.Int32).Value = textBox10;

                    //cmd.Parameters.Add("medicineWithReplacement", OracleDbType.Int32).Direction = ParameterDirection.Output;
                    //cmd.Parameters.Add("possibleMedicineReplacement", OracleDbType.Int32).Direction = ParameterDirection.Output;


                    connection.Open();
                    cmd.ExecuteNonQuery();
                    //OracleCommand cmd = new OracleCommand("RESCHEDULING_STATUS_PATIENTS", connection);
                    //cmd.CommandType = CommandType.StoredProcedure;

                    //cmd.Parameters.Add("patients", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                    //connection.Open();
                    //cmd.ExecuteNonQuery();
                    OracleDataReader reader = ((OracleRefCursor)cmd.Parameters["summary"].Value).GetDataReader();

                    while (reader.Read())
                    {
                        textBox9.Text = reader.GetValue(3).ToString();
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            try
            {
                originals_replacement_diff(Int32.Parse(textBox10.Text));
                this.oRIGINALS_REPLACEMENTS_DIFFTableAdapter.Fill(this.difference_orig_repl.ORIGINALS_REPLACEMENTS_DIFF);
                chart7.DataSource = difference_orig_repl;
                chart7.DataBind();
            }
            catch (Exception ex)
            {
                // MessageBox.Show(ex.Message);
                MessageBox.Show("Put in year in correct format! Eg. 2019");
            }

        }

        private void status_id_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (status_id.SelectedIndex == 0)
            {
                medicine_combo.Enabled = true;
                instructions.Enabled = true;
            }
            else
            {
                medicine_combo.Enabled = false;
                instructions.Enabled = false;
            }
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox6.Text))
            {
                button6.Enabled = false;
            }
            else
            {
                button6.Enabled = true;
            }
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox7.Text))
            {
                button7.Enabled = false;
            }
            else
            {
                button7.Enabled = true;
            }
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox8.Text))
            {
                button8.Enabled = false;
            }
            else
            {
                button8.Enabled = true;
            }
        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(textBox10.Text))
            {
                button9.Enabled = false;
            }
            else
            {
                button9.Enabled = true;
            }
        }

        private void amountReplaceOrig(int textBox17)
        {
            try
            {
                using (OracleConnection connection = new OracleConnection(connStr))
                {
                    OracleCommand cmd = new OracleCommand("ORIG_REPL_RATIO", connection);
                    cmd.CommandType = CommandType.StoredProcedure;

                    //cmd.Parameters.Add("medicineWithReplacement", OracleDbType.Int32).Direction = ParameterDirection.Output;
                    //cmd.Parameters.Add("medicineReplacement", OracleDbType.Int32).Direction = ParameterDirection.Output;
                    //cmd.Parameters.Add("medicineWithoutReplacement", OracleDbType.Int32).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("scope_to_year", OracleDbType.Int32).Value = textBox17;

                    connection.Open();
                    cmd.ExecuteNonQuery();


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            try
            {

                amountReplaceOrig(Int32.Parse(textBox17.Text));
                this.oRIGINALS_REPLACEMENTS_AMOUNTTableAdapter.Fill(this.amount_replace_orig.ORIGINALS_REPLACEMENTS_AMOUNT);
                chart5.DataSource = amount_replace_orig;
                chart5.DataBind();
                chart5.Update();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void summedReplaceOrig(int textBox18)
        {
            try
            {
                using (OracleConnection connection = new OracleConnection(connStr))
                {
                    OracleCommand cmd = new OracleCommand("ORIGINALS_AND_REPLACEMENTS", connection);
                    cmd.CommandType = CommandType.StoredProcedure;

                    //cmd.Parameters.Add("medicineWithReplacement", OracleDbType.Int32).Direction = ParameterDirection.Output;
                    //cmd.Parameters.Add("medicineReplacement", OracleDbType.Int32).Direction = ParameterDirection.Output;
                    //cmd.Parameters.Add("medicineWithoutReplacement", OracleDbType.Int32).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("scope_to_year", OracleDbType.Int32).Value = textBox18;

                    connection.Open();
                    cmd.ExecuteNonQuery();


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            try
            {
                summedReplaceOrig(Int32.Parse(textBox18.Text));
                this.oRIGINALS_REPLACEMENTSTableAdapter1.Fill(this.originals_replacements_table.ORIGINALS_REPLACEMENTS);
                chart6.DataSource = originals_replacements_table;
                chart6.DataBind();
                chart6.Update();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void textBox17_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox17.Text))
            {
                button14.Enabled = false;
            }
            else
            {
                button14.Enabled = true;
            }
        }

        private void textBox18_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox18.Text))
            {
                button15.Enabled = false;
            }
            else
            {
                button15.Enabled = true;
            }
        }

    }
}
