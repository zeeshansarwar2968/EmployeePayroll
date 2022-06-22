using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace ADOEmployeePayroll
{
    class EmployeeRepository
    {
        
        //Give path for Database Connection
        public static string connection = @"data source=LAPTOP-ICIBRBKA\MSSQLSERVER01; database=payroll_service; integrated security=true;";
        //Represents a connection to Sql Server Database
        SqlConnection sqlConnection = new SqlConnection(connection);

        //Create Object for EmployeeData Repository
        EmployeeDataManager employeeDataManager = new EmployeeDataManager();
        public void GetSqlData()
        {
            //Open Connection
            sqlConnection.Open();   
            string query = "select * from employee_payroll";
            //Pass query to TSql
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            //sqldatareader provides a way of reading forward-only stream of rows of data
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

            //Check if sqlDataReader has Rows
            if (sqlDataReader.HasRows)
            {
                //Read each row
                while (sqlDataReader.Read())
                {
                    //Read data SqlDataReader and store 
                    employeeDataManager.EmployeeID = Convert.ToInt32(sqlDataReader["id"]);
                    employeeDataManager.EmployeeName = sqlDataReader["name"].ToString();
                    employeeDataManager.BasicPay = Convert.ToDouble(sqlDataReader["BasicPay"]);
                    employeeDataManager.Deduction = Convert.ToDouble(sqlDataReader["Deduction"]);
                    employeeDataManager.IncomeTax = Convert.ToDouble(sqlDataReader["IncomeTax"]);
                    employeeDataManager.TaxablePay = Convert.ToDouble(sqlDataReader["TaxablePay"]);
                    employeeDataManager.NetPay = Convert.ToDouble(sqlDataReader["NetPay"]);
                    employeeDataManager.Gender = Convert.ToChar(sqlDataReader["Gender"]);
                    employeeDataManager.EmployeePhoneNumber = Convert.ToInt64(sqlDataReader["EmployeePhoneNumber"]);
                    employeeDataManager.EmployeeDepartment = sqlDataReader["EmployeeDepartment"].ToString();
                    employeeDataManager.Address = sqlDataReader["Address"].ToString();
                    employeeDataManager.StartDate = Convert.ToDateTime(sqlDataReader["startDate"]);

                    //Display Data
                    Console.WriteLine("\nEmployee ID: {0} \t Employee Name: {1} \nBasic Pay: {2} \t Deduction: {3} \t Income Tax: {4} \t Taxable Pay: {5} \t NetPay: {6} \nGender: {7} \t PhoneNumber: {8} \t Department: {9} \t Address: {10}", employeeDataManager.EmployeeID, employeeDataManager.EmployeeName, employeeDataManager.BasicPay, employeeDataManager.Deduction, employeeDataManager.IncomeTax, employeeDataManager.TaxablePay, employeeDataManager.NetPay, employeeDataManager.Gender, employeeDataManager.EmployeePhoneNumber, employeeDataManager.EmployeeDepartment, employeeDataManager.Address);
                }
                //Close sqlDataReader Connection
                sqlDataReader.Close();
            }
            //Close the Connection
            sqlConnection.Close();
        }

        //Logic to update the salary to 3000000 as per 3rd UseCase
        public int UpdateSalaryQuery()
        {
            //Open Connection
            sqlConnection.Open();
            string query = "update employee_payroll set BasicPay=3000000 where name= 'John'";
            //Pass query to TSQL
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            int result = sqlCommand.ExecuteNonQuery();
            if (result != 0)
            {
                Console.WriteLine("Updated!");
            }
            else
            {
                Console.WriteLine("Not Updated!");
            }
            //Close Connection
            sqlConnection.Close();
            GetSqlData();
            return result;
        }

        public bool AddEmployee(EmployeeDataManager employeeDataManager)
        {
            try
            {
                using (sqlConnection)
                {
                    SqlCommand sqlCommand = new SqlCommand("spAddEmployee", sqlConnection);
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@name", employeeDataManager.EmployeeName);
                    sqlCommand.Parameters.AddWithValue("@PhoneNumber", employeeDataManager.EmployeePhoneNumber);
                    sqlCommand.Parameters.AddWithValue("@Address", employeeDataManager.Address);
                    sqlCommand.Parameters.AddWithValue("@Department", employeeDataManager.EmployeeDepartment);
                    sqlCommand.Parameters.AddWithValue("@Gender", employeeDataManager.Gender);
                    sqlCommand.Parameters.AddWithValue("@BasicPay", employeeDataManager.BasicPay);
                    sqlCommand.Parameters.AddWithValue("@Deductions", employeeDataManager.Deduction);
                    sqlCommand.Parameters.AddWithValue("@TaxablePay", employeeDataManager.TaxablePay);
                    sqlCommand.Parameters.AddWithValue("@Tax", employeeDataManager.IncomeTax);
                    sqlCommand.Parameters.AddWithValue("@NetPay", employeeDataManager.NetPay);
                    sqlCommand.Parameters.AddWithValue("@StartDate", DateTime.Now);

                    //Opens connection thread to the database
                    sqlConnection.Open();

                    //Executing the queries, here we are executing the stored procedure
                    var result = sqlCommand.ExecuteNonQuery();

                    //Closes the connection to database
                    sqlConnection.Close();

                    if (result != 0)
                    {
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                sqlConnection.Close();
            }
        }

        public bool UpdateSalary(EmployeeDataManager employeeDataManager)
        {
            int result = 0;
            try
            {
                using (sqlConnection)
                {
                    //Give stored Procedure
                    SqlCommand sqlCommand = new SqlCommand("dbo.spUpdateSalaryF", this.sqlConnection);
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@salary", employeeDataManager.BasicPay);
                    sqlCommand.Parameters.AddWithValue("@name", employeeDataManager.EmployeeName);
                    sqlCommand.Parameters.AddWithValue("@EmpId", employeeDataManager.EmployeeID);
                    //Open Connection
                    sqlConnection.Open();
                    //Return Number of Rows affected
                    result = sqlCommand.ExecuteNonQuery();
                    if (result != 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }
        public int RetrieveQuery(string name)
        {

            int result = 0;
            try
            {
                using (sqlConnection)
                {
                    //Give stored Procedure
                    SqlCommand sqlCommand = new SqlCommand("dbo.spRetrieveDataUsingName", this.sqlConnection);
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@name", name);
                    //Open Connection
                    sqlConnection.Open();
                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    //Check if swlDataReader has Rows
                    if (sqlDataReader.HasRows)
                    {
                        //Read each row
                        while (sqlDataReader.Read())
                        {
                            result++;
                            //Read data SqlDataReader and store 
                            employeeDataManager.EmployeeID = Convert.ToInt32(sqlDataReader["id"]);
                            employeeDataManager.EmployeeName = sqlDataReader["name"].ToString();
                            employeeDataManager.BasicPay = Convert.ToDouble(sqlDataReader["BasicPay"]);
                            employeeDataManager.Deduction = Convert.ToDouble(sqlDataReader["Deduction"]);
                            employeeDataManager.IncomeTax = Convert.ToDouble(sqlDataReader["IncomeTax"]);
                            employeeDataManager.TaxablePay = Convert.ToDouble(sqlDataReader["TaxablePay"]);
                            employeeDataManager.NetPay = Convert.ToDouble(sqlDataReader["NetPay"]);
                            employeeDataManager.Gender = Convert.ToChar(sqlDataReader["Gender"]);
                            employeeDataManager.EmployeePhoneNumber = Convert.ToInt64(sqlDataReader["EmployeePhoneNumber"]);
                            employeeDataManager.EmployeeDepartment = sqlDataReader["EmployeeDepartment"].ToString();
                            employeeDataManager.Address = sqlDataReader["Address"].ToString();
                            employeeDataManager.StartDate = Convert.ToDateTime(sqlDataReader["StartDate"]);

                            //Display Data
                            Console.WriteLine("\nEmployee ID: {0} \t Employee Name: {1} \nBasic Pay: {2} \t Deduction: {3} \t Income Tax: {4} \t Taxable Pay: {5} \t NetPay: {6} \nGender: {7} \t PhoneNumber: {8} \t Department: {9} \t Address: {10}", employeeDataManager.EmployeeID, employeeDataManager.EmployeeName, employeeDataManager.BasicPay, employeeDataManager.Deduction, employeeDataManager.IncomeTax, employeeDataManager.TaxablePay, employeeDataManager.NetPay, employeeDataManager.Gender, employeeDataManager.EmployeePhoneNumber, employeeDataManager.EmployeeDepartment, employeeDataManager.Address);
                        }
                        //Close sqlDataReader Connection
                        sqlDataReader.Close();
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            sqlConnection.Close();
            return result;
        }
    }
}
