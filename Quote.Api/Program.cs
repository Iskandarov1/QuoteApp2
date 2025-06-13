using System.IO;
using Quote.Api.Contracts;
using Quote.Api.Helpers;
using Quote.Application;
using Quote.Domain.Core.Localizations;
using Quote.Infrastructure;
using Quote.Persistence;
using Quote.Application.Resources;
using Quote.Domain.Repositories;
using Quote.Services.BackgroundTasks;
using Serilog;

var builder = WebApplication.CreateBuilder(args);


var formsPath = Path.Combine(builder.Environment.ContentRootPath, "Forms");
Directory.CreateDirectory(formsPath);


builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File(Path.Combine(formsPath, "application.log"),
        rollingInterval: RollingInterval.Day,
        fileSizeLimitBytes: 10 * 1024 * 1024,
        retainedFileCountLimit: 31,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplication();
builder.Services.AddInfrastructure();
builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddBackgroundTasks(builder.Configuration);
builder.Services.AddScoped<ISharedViewLocalizer, ApplicationSharedViewLocalizer>();
builder.Services.AddQuoteLocalization();
builder.Services.AddSingleton<IUniqueFileStorage, UniqueFileStorage>();



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();