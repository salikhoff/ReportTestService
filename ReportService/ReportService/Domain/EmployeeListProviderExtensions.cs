using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportService.Domain
{
    public static class EmployeeListProviderExtensions
    {
        public static async Task<IDictionary<string, IEnumerable<Employee>>> GetEmployeesGroupedAsync(this IEmployeeListProvider employeeListProvider)
        {
            IEnumerable<Employee> employees = await employeeListProvider.GetEmployeesAsync();

            Dictionary<string, IEnumerable<Employee>> employeesByDepartments = new Dictionary<string, IEnumerable<Employee>>();
            foreach (Employee employee in employees)
            {
                string departmentName = employee.Department ?? "Без отдела";
                IEnumerable<Employee> list;
                if (!employeesByDepartments.TryGetValue(departmentName, out list))
                {
                    list = new List<Employee>();
                    employeesByDepartments.Add(departmentName, list);
                }
                ((List<Employee>)list).Add(employee);
            }
            return employeesByDepartments;
        }
    }
}
