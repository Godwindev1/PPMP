using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Authentication;
using dotenv;
using dotenv.net;
using PPMP.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using PPMP.Repo;
using Services;
using Services.Operations;

namespace PPMP
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            DotEnv.Load();
            var ConnectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");
            builder.Configuration.AddEnvironmentVariables();


            builder.Services.AddDbContext<UserDBContext>(options =>
                 options.UseMySql(
                     ConnectionString,
                     new MySqlServerVersion(new Version(8, 0, 34))
                 )
             );

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddControllersWithViews();

            builder.Services.AddIdentity<User, Role>(options =>
            {
                options.SignIn.RequireConfirmedEmail = true;
            })
            .AddEntityFrameworkStores<UserDBContext>()
            .AddDefaultTokenProviders();

            builder.Services.Configure<DataProtectionTokenProviderOptions>(o =>
            o.TokenLifespan = TimeSpan.FromMinutes(10));

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromDays(30);
                options.SlidingExpiration = true;
            });

            builder.Services.AddAuthorizationBuilder()
                .AddPolicy("FullAccessPolicy", policy => policy.RequireRole("DEVELOPER"))
                .AddPolicy("ClientAccessPolicy", policy => policy.RequireRole("CLIENT"));


            builder.Services.AddScoped<ClientRepo>();
            builder.Services.AddScoped<ProjectRepo>();
            builder.Services.AddScoped<StateTagRepo>();
            builder.Services.AddScoped<SubgoalRepo>();
            builder.Services.AddScoped<GoalTaskRepo>();

            builder.Services.AddTransient<EmailService>();

            builder.Services.AddScoped<IStartupOperation, SeedStateTags>();
            builder.Services.AddHostedService<StartupOperationsHostedService>();
            // Add services to the container.

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=}/{action=}/");

            app.Run();
        }

    }


}
