using ReportService.Repositories;
using ReportService.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportService.Domain
{
    public class EmployeeListProvider : IEmployeeListProvider
    {
        IEmployeesRepository employeesRepository;
        IEmployeeCodeService employeeCodeService;
        ISalaryService salaryService;

        public EmployeeListProvider(IEmployeesRepository employeesRepository, IEmployeeCodeService employeeCodeService, ISalaryService salaryService)
        {
            this.employeesRepository = employeesRepository;
            this.employeeCodeService = employeeCodeService;
            this.salaryService = salaryService;
        }

        public async Task<IEnumerable<Employee>> GetEmployeesAsync()
        {
            IEnumerable<Employee> employees = await this.employeesRepository.GetEmployeesAsync();
            await Task.WhenAll(employees.Select(e => FillEmployeeInfoAsync(e)));
            return employees;
        }

        private async Task FillEmployeeInfoAsync(Employee employee)
        {
            employee.BuhCode = await this.employeeCodeService.GetCodeAsync(employee.Inn);
            employee.Salary = Convert.ToInt32(await this.salaryService.GetSalaryAsync(employee.Inn, employee.BuhCode));
        }
    }
}
