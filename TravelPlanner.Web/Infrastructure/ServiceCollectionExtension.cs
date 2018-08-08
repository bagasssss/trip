using System;
using System.Data.Entity;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TravelPlanner.BusinessLogic.Interfaces;
using TravelPlanner.BusinessLogic.Security;
using TravelPlanner.BusinessLogic.Services;
using TravelPlanner.DataAccess;
using TravelPlanner.DomainModel;
using TravelPlanner.Identity.IdentityManagers;
using TravelPlanner.Identity.IdentityStores;
using TravelPlanner.Identity.IdentityValidators;
using TravelPlanner.Web.Infrastructure.WebSocket;
using TravelPlanner.Web.Models;

namespace TravelPlanner.Web.Infrastructure
{
    public static class ServiceCollectionExtension
    {
        public static void AddTravelPlanner(this IServiceCollection services, IConfigurationRoot configuration)
        {
            ConfigureDb(services, configuration);
            ConfigureIdentity(services);
            ConfigureSecurity(services, configuration);
            ConfigureBusinessLogic(services);
            ConfigureWebSocket(services);
        }

        private static void ConfigureDb(IServiceCollection services, IConfigurationRoot configuration)
        {
            services.AddTransient<IGenericRepository, EntityFrameworkRepository>();
            services.AddScoped<DbContext>((provider) => new TravelPlannerDbContext(configuration.GetConnectionString("DefaultConnection")));
            //DbInitializer.Initialize(new TravelPlannerDbContext(Configuration.GetConnectionString("DefaultConnection")));
        }

        private static void ConfigureBusinessLogic(IServiceCollection services)
        {
            services.AddTransient<ITripService, TripService>();
            services.AddTransient<ISightObjectService, SightObjectService>();
            services.AddTransient<ITripInviteService, TripInviteService>();
            services.AddTransient<INotificationService, TextFileNotificationService>();
            services.AddTransient<IMessageService, MessageService>();
        }

        private static void ConfigureIdentity(IServiceCollection services)
        {
            services.AddTransient<IUserStore<User, Guid>, TravelPlannerUserStore>();
            services.AddTransient<ApplicationUserManager>((ctx) => new ApplicationUserManager(ctx.GetService<IUserStore<User, Guid>>())
            {
                PasswordValidator = new ApplicationPasswordValidator()
                {
                    RequiredLength = 6
                },
                UserValidator = new ApplicationUserValidator<User>(new ApplicationUserManager(ctx.GetService<IUserStore<User, Guid>>()))
                {
                    RequireUniqueEmail = true
                }
            });
        }

        private static void ConfigureSecurity(IServiceCollection services, IConfigurationRoot configuration)
        {
            services.Configure<JWTSettings>(configuration.GetSection("JWTSettings"));
            services.AddTransient<IAuthTokenManager, JWTTokenManager>();

            services.Configure<IdentityOptions>(opts =>
            {
                opts.Cookies.ApplicationCookie.Events = new CookieAuthenticationEvents()
                {
                    OnRedirectToLogin = ctx =>
                    {
                        ctx.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        return Task.FromResult("User Unauthorized");
                    }
                };
            });
        }

        private static void ConfigureWebSocket(IServiceCollection services)
        {
            services.AddTransient<WebSocketConnectionManager>();
            services.AddSingleton<WebSocketMessageHandler>();
        }
    }
}
