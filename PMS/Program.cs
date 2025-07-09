using System.Text;
using AspNetCoreRateLimit;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using DotNetCore.CAP;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using PMS.Common;
using PMS.Common.AutoMapper;
using PMS.Configrations;
using PMS.Data;
using PMS.Features.AuthManagement.RegisterUser.Consumers;
using PMS.Filters;
using PMS.Middlewares;

namespace PMS;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
        builder.Host.ConfigureContainer<ContainerBuilder>(container => { container.RegisterModule<ApplicationModule>(); });
        builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

        var jwtSettings = builder.Configuration.GetSection("JwtSettings");
        var key = Encoding.UTF8.GetBytes(jwtSettings.GetValue<string>("SecretKey"));
        
        var OTPSettings = builder.Configuration.GetSection("OTPSettings");
        var otpKey = Encoding.UTF8.GetBytes(OTPSettings.GetValue<string>("SecretKey"));

        builder.Services.AddCap(cfg =>
        {
            cfg.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            cfg.UseEntityFramework<Context>();
            cfg.UseRabbitMQ(opt =>
            {
                opt.HostName = "localhost";
                opt.Port = 5672; // Default AMQP port
                opt.UserName = "guest";
                opt.Password = "guest";
                opt.ExchangeName = "cap.default.router";
            });

            cfg.DefaultGroupName = "Cap.queue";
            cfg.UseDashboard();
            cfg.FailedRetryCount = 5; // Retry on failures
        });

        builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimitOptions"));
        builder.Services.AddInMemoryRateLimiting();
        builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
        builder.Services.AddScoped<UserRegisteredEventConsumer>();

        builder.Services.AddAuthentication(opts => {
                opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(opts =>
            {
                opts.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidIssuer = jwtSettings.GetValue<string>("Issuer"),
                    ValidAudience = jwtSettings.GetValue<string>("Audience"),
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                };
            })
            .AddJwtBearer("2FA", opts =>
            {
                opts.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = OTPSettings.GetValue<string>("Issuer"),
                    ValidAudience = OTPSettings.GetValue<string>("Audience"),
                    IssuerSigningKey = new SymmetricSecurityKey(otpKey),
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                };
            });
        builder.Services.AddAuthorization();
        builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
        builder.Services.AddMediatR(typeof(Program).Assembly);
        builder.Services.AddControllersWithViews(options =>
        {
            options.Filters.Add<UserInfoFilter>();
            options.Filters.Add<CancellationTokenFilter>();
        });
        builder.Services.AddAutoMapper(typeof(Program).Assembly);
        
        var app = builder.Build();
        
        AutoMapperService._mapper = app.Services.GetService<IMapper>();
        app.UseIpRateLimiting();
        if (app.Environment.IsDevelopment()) { app.UseSwagger(); app.UseSwaggerUI(); }
        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseMiddleware<GlobalErrorHandlerMiddleware>();
        app.UseMiddleware<TransactionMiddleware>();
        app.UseMiddleware<TimeOutMiddleware>();
        app.MapControllers();
        app.Run();
    }
}