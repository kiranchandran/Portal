using CompanyName.Data.Contexts;
using CompanyName.Repository.Interfaces;
using CompanyName.Repository;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using CompanyName.Services.Interfaces;
using CompanyName.Services.Implementations;
using CompanyName.Data.Seeds;
using CompanyName.Api.Filters;
using CompanyName.Services.Mapping;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using System.Reflection;
using Microsoft.OpenApi.Models;

namespace CompanyName.Api.Extensions
{
    /// <summary>
    /// Extensions for Service Collection.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Configure the services needed for the application.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers(config =>
            {
                //config.ModelValidatorProviders.Clear();
                config.Filters.Add(typeof(GlobalExceptionFilter));
            }).AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });
            services.AddDbContext<DataContext>(cfg =>
            {
                //cfg.UseSqlServer(configuration.GetConnectionString("ConnectionString"));
                cfg.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString());
            }, ServiceLifetime.Singleton, ServiceLifetime.Singleton);
            services.ConfigureDomainServices();
            services.ConfigureDataServices();
            services.AddAutoMapper(typeof(EmployeeMappingProfile).Assembly);
            services.ConfigureEnvironmentOptions(configuration);
            services.ConfigureCorsServices();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(option =>
            {
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                option.IncludeXmlComments(xmlPath);

                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token. Enter just the token, don't add Bearer in front of it.",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });
        }

        /// <summary>
        /// Configures the authentication and authorization.
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/>.</param>
        /// <param name="configuration"><see cref="IConfiguration"/>.</param>
        public static void ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApi(configuration.GetSection("AzureAd"));

            services.AddAuthorization(options =>
              options.AddPolicy("ApiAcessPolicy",
              policy => policy.RequireAuthenticatedUser().RequireClaim("http://schemas.microsoft.com/identity/claims/scope", "API")));
        }

        private static void ConfigureDomainServices(this IServiceCollection services)
        {
            services.AddTransient<DataInitialiser>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        private static void ConfigureDataServices(this IServiceCollection services)
        {
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IDepartmentService, DepartmentService>();
        }


        private static void ConfigureEnvironmentOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();
        }

        private static void ConfigureCorsServices(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                {
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });
        }        
    }
}
