using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportService.Domain
{
    public interface IEmployeeListProvider
    {
        Task<IEnumerable<Employee>> GetEmployeesAsync();
    }
}
