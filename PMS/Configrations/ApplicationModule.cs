using System.Reflection;
using System.Text;
using Autofac;
using DotNetCore.CAP;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.SwaggerGen;
using PMS.Common;
using PMS.Common.BaseEndpoints;
using PMS.Common.BaseHandlers;
using PMS.Common.Views;
using PMS.Data;
using PMS.Data.Interceptors;
using PMS.Data.Repositories;
using PMS.Features.AuthManagement.ActivateUser2FA;
using PMS.Features.AuthManagement.ConfirmUserRegistration;
using PMS.Features.AuthManagement.LogInUser;
using PMS.Features.AuthManagement.RegisterUser;
using PMS.Features.AuthManagement.RegisterUser.Consumers;
using PMS.Features.Common.Pagination;
using PMS.Features.UserManagement.GetAllUsers;
using PMS.Features.UserManagement.GetAllUsers.Queries;
using PMS.Helpers;
using PMS.Models;
using Module = Autofac.Module;

namespace PMS.Configrations
{
    public class ApplicationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            #region Database Context Registration
            builder.Register(context =>
            {
                var config = context.Resolve<IConfiguration>();
                var connectionString = config.GetConnectionString("DefaultConnection");
                var options = new DbContextOptionsBuilder<Context>()
                    .UseSqlServer(connectionString)
                    .Options;

                return new Context(options);
            }).As<Context>().InstancePerLifetimeScope();
            #endregion

            #region Services Registration
            builder.RegisterType<UserInfo>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<TokenHelper>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<UserInfoProvider>().AsSelf().InstancePerLifetimeScope();
            #endregion

            #region MediatR Handlers Registration
            // Register MediatR softDeleteRequest handlers
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .AsClosedTypesOf(typeof(IRequestHandler<,>))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
            #endregion

            #region FluentValidation Registration
            // Register FluentValidation validators
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(t => t.IsClosedTypeOf(typeof(IValidator<>)))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
            #endregion

            #region Endpoint Registration
            // Register specific endpoint parameters
            builder.RegisterGeneric(typeof(BaseEndpointParameters<>))
                .AsSelf()
                .InstancePerLifetimeScope();
            // Register endpoints
            builder.RegisterType<RegisterUserEndpoint>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<ConfirmEmailEndpoint>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<Activate2FAQRCodeEndpoint>().AsSelf().InstancePerLifetimeScope();
            #endregion

            #region Repository Registration
            // Register repositories
            // builder.RegisterType<UserRepository>().As<IUserRepository>().InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();
            builder.RegisterType<Repository<BaseModel>>().As<IRepository<BaseModel>>().InstancePerLifetimeScope();
            #endregion

            #region JWT Authentication Registration
            // Register JWT authentication
            builder.Register(context =>
            {
                var config = context.Resolve<IConfiguration>();
                var jwtSettings = config.GetSection("JwtSettings");
                var secretKey = jwtSettings.GetValue<string>("SecretKey");
                if (string.IsNullOrEmpty(secretKey))
                {
                    throw new InvalidOperationException("SecretKey is not configured properly in appsettings.json");
                }

                var key = Encoding.UTF8.GetBytes(secretKey);

                return new JwtBearerOptions
                {
                    TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidIssuer = jwtSettings.GetValue<string>("Issuer"),
                        ValidAudience = jwtSettings.GetValue<string>("Audience"),
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateAudience = true,
                        ValidateIssuer = true,
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,
                    }
                };
            }).As<JwtBearerOptions>().SingleInstance();
            #endregion

            #region HttpContextAccessor Registration
            // Register HttpContextAccessor
            builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>().SingleInstance();
            #endregion

            #region Controller Registration
            // Register controllers
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(t => t.IsSubclassOf(typeof(ControllerBase)))
                .AsSelf()
                .InstancePerLifetimeScope();
            #endregion

            #region Swagger Registration
            // Register SwaggerGen options
            builder.RegisterType<SwaggerGenOptions>().AsSelf().SingleInstance();
            #endregion

            #region ViewModel Validators Registration
            // Register validators for ViewModels
            builder.RegisterType<ConfirmEmailRequestViewModelValidator>()
                .As<IValidator<ConfirmEmailRequestViewModel>>()
                .InstancePerLifetimeScope();
            builder.RegisterType<RegisterUserRequestViewModelValidator>()
                .As<IValidator<RegisterUserRequestViewModel>>()
                .InstancePerLifetimeScope();
            #endregion

            #region Other Registrations
            builder.RegisterType<LogInInfoDTOValidator>()
                .As<IValidator<LogInInfoDTO>>()
                .InstancePerLifetimeScope();
            builder.RegisterType<UserInfoFilter>().As<IActionFilter>().InstancePerLifetimeScope();
            builder.RegisterType<Mediator>().As<IMediator>().InstancePerLifetimeScope(); 
            #endregion
            builder.RegisterType<BaseRequestHandlerParameters>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<GetAllUsersQueryHandler>().As<IRequestHandler<GetAllUsersQuery, RequestResult<PaginatedResult<UserDTO>>>>().InstancePerLifetimeScope();
            builder.RegisterType<TimeOutMiddleware>().InstancePerDependency();
            builder.RegisterType<CancellationTokenProvider>().AsSelf().SingleInstance();
            builder.RegisterType<CancelCommandInterceptor>().AsSelf().SingleInstance();

        }
    }
}
