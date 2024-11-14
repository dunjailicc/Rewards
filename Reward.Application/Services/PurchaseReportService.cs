using Azure.Storage.Blobs;
using Azure.Storage.Queues;
using Microsoft.AspNetCore.Http;
using Rewards.Application.Pagination;
using Rewards.Business.DTO;
using Rewards.Business.DTO.Filters;
using Rewards.Business.Exceptions;
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

        public async Task<PaginatedResult<PurchaseRecordDto>> GetAsync(PurchaseReportFilter filter)
        {
            var result = await _purchaseReportRepository.GetAsync(
                filter.CustomerId, 
                filter.CampaignId, 
                filter.PageNumber, 
                filter.PageSize);

            return new PaginatedResult<PurchaseRecordDto>
            {
                CurrentPage = result.CurrentPage,
                TotalCount = result.TotalCount,
                TotalPages = result.TotalPages,
                Items = result.Items.Select(x => new PurchaseRecordDto
                {
                    Id = x.Id,
                    CampaignId = x.CampaignId,
                    CustomerId = x.CustomerId,
                    CustomerName = x.CustomerName
                }).ToList()
            };
        }

        public async Task<PurchaseRecordDto> GetByIdAsync(int id)
        {
            var recordFromDb = await _purchaseReportRepository.GetByIdAsync(id);

            if (recordFromDb is null)
            {
                throw new NotFoundException("Purchase record not found."); 
            }

            return new PurchaseRecordDto
            {
                CampaignId = recordFromDb.CampaignId,
                CustomerId = recordFromDb.CustomerId,
                CustomerName = recordFromDb.CustomerName,
                Id = recordFromDb.Id
            };

        }

        public async Task ProcessBatch(List<CreatePurchaseRecordDto> records, int campaignId)
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
            if (file is null || !file.FileName.EndsWith(".csv"))
            {
                throw new InvalidFileFormatException("File must be CSV.");
            }

            var newFileName = Guid.NewGuid().ToString() + ".csv";
            var containerName = "csvreports";
            var blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            await blobContainerClient.CreateIfNotExistsAsync();

            if (file.Length > 0)
            {
                var blobClient = blobContainerClient.GetBlobClient(newFileName);

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
