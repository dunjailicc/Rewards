using Rewards.Application.Interfaces;
using Rewards.Application.Pagination;
using Rewards.Business.Services;
using Rewards.DataAccess;
using Rewards.DataAccess.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<RewardsDbContext>();

builder.Services.AddControllers();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IRewardRepository, RewardRepository>();
builder.Services.AddTransient<IRewardService, RewardService>();
builder.Services.AddTransient<IPaginationUtils, PaginationUtils>();

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
