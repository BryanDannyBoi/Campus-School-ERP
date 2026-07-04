using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SchoolERP.Application.Interfaces;
using SchoolERP.Infrastructure.Services;
using System.Text;

namespace SchoolERP.Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IPasswordService, PasswordService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IFileService, FileService>();
        services.AddScoped<IAuthService, AuthService>();

        services.AddHttpContextAccessor();

        return services;
    }
}
