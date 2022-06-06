using CheckBikeTheftAPI.CheckBikeTheftAPI.Core.Interfaces;
using CheckBikeTheftAPI.CheckBikeTheftAPI.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Add repository lifetime
builder.Services.AddScoped<IStolenBikeRepository, StolenBikeRepository>();

builder.Services.AddControllers()
       .AddJsonOptions(
            options =>
            {
                options.JsonSerializerOptions.Converters.Add(
                    new System.Text.Json.Serialization.JsonStringEnumConverter()
                );
            }
        );
// add api versioning to support multiple API versions
builder.Services.AddApiVersioning(
    x =>
    {
        x.DefaultApiVersion = new ApiVersion(1, 0);
        // set the default version when the client has not specified any versions.
        // If we haven't set this flag to true and client hit the API
        // without mentioning the version then UnsupportedApiVersion exception occurs (api-supported-versions is set)
        x.AssumeDefaultVersionWhenUnspecified = true;
        x.ReportApiVersions = true; // return the API version in response header.
    }
);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
// to show default values for enum UseInlineDefinitionsForEnums() 
builder.Services.AddSwaggerGen(options => { options.UseInlineDefinitionsForEnums(); options.EnableAnnotations(); });
builder.Services.AddHttpClient();

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

public partial class Program
{
}