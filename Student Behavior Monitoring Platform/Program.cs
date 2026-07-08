using Core.Interfaces.Repositories;
using E_learning.API.Extensions;
using E_Learning.Core.Base;
using E_Learning.Core.Interfaces.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Repository.Repositoires.GenericRepositories;
using SchoolSystem.Infrastructure.Repositories;
using SchoolSystem.Infrastructure.Repositories.Clustering.Implementations;
using Services.Services.AdminProfile;
using Services.Services.AuthServices;
using Services.Services.Behavior;
using Services.Services.BehaviorRecognition;
using Services.Services.Cluster.SchoolSystem.Application.Services.Clustering.Interfaces;
using Services.Services.Dashboard;
using Services.Services.FaceRecognition;
using Services.Services.FileStorge;
using Services.Services.Grade;
using Services.Services.Setting;
using StudentBehaviorPlatform.Application.Services;
using StudentBehaviorPlatform.Core.Services;
using StudentBehaviorPlatform.Data;
using StudentBehaviorPlatform.Data.Entities;
using StudentBehaviorPlatform.Data.Repositories;
using StudentBehaviorPlatform.Data.Repositories.Implementations;
using StudentBehaviorPlatform.Data.Repositories.Interfaces;
using StudentBehaviorPlatform.Infrastructure.Repositories;
using StudentBehaviorPlatform.Services;
using StudentBehaviorPlatform.Services.Interfaces;
using StudentBehaviorPlatform.Services.Services;
using StudentMonitor.Core.Interfaces;
using StudentMonitor.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddScoped<ResponseHandler>();

#region Swagger Configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter your JWT token"
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
#endregion

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddHttpClient<IFaceRecognitionService, FaceRecognitionService>(client =>
{
    var apiUrl = builder.Configuration["AiApiUrl"]
        ?? throw new InvalidOperationException("AiApiUrl not configured!");
    client.BaseAddress = new Uri(apiUrl);
    client.Timeout = TimeSpan.FromSeconds(30);
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});



#region Identity Configuration
builder.Services.AddIdentity<ApplicationUser, IdentityRole<int>>()
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();
#endregion

#region JWT Bearer Authentication Configuration
var jwtSettings = builder.Configuration.GetSection("Jwt");
var secretKey = jwtSettings["SecretKey"];

if (string.IsNullOrEmpty(secretKey) || secretKey.Length < 32)
    throw new InvalidOperationException("Jwt:SecretKey must be configured and at least 256 bits (32 characters).");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidateAudience = true,
        ValidAudience = jwtSettings["Audience"],
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
        RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
    };

    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            if (context.Exception is SecurityTokenExpiredException)
                context.Response.Headers.Append("Token-Expired", "true");
            return Task.CompletedTask;
        },
        OnChallenge = context =>
        {
            context.HandleResponse();
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";
            return context.Response.WriteAsJsonAsync(new
            {
                message = "Unauthorized",
                statusCode = 401
            });
        }
    };
});
#endregion

#region Authorization Configuration
builder.Services.AddAuthorization();
#endregion

#region Dependency Injection
builder.Services.AddHttpContextAccessor(); 
builder.Services.AddScoped<IBehaviorRuleRepo, BehaviorRuleRepo>();
builder.Services.AddScoped<IBehaviorRuleService, BehaviorRuleService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
builder.Services.AddScoped<IStudentRepo, StudentRepository>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IClusterRunRepository, ClusterRunRepository>();
builder.Services.AddScoped<IClusterMemberRepository, ClusterMemberRepository>();
builder.Services.AddScoped<IClusterGroupRepository, ClusterGroupRepository>();
builder.Services.AddScoped<IClusterService, ClusterService>();
builder.Services.AddScoped<IDashboardRepository, DashboardRepository>();
builder.Services.AddScoped<IDashboard, Dashboard>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IAdminProfileServices, AdminProfileServices>();
builder.Services.AddScoped<IFileStorge, FileStorge>();        
builder.Services.AddScoped<IGenericSetting, GenericSetting>();
builder.Services.AddScoped<IAttendanceService, AttendanceService>();
builder.Services.AddHttpClient<IBehaviorRecognitionService, BehaviorRecognitionService>();
builder.Services.AddScoped<IBehaviorService, BehaviorService>();
builder.Services.AddScoped<IGradeRepo, GradeRepo>();
builder.Services.AddScoped<IGradeService, GradeService>();
builder.Services.AddScoped<IBehaviorIncidentRepo, BehaviorIncidentRepo>();
builder.Services.AddScoped<IBehaviorService, BehaviorService>();


// ?? Business Logic
builder.Services.AddScoped<IAttendanceService, AttendanceService>();
#endregion


var app = builder.Build();
app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (Exception ex)
    {
        context.Response.StatusCode = 500;
        context.Response.ContentType = "text/plain";
        await context.Response.WriteAsync(ex.ToString());
    }
});
app.UseDeveloperExceptionPage();

// Configure the HTTP request pipeline
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Student Behavior Monitoring API V1");
    options.RoutePrefix = string.Empty;
});
await app.MigrateDatabaseAsync();

if (app.Environment.IsDevelopment())
{


}

app.UseHttpsRedirection();
app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();

app.MapControllers();

app.Run();
