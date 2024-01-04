using HotelBookingSystem.Api;
using HotelBookingSystem.Application;
using HotelBookingSystem.Infrastructure.Persistence;
using Serilog;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddProblemDetails()
                .AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(setup =>
{
    var actionMethodsXmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var actionMethodsXmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, actionMethodsXmlCommentsFile);

    var DTOsXmlCommentsFile = $"{Assembly.GetAssembly(typeof(ApplicationConfiguration))!.GetName().Name}.xml";
    var DTOsXmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, DTOsXmlCommentsFile);

    setup.IncludeXmlComments(actionMethodsXmlCommentsFullPath);
    setup.IncludeXmlComments(DTOsXmlCommentsFullPath);
});

builder.Host.UseSerilog((context, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);
});

bool isDevelopment = builder.Environment.IsDevelopment();

builder.Services.AddPersistence(builder.Configuration, isDevelopment);
builder.Services.AddApplication();

var app = builder.Build();


if (isDevelopment)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();

app.UseStatusCodePages();

app.UseExceptionHandler();

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
