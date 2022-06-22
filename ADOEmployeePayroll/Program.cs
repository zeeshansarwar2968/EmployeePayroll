using System;

namespace ADOEmployeePayroll
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine("\t\t\t-----------------------------------------------------------");
            Console.WriteLine("\t\t\t   Welcome to Employee Payroll Application using ADO.net");
            Console.WriteLine("\t\t\t-----------------------------------------------------------");
            Console.ResetColor();
            Console.WriteLine("");
            EmployeeRepository employeeRepository = new EmployeeRepository();
            
            //employeeRepository.UpdateSalaryQuery();
            //employeeRepository.UpdateSalary("Sarwar");
            Console.WriteLine("1. Get Employee Data from the available Database");
            Console.WriteLine("2. Add New Employee to the present database");
            Console.WriteLine("3. Update Employee data in the database");
            Console.WriteLine("4. Get details Employee from database using Employee Name");
            Console.ForegroundColor= ConsoleColor.Red;
            Console.WriteLine("Press 0 to Exit");
            Console.ResetColor();
            Console.WriteLine("");
            Console.Write("Enter from the available options to start program execution : ");
            int choice = Convert.ToInt32(Console.ReadLine());
            switch (choice)
            {
                case 1:
                    employeeRepository.GetSqlData();
                    break;
                case 2:
                    AddEmployeeData();
                    employeeRepository.GetSqlData();
                    break;
                case 3:
                    UpdateQuery();
                    employeeRepository.GetSqlData();
                    break;
                case 4:
                    employeeRepository.RetrieveQuery("Fineas");
                    break;
                case 0:
                    return;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Please enter a valid choice from the above options!!");
                    Console.ResetColor();
                    break;
            }
         
        }

        //Function/method to add rows of data to the connected database
        public static void AddEmployeeData()
        {
            EmployeeDataManager employeeDataManager = new EmployeeDataManager();

            employeeDataManager.EmployeeName = "Fineas";
            employeeDataManager.Gender = Convert.ToChar("M");
            employeeDataManager.EmployeePhoneNumber = 7654189765;
            employeeDataManager.EmployeeDepartment = "IT";
            employeeDataManager.Address = "56/1, Bolevyard Street, NY";
            employeeDataManager.BasicPay = 75000;
            employeeDataManager.Deduction = 1500;
            employeeDataManager.TaxablePay = 1000;
            employeeDataManager.IncomeTax = 5000;
            employeeDataManager.NetPay = 70000;
            employeeDataManager.StartDate = Convert.ToDateTime("2020-09-06");

            EmployeeRepository employeeRepository = new EmployeeRepository();

            bool result = employeeRepository.AddEmployee(employeeDataManager);

            if (result == true)
            {
                Console.WriteLine("Data is inserted into database");
            }
            else
            {
                Console.WriteLine("Data is not inserted into database");
            }
        }


        //Function/method to update present data in the connected database
        public static void UpdateQuery()
        {
            EmployeeDataManager employeeDataManager = new EmployeeDataManager();

            employeeDataManager.EmployeeID = 6;
            employeeDataManager.BasicPay = 120000;
            employeeDataManager.EmployeeName = "Fineas";

            EmployeeRepository employeeRepository = new EmployeeRepository();

            bool result = employeeRepository.UpdateSalary(employeeDataManager);

            if (result == true)
            {
                Console.WriteLine("Data is updated in the database");
            }
            else
            {
                Console.WriteLine("Data is not updated in the database");
            }            
        }

    }
}