using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ReportService.Services
{
    public class SalaryService : ISalaryService
    {
        string serviceUrl;

        public SalaryService(IConfiguration configuration)
        {
            serviceUrl = configuration.GetValue<string>("Services:SalaryService");
        }

        public async Task<decimal> GetSalary(string inn, string employeeCode)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                string empCodeJson = JsonConvert.SerializeObject(new { BuhCode = employeeCode });
                HttpResponseMessage response = await httpClient.PostAsync(serviceUrl + inn, new StringContent(empCodeJson));
                string responseText = await response.Content.ReadAsStringAsync();
                return decimal.Parse(responseText);
            }
        }
    }
}
