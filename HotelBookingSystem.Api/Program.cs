using HotelBookingSystem.Api;
using HotelBookingSystem.Application;
using HotelBookingSystem.Infrastructure.Email;
using HotelBookingSystem.Infrastructure.Identity;
using HotelBookingSystem.Infrastructure.PDF;
using HotelBookingSystem.Infrastructure.Persistence;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);
});

bool isDevelopment = builder.Environment.IsDevelopment();

builder.Services.AddApplication();

builder.Services.AddPersistenceInfrastructure(builder.Configuration, isDevelopment);
builder.Services.AddIdentityInfrastructure(builder.Configuration);
builder.Services.AddPdfInfrastructure(); 
builder.Services.AddEmailInfrastructure(builder.Configuration);

builder.Services.AddWebComponents(); 


var app = builder.Build();

if (isDevelopment)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseSerilogRequestLogging();

app.UseStaticFiles();

app.Migrate();

app.UseCors();

app.UseStatusCodePages();

app.UseExceptionHandler();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
