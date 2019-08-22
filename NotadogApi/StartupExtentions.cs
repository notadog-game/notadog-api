using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using NotadogApi.Security;
using Microsoft.AspNetCore.Http;

namespace NotadogApi
{
    public static class StartupExtensions
    {
        public static void AddJwt(this IServiceCollection services)
        {
            services.AddOptions();

            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("JWT_SECRET")));
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            var issuer = Environment.GetEnvironmentVariable("JWT_ISSUER");
            var audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE");

            services.Configure<JwtIssuerOptions>(options =>
            {
                options.Issuer = issuer;
                options.Audience = audience;
                options.SigningCredentials = signingCredentials;
            });

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingCredentials.Key,

                ValidateIssuer = true,
                ValidIssuer = issuer,

                ValidateAudience = true,
                ValidAudience = audience,

                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = tokenValidationParameters;
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = (context) =>
                        {
                            var tokenFromHeader = context.HttpContext.Request.Headers["Authorization"];
                            var tokenFromQuery = context.Request.Query["access_token"];

                            if (tokenFromHeader.Count > 0 && tokenFromHeader[0].StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                            {
                                context.Token = tokenFromHeader[0].Substring("Bearer ".Length).Trim();
                            }

                            else if (tokenFromQuery.Count > 0 && tokenFromQuery[0].StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                            {
                                context.Token = tokenFromQuery[0].Substring("Bearer ".Length).Trim();
                            }

                            return Task.CompletedTask;
                        }
                    };

                });
        }
    }
}