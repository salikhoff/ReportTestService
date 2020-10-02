using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReportService.Domain;
using ReportService.UnitTests.Stubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReportService.UnitTests
{
    [TestClass]
    public class EmployeeListProviderTest
    {
        [TestMethod]
        public void TestEmployeeListProvider()
        {
            Employee employee1 = new Employee() { Name = "Имя1", Salary = 1000, Inn = "Inn1", BuhCode = "BC1", Department = "Отдел1" };
            Employee employee2 = new Employee() { Name = "Имя2", Salary = 2000, Inn = "Inn2", BuhCode = "BC2", Department = "Отдел1" };
            Employee employee3 = new Employee() { Name = "Имя3", Salary = 3000, Inn = "Inn3", BuhCode = "BC3", Department = "Отдел2" };
            Employee employee4 = new Employee() { Name = "Имя4", Salary = 4000, Inn = "Inn4", BuhCode = "BC4", Department = null };
            List<Employee> employees = new List<Employee>() { employee1, employee2, employee3, employee4 };

            EmployeeListProvider employeeListProvider = new EmployeeListProvider(new EmployeeRepositoryStub(employees), new EmployeeCodeServiceStub(employees), new SalaryServiceStub(employees));
            IDictionary<string, IEnumerable<Employee>> employeesByDepartment = employeeListProvider.GetEmployeesGroupedAsync().Result;

            Assert.AreEqual(3, employeesByDepartment.Count);

            IEnumerable<Employee> departmentEmployees;
            
            Assert.IsTrue(employeesByDepartment.TryGetValue(employee1.Department, out departmentEmployees));
            Assert.AreEqual(2, departmentEmployees.Count());
            Assert.IsTrue(departmentEmployees.Contains(employee1));
            Assert.IsTrue(departmentEmployees.Contains(employee2));

            Assert.IsTrue(employeesByDepartment.TryGetValue(employee3.Department, out departmentEmployees));
            Assert.AreEqual(1, departmentEmployees.Count());
            Assert.IsTrue(departmentEmployees.Contains(employee3));

            Assert.IsTrue(employeesByDepartment.TryGetValue("Без отдела", out departmentEmployees));
            Assert.AreEqual(1, departmentEmployees.Count());
            Assert.IsTrue(departmentEmployees.Contains(employee4));

        }
    }
}
