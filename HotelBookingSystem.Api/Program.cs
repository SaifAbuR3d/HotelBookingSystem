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

builder.Services.AddWebComponents(builder.Configuration);



var app = builder.Build();


app.UseStatusCodePages();

//if (!isDevelopment)
//{
    app.UseExceptionHandler(); // Adds my custom GlobalExceptionHandler to the pipeline
//}

app.UseSwagger();
app.UseSwaggerUI(); // this should be only in development, but I use it as a user-friendly way to test the API in production

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.Migrate();

app.UseCors();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
