using RabbitMQ.Client;
using Serilog;
using Serilog.Sinks.Elasticsearch;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

var configuration = builder.Configuration;
var elasticsearchUrl = configuration["Elasticsearch:Url"];

Log.Logger = new LoggerConfiguration()
    .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elasticsearchUrl))
    {
        AutoRegisterTemplate = true,
        IndexFormat = "logs-{0:yyyy.MM.dd}"
    })
    .CreateLogger();

Log.Information("This is a log message sent to Elasticsearch");

//var rabbitMqHostName = configuration["RabbitMQ:HostName"];

//var factory = new ConnectionFactory() { HostName = rabbitMqHostName };
//using var connection = factory.CreateConnection();
//using var channel = connection.CreateModel();

app.UseAuthorization();

app.MapControllers();

app.Run();
