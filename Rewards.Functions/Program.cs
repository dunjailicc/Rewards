using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rewards.Application.Interfaces;
using Rewards.Application.Pagination;
using Rewards.Business.Services;
using Rewards.DataAccess;
using Rewards.DataAccess.Repositories;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.AddAzureClients(clientBuilder =>
        {
            clientBuilder.AddBlobServiceClient("UseDevelopmentStorage=true");
            clientBuilder.AddQueueServiceClient("UseDevelopmentStorage=true");
        });
        services.AddDbContext<RewardsDbContext>();

        services.AddTransient<IRewardRepository, RewardRepository>();
        services.AddTransient<IRewardService, RewardService>();
        services.AddTransient<ICampaignRepository, CampaignRepository>();
        services.AddTransient<ICampaignService, CampaignService>();
        services.AddTransient<IPurchaseReportService, PurchaseReportService>();
        services.AddTransient<IPurchaseRecordRepository, PurchaseRecordRepository>();
        services.AddTransient<IPaginationUtils, PaginationUtils>();
    })
    .Build();

host.Run();
