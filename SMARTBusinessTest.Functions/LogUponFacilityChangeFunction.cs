using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Azure.Storage.Queues;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Sql;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Configuration;
using Newtonsoft.Json;


namespace SMARTBusinessTest.Functions
{
    public class LogUponFacilityChangeFunction
    {
        private QueueClient _queueClient;
        public LogUponFacilityChangeFunction(IConfiguration configuration)
        {
            var conf = configuration["AzureWebJobsStorage"];
            _queueClient = new QueueClient(conf, "facilitychangequeue");
        }
        
        [FunctionName("LogUponFacilityChangeFunction")]
        public async Task Run(
                [SqlTrigger("dbo.Facility", "ConnectionStrings:DatabaseConnection")] IReadOnlyList<SqlChange<Facility>> changes,
                ILogger log)
        {
            var message = ("SQL Changes: " + JsonConvert.SerializeObject(changes));
            log.LogInformation(message);
            await _queueClient.SendMessageAsync(message);
        }
    }

    public class Facility
    {
        public string Facility_Id { get; set; }
        public string Facility_Code { get; set; }
        public string Facility_Name { get; set; }
        public int Area { get; set; }
    }
}
