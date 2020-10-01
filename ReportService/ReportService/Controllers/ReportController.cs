using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Npgsql;
using ReportService.Domain;
using ReportService.Services;

namespace ReportService.Controllers
{
    [Route("api/[controller]")]
    public class ReportController : Controller
    {
        IEmployeeListProvider employeeListProvider;

        public ReportController(IEmployeeListProvider employeeListProvider)
        {
            this.employeeListProvider = employeeListProvider;
        }

        [HttpGet]
        [Route("{year}/{month}")]
        public async Task<IActionResult> Download(int year, int month)
        {
            var actions = new List<(Action<Employee, Report>, Employee)>();
            var report = new Report()
            {
                S = new DateTime(year, month, 1).ToString("MMMMMM", CultureInfo.CurrentCulture)
            };

            IEnumerable<Employee> employees = await this.employeeListProvider.GetEmployeesAsync();

            Dictionary<string, List<Employee>> employeesByDepartments = new Dictionary<string, List<Employee>>();
            foreach (Employee employee in employees)
            {
                string departmentName = employee.Department ?? "Без отдела";
                List<Employee> list;
                if (!employeesByDepartments.TryGetValue(departmentName, out list))
                {
                    list = new List<Employee>();
                    employeesByDepartments.Add(departmentName, list);
                }
                list.Add(employee);
            }

            foreach(KeyValuePair<string, List<Employee>> department in employeesByDepartments)
            {
                actions.Add((new ReportFormatter(null).NL, new Employee()));
                actions.Add((new ReportFormatter(null).WL, new Employee()));
                actions.Add((new ReportFormatter(null).NL, new Employee()));
                actions.Add((new ReportFormatter(null).WD, new Employee() { Department = department.Key }));
                for (int i = 1; i < department.Value.Count(); i++)
                {
                    actions.Add((new ReportFormatter(department.Value[i]).NL, department.Value[i]));
                    actions.Add((new ReportFormatter(department.Value[i]).WE, department.Value[i]));
                    actions.Add((new ReportFormatter(department.Value[i]).WT, department.Value[i]));
                    actions.Add((new ReportFormatter(department.Value[i]).WS, department.Value[i]));
                }
            }
            actions.Add((new ReportFormatter(null).NL, null));
            actions.Add((new ReportFormatter(null).WL, null));

            foreach (var act in actions)
            {
                act.Item1(act.Item2, report);
            }
            report.Save();
            var file = System.IO.File.ReadAllBytes("D:\\report.txt");
            var response = File(file, "application/octet-stream", "report.txt");
            return response;
        }
    }
}
