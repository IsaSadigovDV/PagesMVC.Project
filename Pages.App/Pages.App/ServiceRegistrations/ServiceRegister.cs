using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Pages.App.Context;
using Pages.App.Services.Implementations;
using Pages.App.Services.Interfaces;
using Pages.Core.Entities;

namespace Pages.App.ServiceRegistrations
{
    public static class ServiceRegister
    {
        public static void Register(this IServiceCollection services, IConfiguration configuration) 
        {
            services.AddDbContext<PagesDbContext>(opt =>
            {
                opt.UseSqlServer(configuration.GetConnectionString("Default"));
            });
            services.AddScoped<IBasketService, BasketService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddTransient<IBraintreeService, BraintreeService>();
            services.AddIdentity<AppUser, IdentityRole>()
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<PagesDbContext>();
            services.Configure<IdentityOptions>(options =>
            {
                // Default Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 3;
                options.Lockout.AllowedForNewUsers = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireDigit = true;
                options.SignIn.RequireConfirmedEmail = true;
                options.User.RequireUniqueEmail = true;
            });
        }
    }
}
