using System;

namespace ADOEmployeePayroll
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Employee Payroll Application using ADO.net");
            EmployeeRepository employeeRepository = new EmployeeRepository();
        }
    }
}