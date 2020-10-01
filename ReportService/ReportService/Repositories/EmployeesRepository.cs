using Microsoft.Extensions.Configuration;
using Npgsql;
using ReportService.Domain;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace ReportService.Repositories
{
    public class EmployeesRepository : IEmployeesRepository
    {
        string connectionString;

        public EmployeesRepository(IConfiguration configuration)
        {
            this.connectionString = configuration.GetConnectionString("Employees"); ;
        }

        public async Task<IEnumerable<Employee>> GetEmployeesAsync()
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(this.connectionString))
            {
                string commandString = @"
                    SELECT e.name, e.inn, d.name 
                    FROM emps e 
                    LEFT JOIN deps d on e.departmentid = d.id 
                    WHERE d.active = true
                ";
                using (NpgsqlCommand command = new NpgsqlCommand(commandString, connection))
                {
                    using (DbDataReader reader = await command.ExecuteReaderAsync())
                    {
                        List<Employee> result = new List<Employee>();
                        while (await reader.ReadAsync())
                        {
                            Employee employee = new Employee() { Name = reader.GetString(0), Inn = reader.GetString(1), Department = reader.GetString(2) };
                            result.Add(employee);
                        }
                        return result;
                    }
                }
            }
        }
    }
}
