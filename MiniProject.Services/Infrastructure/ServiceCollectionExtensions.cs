using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MiniProject.DAL.ScaffoldContainer;
using MiniProject.Services.Master.Interface.Kecamatan;
using MiniProject.Services.Master.Interface.Kelurahan;
using MiniProject.Services.Master.Interface.Kodepos;
using MiniProject.Services.Master.Interface.Kota;
using MiniProject.Services.Master.Service.Kecamatan;
using MiniProject.Services.Master.Service.Kelurahan;
using MiniProject.Services.Master.Service.Kodepos;
using MiniProject.Services.Master.Service.Kota;
using MiniProject.Services.Transaction.Interface;
using MiniProject.Services.Transaction.Service;

namespace MiniProject.Services.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMiniProjectServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("MiniProjectConnection")
            ?? "Server=localhost; Database=Latihan1; User ID=postgres; Password=Lazzuard;";

        services.AddDbContext<Latihan1Context>(o => o.UseNpgsql(connectionString));
        services.AddScoped<IKotaService, KotaService>();
        services.AddScoped<IKecamatanService, KecamatanService>();
        services.AddScoped<IKelurahanService, KelurahanService>();
        services.AddScoped<IKodeposService, KodeposService>();
        services.AddScoped<IBudgetService, BudgetService>();

        return services;
    }
}
