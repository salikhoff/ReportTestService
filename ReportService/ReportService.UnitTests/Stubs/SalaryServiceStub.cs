using ReportService.Domain;
using ReportService.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportService.UnitTests.Stubs
{
    class SalaryServiceStub : ISalaryService
    {
        List<Employee> employees;
        public SalaryServiceStub(List<Employee> employees)
        {
            this.employees = employees;
        }

        public async Task<decimal> GetSalaryAsync(string inn, string employeeCode)
        {
            return await Task.Run(() => this.employees.First(e => e.Inn == inn && e.BuhCode == employeeCode).Salary);
        }
    }
}
