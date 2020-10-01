using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ReportService.Services
{
    public class EmployeeCodeService : IEmployeeCodeService
    {
        string serviceUrl;

        public EmployeeCodeService(IConfiguration configuration)
        {
            this.serviceUrl = configuration.GetValue<string>("Services:EmployeeCodeService");
        }

        public async Task<string> GetCodeAsync(string inn)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                return await httpClient.GetStringAsync(this.serviceUrl + inn);
            }
        }
    }
}
