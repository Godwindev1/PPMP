using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using dotenv.net;
using PPMP.API.Data;
using PPMP.API.Repo;
using PPMP.API.Services;
using PPMP.API.Services.Operations;

var builder = WebApplication.CreateBuilder(args);

DotEnv.Load();
var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");
builder.Configuration.AddEnvironmentVariables();

// Database
builder.Services.AddDbContext<UserDBContext>(options =>
    options.UseMySql(
        connectionString,
        new MySqlServerVersion(new Version(8, 0, 34))
    )
);

// Identity
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

// Authorization
builder.Services.AddAuthorizationBuilder()
    .AddPolicy("FullAccessPolicy", policy => policy.RequireRole("DEVELOPER"))
    .AddPolicy("ClientAccessPolicy", policy => policy.RequireRole("CLIENT"));

// App services
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

// Repos
builder.Services.AddScoped<ClientRepo>();
builder.Services.AddScoped<ProjectRepo>();
builder.Services.AddScoped<StateTagRepo>();
builder.Services.AddScoped<SubgoalRepo>();
builder.Services.AddScoped<GoalTaskRepo>();

// Services
builder.Services.AddTransient<EmailService>();
builder.Services.AddScoped<IStartupOperation, SeedStateTags>();
builder.Services.AddHostedService<StartupOperationsHostedService>();

// CORS — allows PPMP.Web to call this API
builder.Services.AddCors(options =>
{
    options.AddPolicy("WebClient", policy =>
        policy.WithOrigins("https://localhost:7001")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
   // app.UseSwagger();
    // app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("WebClient");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();