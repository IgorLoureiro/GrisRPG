using GrisAPI.DbContext;
using GrisAPI.Repositories.CreatureRepository;
using GrisAPI.Repositories.UserRepository;
using GrisAPI.Services.AuthenticationService;
using GrisAPI.Services.CreatureService;
using Microsoft.EntityFrameworkCore;

namespace GrisAPI.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<ICreatureService, CreatureService>();
        return services;
    }
    
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ICreatureRepository, CreatureRepository>();
        return services;
    }
    
    public static IServiceCollection AddDatabaseContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
        
        return services;
    }
}