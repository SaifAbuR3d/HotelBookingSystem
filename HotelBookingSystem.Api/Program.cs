using HotelBookingSystem.Api.Filters;
using HotelBookingSystem.Api.Middleware;
using HotelBookingSystem.Api.Middlewares;
using HotelBookingSystem.Application;
using HotelBookingSystem.Application.Identity;
using HotelBookingSystem.Infrastructure.Identity;
using HotelBookingSystem.Infrastructure.Persistence;
using Serilog;
using System.Reflection;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(option =>
{
    option.Filters.Add<LogActivityFilter>(); 
});

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
builder.Services.AddIdentityInfrastructure(builder.Configuration);

builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(Policies.GuestOnly, policy => policy
                                      .RequireRole(UserRoles.Guest)
                                      .RequireClaim(ClaimTypes.NameIdentifier));

    options.AddPolicy(Policies.AdminOnly, policy => policy
                                      .RequireRole(UserRoles.Admin)
                                      .RequireClaim(ClaimTypes.Role, UserRoles.Admin));
});

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

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
