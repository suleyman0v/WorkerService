using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SmartNtsService.Helpers;
using SmartNtsService.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SmartNtsService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                    string url = $"https://catfact.ninja/fact";
                    HttpClient client = new HttpClient();
                    var result = await client.GetAsync(url);
                    var content = await result.Content.ReadAsStringAsync();

                    if (((int)result.StatusCode) == 200 && content != "{}")
                    {
                    }
                    await Task.Delay(300000, stoppingToken);//5 min
                }
                catch (Exception ex)
                {
                    Mail ml = new Mail();
                    ml.SendMail("test@gmail.com", "Error Test Service", ex.ToString());
                    _logger.LogInformation(ex.ToString(), DateTimeOffset.Now);
                    _logger.LogInformation("SmartNtsService exception Data: ", ex.Message);
                    //await StartAsync(stoppingToken);
                }

            }



        }
        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation
            ("Worker service has been stopped at: {0}", DateTime.Now);
            return base.StopAsync(cancellationToken);
        }
        public override void Dispose()
        {
            _logger.LogInformation
            ("Worker service has been disposed at: {0}", DateTime.Now);
            base.Dispose();
        }
    }
}
