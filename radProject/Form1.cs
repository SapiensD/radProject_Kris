using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace radProject
{
    public partial class Form1 : Form
    {
        public static List<string> usersID = new List<string>();
        public static string fileName = "D:/report.xlsx";
        public static DateTime dateFrom;
        public static DateTime dateTo;
        public Form1()
        {
            InitializeComponent();
            
            update();

            dateTimePicker1.MinDate = new DateTime(1985, 6, 20);
            dateTimePicker1.MaxDate = DateTime.Today;
            dateTimePicker2.MinDate = new DateTime(1985, 6, 20);
            dateTimePicker2.MaxDate = DateTime.Today;
        }
        public void update()
        {
            dataGridView1.DataSource = PostgresConnect.dbGetListExpenses();
            dataGridView2.DataSource = PostgresConnect.dbGetListDepartments();
            dataGridView3.DataSource = PostgresConnect.dbGetListExpenses();
            dataGridView4.DataSource = PostgresConnect.dbGetListExpenseReport();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // достаём номер строки, на которую нажали
            // без повторений
            if(e.RowIndex >= 0)
            {
                bool flag = true;
                for (int i = 0; i < usersID.Count; i++)
                    if (dataGridView1[1, e.RowIndex].Value.ToString() == usersID[i])
                    {
                        flag = false;
                        usersID.Remove(dataGridView1[1, e.RowIndex].Value.ToString());
                    }

                if (flag)
                    usersID.Add(dataGridView1[1, e.RowIndex].Value.ToString());

                if (dataGridView1[0, e.RowIndex].Value == "+++")
                    dataGridView1[0, e.RowIndex].Value = "";
                else
                    dataGridView1[0, e.RowIndex].Value = "+++";
            }
        }

        private void label1_Click(object sender, DataGridViewCellEventArgs e) { }
        private void button1_Click(object sender, EventArgs e) 
        {
            // ЗАПРОС В БД : ПОЛУЧИТЬ ВСЕХ ПОЛЬЗОВАТЕЛЕЙ из usersID И НАЙТИ их в таблице sales со значением столбца shipped = "false"
            // создать excel файл и сделать выгрузку данных 

            dateFrom = dateTimePicker1.Value;
            dateTo = dateTimePicker2.Value;

            excelReport newReport = new excelReport(PostgresConnect.dbGetReport(usersID, dateFrom, dateTo), fileName); // получаю отчёт из бд и печатаю отчёт
            newReport.createReport();
            MessageBox.Show("Успешно!");
        }

        private void label1_Click(object sender, EventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }

        private void label2_Click_1(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {

        }


        public static int lastClick1 = 0;
        public static int lastClick2 = 0;
        public static int lastClick3 = 0;

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                dataGridView2[0, lastClick1].Value = "";
                dataGridView2[0, e.RowIndex].Value = "*";
                lastClick1 = e.RowIndex;

                departmentName.Text = dataGridView2[2, e.RowIndex].Value.ToString();
            }
        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex >=0)
            {
                dataGridView3[0, lastClick2].Value = "";
                dataGridView3[0, e.RowIndex].Value = "*";
                lastClick2 = e.RowIndex;

                expenseName.Text = dataGridView3[2, e.RowIndex].Value.ToString();
            }
        }

        private void dataGridView4_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex>=0)
            {
                dataGridView4[0, lastClick3].Value = "";
                dataGridView4[0, e.RowIndex].Value = "*";
                lastClick3 = e.RowIndex;

                reportDepartment.Text = dataGridView4[2, e.RowIndex].Value.ToString();
                reportExpense.Text = dataGridView4[3, e.RowIndex].Value.ToString();
                reportAmount.Text = dataGridView4[4, e.RowIndex].Value.ToString();
                reportDate.Value = (DateTime)dataGridView4[5, e.RowIndex].Value;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            PostgresConnect.dbUpdateDepartments(lastClick1, departmentName.Text);
            update();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            PostgresConnect.dbInsertDepartment(departmentName.Text);
            update();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            PostgresConnect.dbUpdateExpenses(lastClick2, expenseName.Text);
            update();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            PostgresConnect.dbInsertExpense(expenseName.Text);
            update();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            PostgresConnect.dbUpdateExpenseReport(lastClick3, reportDepartment.Text, reportExpense.Text, reportAmount.Text, reportDate.Value.Year + "-" + reportDate.Value.Month + "-" + reportDate.Value.Day);
            update();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            PostgresConnect.dbInsertExpenseReport(reportDepartment.Text, reportExpense.Text, reportAmount.Text, reportDate.Value.Year + "-" + reportDate.Value.Month + "-" + reportDate.Value.Day);
            update();
        }

        private void reportDepartment_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if(reportDepartment.Text != "")
                {
                    int result = Convert.ToInt32(reportDepartment.Text);
                }
            }
            catch(Exception ex)
            {
                reportDepartment.Text = "";
                MessageBox.Show(ex.Message);
            }
        }

        private void reportExpense_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (reportExpense.Text != "")
                {
                    int result = Convert.ToInt32(reportExpense.Text);
                }
            }
            catch (Exception ex)
            {
                reportExpense.Text = "";
                MessageBox.Show(ex.Message);
            }
        }

        private void reportAmount_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (reportAmount.Text != "")
                {
                    int result = Convert.ToInt32(reportAmount.Text);
                }
            }
            catch (Exception ex)
            {
                reportAmount.Text = "";
                MessageBox.Show(ex.Message);
            }
        }
    }
}
