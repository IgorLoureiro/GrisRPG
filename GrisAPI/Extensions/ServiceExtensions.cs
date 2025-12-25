using GrisAPI.DbContext;
using GrisAPI.Repositories.CardRepository;
using GrisAPI.Repositories.CreatureRepository;
using GrisAPI.Repositories.DeckRepository;
using GrisAPI.Repositories.ExtraDeckRepository;
using GrisAPI.Repositories.JokerRepository;
using GrisAPI.Repositories.UserRepository;
using GrisAPI.Services.AuthenticationService;
using GrisAPI.Services.CardService;
using GrisAPI.Services.CreatureService;
using GrisAPI.Services.DeckService;
using GrisAPI.Services.ExtraDeckService;
using GrisAPI.Services.JokerService;
using Microsoft.EntityFrameworkCore;

namespace GrisAPI.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<ICreatureService, CreatureService>();
        services.AddScoped<IExtraDeckService, ExtraDeckService>();
        services.AddScoped<IJokerService, JokerService>();
        services.AddScoped<ICardService, CardService>();
        services.AddScoped<IDeckService, DeckService>();
        return services;
    }
    
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ICreatureRepository, CreatureRepository>();
        services.AddScoped<IExtraDeckRepository, ExtraDeckRepository>();
        services.AddScoped<IJokerRepository, JokerRepository>();
        services.AddScoped<ICardRepository, CardRepository>();
        services.AddScoped<IDeckRepository, DeckRepository>();
        return services;
    }
    
    public static IServiceCollection AddDatabaseContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
        
        return services;
    }
}