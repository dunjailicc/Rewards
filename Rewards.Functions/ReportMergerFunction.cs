using Azure.Storage.Blobs;
using CsvHelper.Configuration;
using CsvHelper;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Rewards.Business.Services;
using System.Globalization;
using System.Text.Json;
using Rewards.DataAccess.Models;
using Rewards.Functions.Models;
using Rewards.Business.DTO;

namespace Rewards.Functions
{
    public class ReportMergerFunction
    {
        private readonly ILogger<ReportMergerFunction> _logger;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly IPurchaseReportService _purchaseReportService;

        public ReportMergerFunction(ILogger<ReportMergerFunction> logger, BlobServiceClient blobServiceClient, IPurchaseReportService purchaseReportService)
        {
            _logger = logger;
            _blobServiceClient = blobServiceClient;
            _purchaseReportService = purchaseReportService;
        }

        [Function(nameof(ReportMergerFunction))]
        public async Task Run([QueueTrigger("csvreports", Connection = "localstorage")] string message)
        {
            // Deserijalizujte poruku da dobijete ime fajla
            var purchaseRecord = JsonSerializer.Deserialize<Message>(message);

            // Ovde dodajte logiku za preuzimanje fajla iz Bloba
            var blobClient = _blobServiceClient.GetBlobContainerClient("csvreports");
            var blob = blobClient.GetBlobClient(purchaseRecord.FileName);

            using (var stream = await blob.OpenReadAsync())
            {
                // Configure CsvHelper
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = true, // Set to false if your CSV doesn't have headers
                };

                using (var reader = new StreamReader(stream))
                using (var csv = new CsvReader(reader, config))
                {

                    var records = new List<PurchaseRecordDto>();
                    int batchSize = 100;

                    // Read records one by one and process in batches
                    while (await csv.ReadAsync())
                    {
                        var record = csv.GetRecord<PurchaseRecordDto>();
                        records.Add(record);

                        if (records.Count == batchSize)
                        {
                            // Process the batch of records
                            await _purchaseReportService.ProcessBatch(records, purchaseRecord.CampaignId);
                            records.Clear(); // Clear the list for the next batch
                        }
                    }

                    // Process any remaining records that didn't fill a complete batch
                    if (records.Any())
                    {
                        await _purchaseReportService.ProcessBatch(records, purchaseRecord.CampaignId);
                    }
                }
            }

            _logger.LogInformation($"Processed file: {purchaseRecord.FileName}");
            _logger.LogInformation($"C# Queue trigger function processed: {message}");
        }
    }
}
