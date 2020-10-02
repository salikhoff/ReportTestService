using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReportService.Domain;
using ReportService.UnitTests.Stubs;
using System.Collections.Generic;
using System.Text;

namespace ReportService.UnitTests
{
    [TestClass]
    public class ReportBuilderTest
    {
        [TestMethod]
        public void TestReportBuilder()
        {
            List<Employee> employees = new List<Employee>();

            employees.Add(new Employee() { Name = "Имя1", Salary = 1000, Department = "Отдел1" });
            employees.Add(new Employee() { Name = "Имя2", Salary = 2000, Department = "Отдел1" });
            employees.Add(new Employee() { Name = "Имя3", Salary = 3000, Department = "Отдел2" });
            employees.Add(new Employee() { Name = "Имя4", Salary = 4000, Department = null });

            ReportBuilder reportBuilder = new ReportBuilder(new EmployeeListProviderStub(employees));

            string reportString = reportBuilder.GetReportAsync(2020, 01).Result;
            string expectedReportString =
                @"Январь 2020
--------------------------------------------
Отдел1
Имя1         1000р
Имя2         2000р
Всего по отделу         3000р
--------------------------------------------
Отдел2
Имя3         3000р
Всего по отделу         3000р
--------------------------------------------
Без отдела
Имя4         4000р
Всего по отделу         4000р
--------------------------------------------
Всего по предприятию         10000р";

            Assert.AreEqual(expectedReportString, reportString);

            byte[] reportBinary = reportBuilder.GetReportBinaryAsync(2020, 01).Result;
            byte[] expectedReportBinary = Encoding.UTF8.GetBytes(expectedReportString);
            CollectionAssert.AreEqual(expectedReportBinary, reportBinary);
        }
    }
}
