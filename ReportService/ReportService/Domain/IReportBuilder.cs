using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportService.Domain
{
    public interface IReportBuilder
    {
        Task<string> GetReportAsync(int year, int month);
    }
}
