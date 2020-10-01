using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportService.Services
{
    public interface IEmployeeCodeService
    {
        Task<string> GetCodeAsync(string inn);
    }
}
