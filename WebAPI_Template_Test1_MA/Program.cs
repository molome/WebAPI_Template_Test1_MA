using Azure.Data.Tables;
using Azure.Identity;
using Azure.Storage.Queues;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using WebAPI_Template_Test1_MA.DbContexts;
using WebAPI_Template_Test1_MA.Services.BlobStorage;
using WebAPI_Template_Test1_MA.Services.QueueStorage;
using WebAPI_Template_Test1_MA.Services.TableStorage;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi


#region Start - Other Options to register storage services globally
var azureStorageConnectionString = builder.Configuration.GetConnectionString("AzureStorageAccountConnection");
var queueName = builder.Configuration["StorageAccount:QueueName"];
var tableName = builder.Configuration["StorageAccount:TableName"];

builder.Services.AddAzureClients(builder =>
{
    builder.AddBlobServiceClient(azureStorageConnectionString);
    builder.AddQueueServiceClient(azureStorageConnectionString).ConfigureOptions(c =>
    {
        c.MessageEncoding = QueueMessageEncoding.Base64;
    });
    builder.AddTableServiceClient(azureStorageConnectionString);
});


builder.Services.AddAzureClients(builder =>
{
    builder.AddClient<QueueClient, QueueClientOptions>((_, _, _) =>
    {
        return new QueueClient(azureStorageConnectionString, queueName, new QueueClientOptions()
        {
            MessageEncoding = QueueMessageEncoding.Base64
        });
    });

    builder.AddClient<TableClient, TableClientOptions>((_, _, _) =>
    {
        return new TableClient(azureStorageConnectionString, tableName);
    });

});

#endregion End - Other Options to register storage services globally

builder.Services.AddScoped<ITableStorageService, TableStorageService>();
builder.Services.AddScoped<IBlobStorageService,BlobStorageService>();
builder.Services.AddScoped<IQueueStorageService, QueueStorageService>();

builder.Services.AddApplicationInsightsTelemetry(new Microsoft.ApplicationInsights.AspNetCore.Extensions.ApplicationInsightsServiceOptions
{
    ConnectionString = builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]
});

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("AzureSqlConnection")));

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
     
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
