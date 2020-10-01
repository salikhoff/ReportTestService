using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportService.Domain
{
    public class ReportBuilder : IReportBuilder
    {
        const string Delimiter = "--------------------------------------------";
        const string Tab = "         ";

        IEmployeeListProvider employeeListProvider;

        public ReportBuilder(IEmployeeListProvider employeeListProvider)
        {
            this.employeeListProvider = employeeListProvider;
        }

        public async Task<string> GetReportAsync(int year, int month)
        {
            IDictionary<string, IEnumerable<Employee>> employeesByDepartments = await this.employeeListProvider.GetEmployeesGroupedAsync();

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(new DateTime(year, month, 1).ToString("MMMMMM", CultureInfo.GetCultureInfo("ru-ru"))).Append(' ').Append(year).AppendLine();

            int totalSalary = 0;
            foreach (KeyValuePair<string, IEnumerable<Employee>> department in employeesByDepartments)
            {
                int departmentSalary = 0;
                stringBuilder.AppendLine(Delimiter);
                stringBuilder.AppendLine(department.Key);
                foreach(Employee employee in department.Value)
                {
                    stringBuilder.Append(employee.Name).Append(Tab).Append(employee.Salary).AppendLine("р");
                    departmentSalary += employee.Salary;
                }
                totalSalary += departmentSalary;
                stringBuilder.Append("Всего по отделу").Append(Tab).Append(departmentSalary).AppendLine("р");
            }
            stringBuilder.AppendLine(Delimiter);
            stringBuilder.Append("Всего по предприятию").Append(Tab).Append(totalSalary).Append("р");

            return stringBuilder.ToString();
        }
    }
}
