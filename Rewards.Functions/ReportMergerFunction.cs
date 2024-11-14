using Azure.Storage.Blobs;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Rewards.Business.DTO;
using Rewards.Business.Services;
using Rewards.Functions.Models;
using System.Globalization;
using System.Text.Json;

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
            var purchaseRecord = JsonSerializer.Deserialize<Message>(message);

            var blobClient = _blobServiceClient.GetBlobContainerClient("csvreports");
            var blob = blobClient.GetBlobClient(purchaseRecord.FileName);

            using (var stream = await blob.OpenReadAsync())
            {
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = true, 
                };

                using (var reader = new StreamReader(stream))
                using (var csv = new CsvReader(reader, config))
                {

                    var records = new List<CreatePurchaseRecordDto>();
                    int batchSize = 100;

                    while (await csv.ReadAsync())
                    {
                        var record = csv.GetRecord<CreatePurchaseRecordDto>();
                        records.Add(record);

                        if (records.Count == batchSize)
                        {
                            await _purchaseReportService.ProcessBatch(records, purchaseRecord.CampaignId);
                            records.Clear(); 
                        }
                    }

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
