using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportService.Services
{
    public interface ISalaryService
    {
        Task<decimal> GetSalaryAsync(string inn, string employeeCode);
    }
}
