using AspNetCoreRateLimit;
using HotelListingPractice.AuthManagement;
using HotelListingPractice.DataAccess.Data;
using HotelListingPractice.DataAccess.IRepository;
using HotelListingPractice.DataAccess.Repository;
using HotelListingPractice.Utility;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelListingPractice
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Configure Cors Policy
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                    builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });

            // Configure Entity Framework and SQL
            services.AddDbContext<DatabaseContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
            );

            // Configure Data Access
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAuthManager, AuthManager>();

            // Configure AutoMapper to map Database objects to and from application DTO Models
            services.AddAutoMapper(typeof(MapperConfig));

            // The CacheProfile is the default way for implementing Caching.
            // However, after implementing Marvin.Cache.Headers, the CacheProfile is now configured in the ServiceExtensions method.
            // The newtonsoft options tell the API to ignore errors for reference loop cycles.
            services.AddControllers(config =>
            {
                config.CacheProfiles.Add("120SecondsDuration", new CacheProfile
                {
                    Duration = 120
                });
            }).AddNewtonsoftJson(options =>
                                 options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "HotelListingPractice", Version = "v1" });
            });

            // Configure Versioning
            services.ConfigureVersioning();

            // Configure global Cache Profile and Validation Options
            services.ConfigureHttpCacheHeaders();

            // Configure AspNetCoreRateLimit which enables rate limiting and throttling to prevent high traffic and DDOS attacks
            services.AddMemoryCache();
            services.ConfigureRateLimiting();
            services.AddHttpContextAccessor();

            // Configure Identity and JWT
            services.AddAuthentication();
            services.ConfigureIdentity();
            services.ConfigureJWT(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "HotelListingPractice v1"));
            }

            // Global Exception Handler
            app.ConfigureExceptionHandler();

            // Cors Policy
            app.UseCors("AllowAll");            

            app.UseHttpsRedirection();

            // Caching
            app.UseResponseCaching();
            app.UseHttpCacheHeaders();

            // AspNetCoreRateLimiting
            app.UseIpRateLimiting();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
