using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MiniProject.DAL.Context;
using MiniProject.Services.Auth;
using MiniProject.Services.Master;
using MiniProject.Services.Transaction.Service;

namespace MiniProject.Services.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMiniProjectServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("MiniProjectConnection")
            ?? throw new InvalidOperationException("Connection string 'MiniProjectConnection' not found.");

        services.AddDbContext<MiniProjectDbContext>(o => o.UseNpgsql(connectionString));
        services.AddScoped<ISampleService, SampleService>();
        services.AddScoped<IRegionService, RegionService>();
        services.AddScoped<IBranchService, BranchService>();
        services.AddScoped<IBudgetService, BudgetService>();
        services.AddScoped<IAuthService, AuthService>();

        return services;
    }
}
