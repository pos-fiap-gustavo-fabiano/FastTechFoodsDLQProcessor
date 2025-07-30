using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using FastTechFoodsDLQProcessor.Data;
using FastTechFoodsDLQProcessor.Services;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services =>
    {
        // Configure MongoDB
        services.AddSingleton<IMongoClient>(provider =>
        {
            var connectionString = Environment.GetEnvironmentVariable("MongoDBConnectionString") 
                ?? throw new InvalidOperationException("MongoDBConnectionString environment variable is required");
            return new MongoClient(connectionString);
        });
        
        services.AddScoped<ApplicationDbContext>();
        services.AddScoped<DLQMessageService>();
    })
    .Build();

host.Run();
