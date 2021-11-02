using AspNetCoreRateLimit;
using HotelListingPractice.DataAccess.Data;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelListingPractice.Utility
{
    public static class ServiceExtensions
    {
        // This method configures global error handling,
        // which allows us to make our exception handling more generic
        // so that a try-catch is not required in each action method
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            // Specify exception handler options
            app.UseExceptionHandler(error =>
            {
                // Context represents the controller that is passing down the information
                error.Run(async context =>
                {
                    // Set 500 "Server Error" as a constant
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    // Set the content-type of the response header
                    context.Response.ContentType = "application/json";
                    // Retrieve the details of the error
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

                    // Log the details
                    if (contextFeature != null)
                    {
                        Log.Error($"Something Went Wrong in the {contextFeature.Error}");

                        await context.Response.WriteAsync(new Error
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = "Internal Server Error. Please Try Again Later."
                        }.ToString());
                    }

                });
            });
        }

        public static void ConfigureVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                // Report in the header of responses what version of the API is being used by the client
                options.ReportApiVersions = true;

                // Use the default API version when the client does not specify one
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);

                // Allow the client to specify in the header which version of the API they would like to use
                options.ApiVersionReader = new HeaderApiVersionReader("api-version");
            });
        }

        public static void ConfigureHttpCacheHeaders(this IServiceCollection services)
        {
            // Enable Caching by creating a parameter in a response header which determines the cache length
            services.AddResponseCaching();

            // Configure global cache options using Marvin.Cache.Headers
            services.AddHttpCacheHeaders(
                (expirationOpt) =>
                {
                    expirationOpt.MaxAge = 120;
                    expirationOpt.CacheLocation = CacheLocation.Private;
                },
                (validationOpt) =>
                {
                    // If the there has been an update in the database, the cache is forced to be revalidated and refreshed
                    validationOpt.MustRevalidate = true;
                });
        }

        public static void ConfigureRateLimiting(this IServiceCollection services)
        {
            // Initialize the rate limit rules
            var rateLimitRules = new List<RateLimitRule>
            {
                new RateLimitRule
                {
                    // This rule will apply to every endpoint
                    Endpoint = "*",

                    // Limit to one call per second
                    Limit = 1,
                    Period = "5s"
                }
            };

            // Apply the rules
            services.Configure<IpRateLimitOptions>(options =>
            {
                options.GeneralRules = rateLimitRules;
            });

            // Implement libraries necessary for AspNetCoreRateLimit
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
        }

        public static void ConfigureIdentity(this IServiceCollection services)
        {
            var builder = services.AddIdentityCore<ApiUser>(q => q.User.RequireUniqueEmail = true);

            builder = new IdentityBuilder(builder.UserType, typeof(IdentityRole), services);
            builder.AddEntityFrameworkStores<DatabaseContext>().AddDefaultTokenProviders();
        }

        public static void ConfigureJWT(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("Jwt");

            //var key = Environment.GetEnvironmentVariable("KEY");
            string key = "293475298374598273459827";

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        var userMachine = context.HttpContext.RequestServices.GetRequiredService<UserManager<ApiUser>>();
                        var user = userMachine.GetUserAsync(context.HttpContext.User);

                        if (user == null)
                        {
                            context.Fail("UnAuthorized");
                        }

                        return Task.CompletedTask;
                    }
                };
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "https://localhost:44373/",
                    //ValidAudience = "https://localhost:44373/",
                    //ValidIssuer = jwtSettings.GetSection("Issuer").Value,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
                };
            });
        }
    }
}
