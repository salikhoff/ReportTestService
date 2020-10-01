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
        IConfiguration configuration;
        IEmployeeCodeService employeeCodeService;
        ISalaryService salaryService;

        public ReportController(IConfiguration configuration, IEmployeeCodeService employeeCodeService, ISalaryService salaryService)
        {
            this.configuration = configuration;
            this.employeeCodeService = employeeCodeService;
            this.salaryService = salaryService;
        }

        [HttpGet]
        [Route("{year}/{month}")]
        public IActionResult Download(int year, int month)
        {
            var actions = new List<(Action<Employee, Report>, Employee)>();
            var report = new Report()
            {
                S = new DateTime(year, month, 1).ToString("MMMMMM", CultureInfo.CurrentCulture)
            };
            var connString = this.configuration.GetConnectionString("Employees");

            var conn = new NpgsqlConnection(connString);
            conn.Open();
            var cmd = new NpgsqlCommand("SELECT d.name from deps d where d.active = true", conn);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                List<Employee> emplist = new List<Employee>();
                var depName = reader.GetString(0);
                var conn1 = new NpgsqlConnection(connString);
                conn1.Open();
                var cmd1 = new NpgsqlCommand("SELECT e.name, e.inn, d.name from emps e left join deps d on e.departmentid = d.id", conn1);
                var reader1 = cmd1.ExecuteReader();
                while (reader1.Read())
                {
                    var emp = new Employee() { Name = reader1.GetString(0), Inn = reader1.GetString(1), Department = reader1.GetString(2) };
                    emp.BuhCode = this.employeeCodeService.GetCode(emp.Inn).Result;
                    emp.Salary = Convert.ToInt32(this.salaryService.GetSalary(emp.Inn, emp.BuhCode).Result);
                    if (emp.Department != depName)
                        continue;
                    emplist.Add(emp);
                }

                actions.Add((new ReportFormatter(null).NL, new Employee()));
                actions.Add((new ReportFormatter(null).WL, new Employee()));
                actions.Add((new ReportFormatter(null).NL, new Employee()));
                actions.Add((new ReportFormatter(null).WD, new Employee() { Department = depName }));
                for (int i = 1; i < emplist.Count(); i++)
                {
                    actions.Add((new ReportFormatter(emplist[i]).NL, emplist[i]));
                    actions.Add((new ReportFormatter(emplist[i]).WE, emplist[i]));
                    actions.Add((new ReportFormatter(emplist[i]).WT, emplist[i]));
                    actions.Add((new ReportFormatter(emplist[i]).WS, emplist[i]));
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
