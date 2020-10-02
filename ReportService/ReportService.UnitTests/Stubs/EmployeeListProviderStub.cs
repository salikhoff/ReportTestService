using ReportService.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ReportService.UnitTests.Stubs
{
    class EmployeeListProviderStub : IEmployeeListProvider
    {
        List<Employee> employees;
        public EmployeeListProviderStub(List<Employee> employees)
        {
            this.employees = employees;
        }
        public async Task<IEnumerable<Employee>> GetEmployeesAsync()
        {
            return await Task.Run(() => this.employees);
        }
    }
}
