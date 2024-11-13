using FluentValidation.AspNetCore;
using Rewards.Application.Interfaces;
using Rewards.Application.Pagination;
using Rewards.Business.Services;
using Rewards.Business.Validators;
using Rewards.DataAccess;
using Rewards.DataAccess.Repositories;
using Microsoft.Extensions.Azure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<RewardsDbContext>();

builder.Services.AddControllers();

builder.Services.AddMemoryCache();

builder.Services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CampaignValidator>());


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IRewardRepository, RewardRepository>();
builder.Services.AddTransient<IRewardService, RewardService>();
builder.Services.AddTransient<ICampaignRepository, CampaignRepository>();
builder.Services.AddTransient<ICampaignService, CampaignService>();
builder.Services.AddTransient<IPurchaseReportService, PurchaseReportService>();
builder.Services.AddTransient<IPurchaseRecordRepository, PurchaseRecordRepository>();
builder.Services.AddTransient<IPaginationUtils, PaginationUtils>();
builder.Services.AddAzureClients(clientBuilder =>
{
    clientBuilder.AddBlobServiceClient(builder.Configuration["localstorage:blob"]!, preferMsi: true);
    clientBuilder.AddQueueServiceClient(builder.Configuration["localstorage:queue"]!, preferMsi: true);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
