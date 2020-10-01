using ReportService.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportService.Repositories
{
    public interface IEmployeesRepository
    {
        Task<IEnumerable<Employee>> GetEmployeesAsync();
    }
}
