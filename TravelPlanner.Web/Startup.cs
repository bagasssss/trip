using System;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TravelPlanner.Web.Infrastructure;
using TravelPlanner.Web.Infrastructure.WebSocket;
using TravelPlanner.Web.Models;

namespace TravelPlanner.Web
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddTravelPlanner(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, 
            IHostingEnvironment env, 
            ILoggerFactory loggerFactory, IOptions<JWTSettings> optionsAccessor, IServiceProvider serviceProvider)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions {
                    HotModuleReplacement = true
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            AutoMapperConfig.Configure();

            // Authentication JWT Settings
            var jwtSettings = optionsAccessor.Value;
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.SecretKey));

            var tokenValidationParameters = GetTokenValidationParameters(signingKey, jwtSettings);

            app.UseJwtBearerAuthentication(new JwtBearerOptions
            {
                TokenValidationParameters = tokenValidationParameters
            });

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AutomaticAuthenticate = false,
                AutomaticChallenge = false
            });

            app.UseWebSockets();
            app.MapWebSocketManager("/ws", serviceProvider.GetService<WebSocketMessageHandler>());

            // Standart MVC settings
            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Home", action = "Index" });
            });
        }
        private TokenValidationParameters GetTokenValidationParameters(SymmetricSecurityKey signingKey,
            JWTSettings jwtSettings)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,

                // Validate the JWT Issuer (iss) claim
                ValidateIssuer = true,
                ValidIssuer = jwtSettings.Issuer,

                // Validate the JWT Audience (aud) claim
                ValidateAudience = true,
                ValidAudience = jwtSettings.Audience
            };
            return tokenValidationParameters;
        }
    }
}
