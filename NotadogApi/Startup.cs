using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using NotadogApi.Domain.Users.Repositories;
using NotadogApi.Domain.Users.Services;
using NotadogApi.Domain.Contexts;
using NotadogApi.Services;
using NotadogApi.Security;
using NotadogApi.Infrastructure;
using NotadogApi.Hubs;
using NotadogApi.Domain.Game;

namespace NotadogApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Notadog API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {{
                    new OpenApiSecurityScheme {
                        Reference = new OpenApiReference {
                            Type = ReferenceType.SecurityScheme, Id = "Bearer"
                        }
                    }, new string[] {}
                }});
            });

            services.AddCors(options => options.AddPolicy("CorsPolicy", builder =>
            {
                builder
                    .WithOrigins(Environment.GetEnvironmentVariable("ALLOWED_ORIGINS"))
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            }));

            services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("notadog-api-in-memory"));

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            services.AddScoped<ICurrentUserAccessor, CurrentUserAccessor>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IRoomStorage, RoomStorage>();
            services.AddSingleton<GameHubBroadcast>();

            services.AddJwt();
            services.AddSignalR();


        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseHsts();

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"));

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseCors("CorsPolicy");

            var gameHubBroadcast = app.ApplicationServices.GetService<GameHubBroadcast>();
            gameHubBroadcast.AssignEventHadlers();

            app.UseSignalR(routes =>
            {
                routes.MapHub<GameHub>("/api/v1/gameHub");
            });

            app.UseMvc();
        }
    }
}
