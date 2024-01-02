using HotelBookingSystem.Api;
using HotelBookingSystem.Application;
using HotelBookingSystem.Infrastructure.Persistence;
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

builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddApplication();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseStatusCodePages();

app.UseExceptionHandler(); 

app.UseAuthorization();

app.MapControllers();

app.Run();
