using GuGullyHive.Admin.Repositories;
using GullyHive.Admin.Models;
using GullyHive.Admin.Repositories;
using GullyHive.Admin.Services;
using GullyHive.Auth.Repositories;
using GullyHive.Auth.Services;
using GullyHive.Seller.Repositories;
using GullyHive.Seller.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using StackExchange.Redis;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
// prevent 500
var jwtKey = builder.Configuration["Jwt:Key"];
if (string.IsNullOrWhiteSpace(jwtKey))
    throw new Exception("JWT Key missing");

var conStr = builder.Configuration.GetConnectionString("ConStr");
if (string.IsNullOrWhiteSpace(conStr))
    throw new Exception("Connection string missing");
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();
builder.Logging.SetMinimumLevel(LogLevel.Debug);
// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(builder.Configuration["FrontendUrl"] ?? "http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.MapInboundClaims = false; // THIS FIXES 403

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
            ),

            RoleClaimType = "role" // matches JWT exactly
        };
    });
// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSingleton<DataProtectionHelper>();

// Question Master Services and Repos
builder.Services.AddScoped<IQuestionMasterService, QuestionMasterService>();
builder.Services.AddScoped<IQuestionMasterRepository, QuestionMasterRepository>();

builder.Services.AddScoped<IServiceCategoryMasterRepository, ServiceCategoryMasterRepository>();
builder.Services.AddScoped<IServiceCategoryMasterService, ServiceCategoryMasterService>();

builder.Services.AddScoped<IStateMasterRepository, StateMasterRepository>();
builder.Services.AddScoped<IStateMasterService, StateMasterService>();

builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IRoleService, RoleService>();

builder.Services.AddScoped<ICityRepository, CityRepository>();
builder.Services.AddScoped<ICityService, CityService>();

builder.Services.AddScoped<IUserRoleRepository, UserRoleRepository>();
builder.Services.AddScoped<IUserRoleService, UserRoleService>();

builder.Services.AddScoped<ISystemSettingRepository, SystemSettingRepository>();
builder.Services.AddScoped<ISystemSettingService, SystemSettingService>();

builder.Services.AddScoped<IJobStatusMasterRepository, JobStatusMasterRepository>();
builder.Services.AddScoped<IJobStatusMasterService, JobStatusMasterService>();

builder.Services.AddScoped<IProviderStatusMasterRepository, ProviderStatusMasterRepository>();
builder.Services.AddScoped<IProviderStatusMasterService, ProviderStatusMasterService>();

builder.Services.AddScoped<ILeadStatusMasterRepository, LeadStatusMasterRepository>();
builder.Services.AddScoped<ILeadStatusMasterService, LeadStatusMasterService>();

builder.Services.AddScoped<ISubCategoryMasterRepository, SubCategoryMasterRepository>();
builder.Services.AddScoped<ISubCategoryMasterService, SubCategoryMasterService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IDashboardRepository, DashboardRepository>();
builder.Services.AddScoped<IDashboardService, DashboardService>();

builder.Services.AddScoped<ILeadRepository, LeadRepository>();
builder.Services.AddScoped<ILeadService, LeadService>();

builder.Services.AddScoped<IPublicProfileRepository, PublicProfileRepository>();
builder.Services.AddScoped<IPublicProfileService, PublicProfileService>();

builder.Services.AddScoped<IResponseRepository, ResponseRepository>();
builder.Services.AddScoped<IResponseService, ResponseService>();

builder.Services.AddScoped<IHelpRepository, HelpRepository>();
builder.Services.AddScoped<IHelpService, HelpService>();

builder.Services.AddScoped<IReferralRepository, ReferralRepository>();
builder.Services.AddScoped<IReferralService, ReferralService>();

builder.Services.AddScoped<IPartnerEarningRepository, PartnerEarningRepository>();
builder.Services.AddScoped<IPartnerEarningService, PartnerEarningService>();





// Auth module controllers
builder.Services.AddControllers()
    .AddApplicationPart(typeof(GullyHive.Auth.Controllers.AuthController).Assembly)
    .AddApplicationPart(typeof(GullyHive.Seller.Controllers.SellerController).Assembly)
    .AddApplicationPart(typeof(GullyHive.Admin.Controllers.QuestionMasterController).Assembly)
    .AddApplicationPart(typeof(GullyHive.Admin.Controllers.ServiceCategoryMasterController).Assembly)
    .AddApplicationPart(typeof(GullyHive.Admin.Controllers.StateMasterController).Assembly)
    .AddApplicationPart(typeof(GullyHive.Admin.Controllers.RoleController).Assembly)
    .AddApplicationPart(typeof(GullyHive.Admin.Controllers.CityController).Assembly)
    .AddApplicationPart(typeof(GullyHive.Admin.Controllers.UserRolesController).Assembly)
    .AddApplicationPart(typeof(GullyHive.Admin.Controllers.SystemSettingsController).Assembly)
    .AddApplicationPart(typeof(GullyHive.Admin.Controllers.JobStatusMasterController).Assembly)
    .AddApplicationPart(typeof(GullyHive.Admin.Controllers.ProviderStatusMasterController).Assembly)
    .AddApplicationPart(typeof(GullyHive.Admin.Controllers.LeadStatusMasterController).Assembly)
    .AddApplicationPart(typeof(GullyHive.Admin.Controllers.SubCategoryMasterController).Assembly);


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
// Swagger with header
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new()
    {
        Title = "GullyHive API",
        Version = "v1"
    });

    //  JWT Bearer config
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter JWT token like: Bearer {your token}"
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// redis
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var options = ConfigurationOptions.Parse("localhost:6379");
    options.AbortOnConnectFail = false; // IMPORTANT
    return ConnectionMultiplexer.Connect(options);
});
var app = builder.Build();

// 🔥 TEMP: show full exceptions on Render
app.UseDeveloperExceptionPage();

// Swagger enabled in Production (important for Render)
app.UseSwagger();
app.UseSwaggerUI();

// app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseCors("AllowFrontend");
app.UseRouting();


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers().RequireCors("AllowFrontend");


app.Run();
