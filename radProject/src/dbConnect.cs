using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using System.Windows.Forms;


// not only connect

namespace radProject
{
    public class PostgresConnect
    {
        public static string Host = "localhost";
        public static string User = "postgres";
        public static string DBname = "company";
        public static string Password = "123";
        public static string Port = "5432";

        public static string connString =
            String.Format( "Server={0};Username={1};Database={2};Port={3};Password={4};SSLMode=Prefer",
            Host,
            User,
            DBname,
            Port,
            Password);

        public static List<expenses> dbGetListExpenses()
        {
            List<expenses> usersList = new List<expenses>();
            using (var conn = new NpgsqlConnection(connString))
            {
                //Console.Out.WriteLine("Opening connection");
                conn.Open();

                using (var command = new NpgsqlCommand("SELECT * FROM expenses", conn))
                {
                    var reader = command.ExecuteReader();
                    while (reader.Read()) { usersList.Add(new expenses(reader.GetInt32(0), reader.GetString(1))); }
                    reader.Close();
                }
            }
            return usersList;
        }
        public static List<departments> dbGetListDepartments()
        {
            List<departments> departmentsList = new List<departments>();
            using (var conn = new NpgsqlConnection(connString))
            {
                //Console.Out.WriteLine("Opening connection");
                conn.Open();

                using (var command = new NpgsqlCommand("SELECT * FROM departments", conn))
                {
                    var reader = command.ExecuteReader();
                    while (reader.Read()) { departmentsList.Add(new departments(reader.GetInt32(0), reader.GetString(1))); }
                    reader.Close();
                }
            }
            return departmentsList;
        }
        public static List<expenseReport> dbGetListExpenseReport()
        {
            List<expenseReport> expenseReportList = new List<expenseReport>();
            using (var conn = new NpgsqlConnection(connString))
            {
                //Console.Out.WriteLine("Opening connection");
                conn.Open();

                using (var command = new NpgsqlCommand("SELECT * FROM expensereport", conn))
                {
                    var reader = command.ExecuteReader();
                    while (reader.Read()) { expenseReportList.Add(new expenseReport(reader.GetInt32(0), reader.GetInt32(1), reader.GetInt32(2), reader.GetDouble(3), reader.GetDateTime(4))); }
                    reader.Close();
                }
            }
            return expenseReportList;
        }

        public static void dbInsertDepartment(string name)
        {
            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                using (var command = new NpgsqlCommand("INSERT INTO departments (name) VALUES ('" + name + "')", conn))
                {
                    command.ExecuteReader();
                }
                conn.Close();
            }
        }
        public static void dbInsertExpense(string name)
        {
            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                using (var command = new NpgsqlCommand("INSERT INTO expenses (name) VALUES ('" + name + "')", conn))
                {
                    command.ExecuteReader();
                }
                conn.Close();
            }
        }
        public static void dbInsertExpenseReport(string id_department, string id_expense, string amount, string date)
        {
            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                using (var command = new NpgsqlCommand("INSERT INTO expenseReport (id_department, id_expense, amount, date) VALUES (" + id_department + ", " + id_expense + ", " + amount + ", '" + date + "')", conn))
                {
                    command.ExecuteReader();
                }
                conn.Close();
            }
        }

        public static void dbUpdateDepartments(int id, string name)
        {
            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                using (var command = new NpgsqlCommand("UPDATE departments " +
                                                        "SET name = '" + name + "' " +
                                                        "WHERE id = " + id, conn))
                {
                    command.ExecuteReader();
                }
                conn.Close();
            }
        }
        public static void dbUpdateExpenses(int id, string name)
        {
            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                using (var command = new NpgsqlCommand("UPDATE expenses " +
                                                        "SET name = '" + name + "'" + " " +
                                                        "WHERE id = " + id, conn))
                {
                    command.ExecuteReader();
                }
                conn.Close();
            }
        }
        public static void dbUpdateExpenseReport(int id, string id_department, string id_expense, string amount, string date)
        {
            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                using (var command = new NpgsqlCommand("UPDATE expenseReport " +
                                                        "SET id_department = " + id_department + ", " + "id_expense = " + id_expense + ", amount = " + amount + ", date = '" + date + "' " +
                                                        "WHERE id = " + id, conn))
                {
                    command.ExecuteReader();
                }
                conn.Close();
            }
        }



        public static List<report> dbGetReport(List<string> expensesID, DateTime dateFrom, DateTime dateTo)
        {
            List<report> report = new List<report>();
            using (var conn = new NpgsqlConnection(connString))
            {
                // debug
                string usersString = "'" + expensesID[0] + "'";
                for (int i = 1; i < expensesID.Count; i++)
                {
                    usersString += ", '" + expensesID[i] + "'";
                }
                MessageBox.Show(usersString);

                foreach (var expense in expensesID)
                {
                    conn.Open();
                    using (var command = new NpgsqlCommand("SELECT expenses.name, expenseReport.amount " +
                                                       "FROM expenses, expenseReport " +
                                                       "WHERE expenses.id = id_expense " +
                                                       "AND expenseReport.id_expense = " + expense +
                                                       "AND DATE(expenseReport.date) > '" + dateFrom.Year + "-" + dateFrom.Month + "-" + dateFrom.Day + "'" +
                                                       "AND DATE(expenseReport.date) < '" + dateTo.Year + "-" + dateTo.Month + "-" + dateTo.Day + "'", conn))
                    {
                        var reader = command.ExecuteReader();
                        string name = "";
                        double sum = 0;
                        while (reader.Read())
                        {
                            name = reader.GetString(0);
                            sum +=reader.GetDouble(1);
                        }
                        reader.Close();
                        report.Add(new report(name, sum));
                    }
                    conn.Close();
                }
            }
            return report;
        }
    }
}