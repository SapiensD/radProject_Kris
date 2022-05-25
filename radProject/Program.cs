using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;
namespace radProject
{
    public class expenses
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public expenses(int Id, string Name)
        {   
            this.Id = Id;
            this.Name = Name;
        }
        public expenses() { }
    }
    public class report
    {
        public string expenseName { get; set; }
        public double summ { get; set; }
        public report(string expenseName, double summ) 
        { 
            this.summ = summ;
            this.expenseName = expenseName;
        }
    }
    public class expenseReport
    {
        public int id { get; set; }
        public int id_department { get; set; }
        public int id_expense { get; set; }
        public double amount { get; set; }
        public DateTime date { get; set; }
        public expenseReport (int id, int id_department, int id_expense, double amount, DateTime date)
        {
            this.id = id;
            this.date = date;
            this.id_department = id_department;
            this.id_expense = id_expense;
            this.amount = amount;
        }
    }
    public class departments
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public departments(int Id, string Name)
        {
            this.Id = Id;
            this.Name = Name;
        }
    }
    internal static class Program
    {     
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
