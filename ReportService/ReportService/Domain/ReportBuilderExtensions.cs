using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportService.Domain
{
    public static class ReportBuilderExtensions
    {

        public static async Task<byte[]> GetReportBinaryAsync(this IReportBuilder reportBuilder, int year, int month)
        {
            string reportString = await reportBuilder.GetReportAsync(year, month);
            return Encoding.UTF8.GetBytes(reportString);
        }
    }
}
