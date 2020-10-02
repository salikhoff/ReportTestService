using ReportService.Domain;
using ReportService.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportService.UnitTests.Stubs
{
    class EmployeeCodeServiceStub : IEmployeeCodeService
    {
        List<Employee> employees;
        public EmployeeCodeServiceStub(List<Employee> employees)
        {
            this.employees = employees;
        }

        public async Task<string> GetCodeAsync(string inn)
        {
            return await Task.Run(() => this.employees.First(e => e.Inn == inn).BuhCode);
        }
    }
}
