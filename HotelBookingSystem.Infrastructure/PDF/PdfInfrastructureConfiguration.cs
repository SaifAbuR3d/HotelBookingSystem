using HotelBookingSystem.Application.Abstractions.InfrastructureInterfaces.PdfInterfaces;
using Microsoft.Extensions.DependencyInjection;
using QuestPDF.Infrastructure;

namespace HotelBookingSystem.Infrastructure.PDF;

public static class PdfInfrastructureConfiguration
{
    public static IServiceCollection AddPdfInfrastructure(
        this IServiceCollection services)
    {
        QuestPDF.Settings.License = LicenseType.Community;
        return services.AddScoped<IPdfGenerator, PdfGenerator>();
    }
}
