using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace VehiclePartsInventory.Api.Extensions;

public static class IdentityServiceExtensions
{
    public static IServiceCollection AddIdentityServices(
        this IServiceCollection services,
        IConfiguration config)
    {
        var jwtKey = config["Jwt:Key"];

        if (string.IsNullOrWhiteSpace(jwtKey))
            throw new Exception("JWT key is missing in appsettings.json.");

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                var key = Encoding.UTF8.GetBytes(jwtKey);

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),

                    ValidateIssuer = true,
                    ValidIssuer = config["Jwt:Issuer"],

                    ValidateAudience = true,
                    ValidAudience = config["Jwt:Audience"],

                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

        services.AddAuthorization();

        return services;
    }
}