using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Azure;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Rewards.API.Middlewares;
using Rewards.Application.Interfaces;
using Rewards.Application.Pagination;
using Rewards.Business.Caching;
using Rewards.Business.Services;
using Rewards.Business.Validators;
using Rewards.DataAccess;
using Rewards.DataAccess.Repositories;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<RewardsDbContext>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
        .AddJwtBearer(options =>
        {
            // this config is fine for test project.
            options.Authority = "your_issuer_here";
            options.RequireHttpsMetadata = false;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = "your_issuer_here", // Your issuer
                ValidAudience = "your_audience_here", // Your audience
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("JzdWIiOiIxMjM0NTY3ODkwIiwiYXVkIjoieW91cl9hdWRpZW5jZV9oZXJlIiwiaXNzIjoieW91cl9pc3N1ZXJfaGVyZSIsImNsaWVudElkIjoiZmNjY2IwMTQtMDczNS00ZDAzLTliZjgtYTI3YzBmYmJjZDA2IiwiY2xpZW50TmFtZSI6I")) // Your secret key
            };
            });

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
    // Or to ignore cycles entirely, use:
    // options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});

builder.Services.AddMemoryCache();

builder.Services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CampaignValidator>());


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header, // The token is expected in the Authorization header
        Description = "Please enter 'Bearer' [space] and then your token in the text input below.\n\nExample: \"Bearer 12345abcdef\"",
        Name = "Authorization", // The name of the header
        Type = SecuritySchemeType.ApiKey // Type of the security scheme
    });

    // Add a security requirement to apply the security definition
    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer" // Reference to the security definition created above
                }
            },
            new string[] {} // No specific scopes, just use the Bearer token
        }
    });
});

builder.Services.AddTransient<IRewardRepository, RewardRepository>();
builder.Services.AddTransient<IRewardService, RewardService>();
builder.Services.AddTransient<ICampaignRepository, CampaignRepository>();
builder.Services.AddTransient<ICampaignService, CampaignService>();
builder.Services.AddTransient<IPurchaseReportService, PurchaseReportService>();
builder.Services.AddTransient<IPurchaseRecordRepository, PurchaseRecordRepository>();
builder.Services.AddTransient<IPaginationUtils, PaginationUtils>();
builder.Services.AddTransient<IRewardCache, RewardCache>();
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

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
