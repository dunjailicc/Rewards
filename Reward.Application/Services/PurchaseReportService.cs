using Azure.Storage.Blobs;
using Azure.Storage.Queues;
using Microsoft.AspNetCore.Http;
using Rewards.Business.DTO;
using Rewards.DataAccess.Models;
using Rewards.DataAccess.Repositories;
using System.Text;
using System.Text.Json;

namespace Rewards.Business.Services
{
    public class PurchaseReportService : IPurchaseReportService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly QueueServiceClient _queueServiceClient;
        private readonly IPurchaseRecordRepository _purchaseReportRepository;

        public PurchaseReportService(
            BlobServiceClient blobServiceClient, 
            QueueServiceClient queueServiceClient,
            IPurchaseRecordRepository purchaseRecordRepository)
        {
            _blobServiceClient = blobServiceClient;
            _queueServiceClient = queueServiceClient;
            _purchaseReportRepository = purchaseRecordRepository;
        }

        public async Task ProcessBatch(List<PurchaseRecordDto> records, int campaignId)
        {
            await _purchaseReportRepository.AddAsync(records.Select(r => new PurchaseRecord
            {
                CampaignId = campaignId,
                CustomerId = r.CustomerId,
                CustomerName = r.CustomerName
            }).ToList());
            
        }

        public async Task StoreFileAndSendMessageToQueueAsync(int campaignId, IFormFile file)
        {   
            // TODO - check if campaignId exists in db
            if (!file.FileName.EndsWith(".csv"))
            {
                //throw new ArgumentException("File must be a CSV.");
            }

            var newFileName = Guid.NewGuid().ToString() + ".csv";
            var containerName = "csvreports";
            var blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            await blobContainerClient.CreateIfNotExistsAsync();

            if (file.Length > 0)
            {
                // Get a blob client for the specific file
                var blobClient = blobContainerClient.GetBlobClient(newFileName);

                // Upload the file
                using (var stream = file.OpenReadStream())
                {
                    var blobInfo = await blobClient.UploadAsync(stream, overwrite: false); 
                }
            }

            var client = _queueServiceClient.GetQueueClient("csvreports");
            var bytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(new
            {
                CampaignId = campaignId,
                FileName = newFileName
            }));

            await client.SendMessageAsync(Convert.ToBase64String(bytes));
        }

    }
}
