using EwpApi.Service;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EwpApi.UpdaterService
{
    public class WorkerService : BackgroundService
    {
        private static RegistryClient _client;
        private const int generalDelay = 300000; // 5 dakika

        public WorkerService()
        {
            _client = new RegistryClient();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(generalDelay, stoppingToken);
                await DoUpdateAsync();
            }
        }

        private static Task DoUpdateAsync()
        {
            _client.DownloadCatalog();

            return Task.FromResult("Done");
        }
    }
}
