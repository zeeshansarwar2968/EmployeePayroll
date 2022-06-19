
--UC-1 creating database
create database payroll_service;
use payroll_service;


--UC-2 creating Table
create Table employee_payroll
(
id int identity(1,1) primary key,
name varchar(200) not null,
salary float,
startDate date
)


--UC-3 Insert values in Table
Insert into employee_payroll(name,salary,startDate) values
('Zeeshan',20000,'2022-03-12'),
('Sarwar',25000,'2022-04-18'),
('John',10000,'2022-05-13'),
('Amelia',30000,'2022-06-09');
select * from employee_payroll;


--UC-4 Retrieve All data--
select * from employee_payroll;

------- UC 5: Select Query using Cast() an GetDate() -------
select salary from employee_payroll where name='John';
select salary from employee_payroll where startDate BETWEEN Cast('2022-05-13' as Date) and GetDate();

------- UC 6: Add Gender Column and Update Table Values -------
Alter table employee_payroll add Gender char(1);

Update employee_payroll set Gender='M' where name='Zeeshan' or name='Sarwar' or name='John';
Update employee_payroll set Gender='F' where name='Amelia';

------- UC 7: Use Aggregate Functions and Group by Gender -------

select Sum(salary) as "TotalSalary",Gender from employee_payroll group by Gender;
select Avg(salary) as "AverageSalary",Gender from employee_payroll group by Gender;
select Min(salary) as "MinimumSalary",Gender from employee_payroll group by Gender;
select Max(salary) as "MaximumSalary",Gender from employee_payroll group by Gender;
select count(salary) as "CountSalary",Gender from employee_payroll group by Gender;

------- UC 8: Add column department,PhoneNumber and Address -------
Alter table employee_payroll
add EmployeePhoneNumber BigInt,EmployeeDepartment varchar(200) not null default 'General', Address varchar(200) default 'Not Provided';

Update employee_payroll set EmployeePhoneNumber='7456187321',EmployeeDepartment='IT',Address='Bangalore,Karnataka' where name='Amelia';

Update employee_payroll set EmployeePhoneNumber='00032143567',EmployeeDepartment='Management',Address='Arizona,US' where name ='Zeeshan';

Update employee_payroll set EmployeePhoneNumber='7453457321',EmployeeDepartment='HR',Address='Chennai,TN' where name ='John';

Update employee_payroll set EmployeePhoneNumber='8453417321',EmployeeDepartment='Marketing',Address='Bareilly,UP' where name ='Sarwar';

------- UC 9: Rename Salary to Basic Pay and Add Deduction,Taxable pay, Income Pay , Netpay -------
select * from employee_payroll;

EXEC sp_RENAME 'employee_payroll.salary' , 'BasicPay'
Alter table employee_payroll add Deduction float,TaxablePay float, IncomeTax float,NetPay float;

Update employee_payroll set Deduction=1000 where Gender='F';
Update employee_payroll set Deduction=2000 where Gender='M';
Update employee_payroll set NetPay=(BasicPay - Deduction);
Update employee_payroll set TaxablePay=0,IncomeTax=0;
select * from employee_payroll;

------- UC 10: Adding another Value for Rujula in Editing Department -------

Insert into employee_payroll(name,BasicPay,StartDate,Address,EmployeePhoneNumber,EmployeeDepartment) values ('Amelia',32000,'2022-06-09','Bangalore,Karnataka','7456187321','Marketing');
select * from employee_payroll;


------- UC 11: Implement the ER Diagram into Payroll Service DB -------

--Create Table for Company
Create Table Company
(CompanyID int identity(1,1) primary key,
CompanyName varchar(100))
--Insert Values in Company
Insert into Company values ('Softech'),('Neotech')
Select * from Company

--Create Employee Table
--drop table employee_payroll
create table Employee
(EmployeeID int identity(1,1) primary key,
CompanyIdentity int,
EmployeeName varchar(200),
EmployeePhoneNumber bigInt,
EmployeeAddress varchar(200),
StartDate date,
Gender char,
Foreign key (CompanyIdentity) references Company(CompanyID)
)
--Insert Values in Employee
insert into Employee values
(1,'Zeeshan Sarwar',9842905050,'5298 Wild Indigo, Georgia,340002','2018-03-28','M'),
(2,'Amelia Bones',9842905550,'Constitution Ave Fairfield, California(CA), 94533','2017-04-22','F'),
(1,'John Doe',7812905050,'Bernard Shaw, Georgia,132001 ','2015-08-22','M'),
(2,'Nitish Kumar',7812905000,'Bernard Shaw, PB Marg Bareilly','2016-08-29','M')

Select * from Employee

--Create Payroll Table
create table PayrollCalculate
(BasicPay float,
Deductions float,
TaxablePay float,
IncomeTax float,
NetPay float,
EmployeeIdentity int,
Foreign key (EmployeeIdentity) references Employee(EmployeeID)
)
--Insert Values in Payroll Table
insert into PayrollCalculate(BasicPay,Deductions,IncomeTax,EmployeeIdentity) values 
(4000000,1000000,20000,1),
(4500000,200000,4000,2),
(6000000,10000,5000,3),
(9000000,399994,6784,4)

select * from PayrollCalculate
--Update Derived attribute values 
update PayrollCalculate set TaxablePay=BasicPay-Deductions

update PayrollCalculate set NetPay=TaxablePay-IncomeTax

select * from PayrollCalculate


--Create Department Table
create table Department
(
DepartmentId int identity(1,1) primary key,
DepartName varchar(100)
)
--Insert Values in Department Table
insert into Department values
('Marketing'),
('Sales'),
('IT')

select * from Department

--Create table EmployeeDepartment
create table EmployeeDepartment
(
DepartmentIdentity int ,
EmployeeIdentity int,
Foreign key (EmployeeIdentity) references Employee(EmployeeID),
Foreign key (DepartmentIdentity) references Department(DepartmentID)
)

--Insert Values in EmployeeDepartment
insert into EmployeeDepartment values
(3,1),
(2,2),
(1,3),
(3,4)

select * from EmployeeDepartment


------- UC 12: Ensure all retrieve queries done especially in UC 4, UC 5 and UC 7 are working with new table structure -------

--UC 4: Retrieve all Data
SELECT CompanyID,CompanyName,EmployeeID,EmployeeName,EmployeeAddress,EmployeePhoneNumber,StartDate,Gender,BasicPay,Deductions,TaxablePay,IncomeTax,NetPay,DepartName
FROM Company
INNER JOIN Employee ON Company.CompanyID = Employee.CompanyIdentity
INNER JOIN PayrollCalculate on PayrollCalculate.EmployeeIdentity=Employee.EmployeeID
INNER JOIN EmployeeDepartment on Employee.EmployeeID=EmployeeDepartment.EmployeeIdentity
INNER JOIN Department on Department.DepartmentId=EmployeeDepartment.DepartmentIdentity

--UC 5: Select Query using Cast() an GetDate()
SELECT CompanyID,CompanyName,EmployeeID,EmployeeName,BasicPay,Deductions,TaxablePay,IncomeTax,NetPay
FROM Company
INNER JOIN Employee ON Company.CompanyID = Employee.CompanyIdentity and StartDate BETWEEN Cast('2012-11-12' as Date) and GetDate()
INNER JOIN PayrollCalculate on PayrollCalculate.EmployeeIdentity=Employee.EmployeeID
--Retrieve query based on Name
SELECT CompanyID,CompanyName,EmployeeID,EmployeeName,BasicPay,Deductions,TaxablePay,IncomeTax,NetPay
FROM Company
INNER JOIN Employee ON Company.CompanyID = Employee.CompanyIdentity and Employee.EmployeeName='Zeeshan Sarwar'
INNER JOIN PayrollCalculate on PayrollCalculate.EmployeeIdentity=Employee.EmployeeID

--UC 7: Use Aggregate Functions and Group by Gender

select Sum(BasicPay) as "TotalSalary",Gender 
from Employee
INNER JOIN PayrollCalculate on PayrollCalculate.EmployeeIdentity=Employee.EmployeeID group by Gender;

select Avg(BasicPay) as "AverageSalary",Gender 
from Employee
INNER JOIN PayrollCalculate on PayrollCalculate.EmployeeIdentity=Employee.EmployeeID group by Gender;

select Min(BasicPay) as "MinimumSalary",Gender 
from Employee
INNER JOIN PayrollCalculate on PayrollCalculate.EmployeeIdentity=Employee.EmployeeID group by Gender;

select Max(BasicPay)  as "MaximumSalary",Gender 
from Employee
INNER JOIN PayrollCalculate on PayrollCalculate.EmployeeIdentity=Employee.EmployeeID group by Gender;

select Count(BasicPay) as "CountSalary",Gender 
from Employee
INNER JOIN PayrollCalculate on PayrollCalculate.EmployeeIdentity=Employee.EmployeeID group by Gender;

