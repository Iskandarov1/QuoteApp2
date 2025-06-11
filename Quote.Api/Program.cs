using Quote.Api.Helpers;
using Quote.Application;
using Quote.Domain.Core.Localizations;
using Quote.Infrastructure;
using Quote.Persistence;
using Quote.Application.Resources;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplication();
builder.Services.AddInfrastructure();
builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddScoped<ISharedViewLocalizer, ApplicationSharedViewLocalizer>();
builder.Services.AddQuoteLocalization();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();