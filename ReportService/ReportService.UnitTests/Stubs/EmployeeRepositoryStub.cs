using ReportService.Domain;
using ReportService.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ReportService.UnitTests.Stubs
{
    class EmployeeRepositoryStub : IEmployeesRepository
    {
        List<Employee> employees;
        public EmployeeRepositoryStub(List<Employee> employees)
        {
            this.employees = employees;
        }
        public async Task<IEnumerable<Employee>> GetEmployeesAsync()
        {
            return await Task.Run(() => this.employees);
        }
    }
}
