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
        IReportBuilder reportBuilder;

        public ReportController(IReportBuilder reportBuilder)
        {
            this.reportBuilder = reportBuilder;
        }

        [HttpGet]
        [Route("{year}/{month}")]
        public async Task<IActionResult> Download(int year, int month)
        {
            byte[] content = await this.reportBuilder.GetReportBinaryAsync(year, month);
            return File(content, "application/octet-stream", "report.txt");
        }
    }
}
