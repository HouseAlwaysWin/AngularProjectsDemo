using System.IO;
using System.Text;
using System.Collections.Generic;
using EcommerceApi.Core.Entities.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using EcommerceApi.Core.Services.Interfaces;
using EcommerceApi.Core.Services;
using EcommerceApi.Helpers.Localization;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using EcommerceApi.Core.Data.Identity;
using EcommerceApi.Core.Data;
using EcommerceApi.Core.Data.Repositories;
using EcommerceApi.Core.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using EcommerceApi.Core.Data.Services.Interfaces;
using EcommerceApi.Core.Services.Repositories;
using Microsoft.Extensions.FileProviders;
using System;
using StackExchange.Redis;
using Microsoft.OpenApi.Models;

namespace EcommerceApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration
        )
        {
            _config = configuration;
        }

        public IConfiguration _config { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            services.AddAutoMapper(typeof(AutomapperProfiles));
            services.AddControllers();
            // services.AddDbContext<ECIdentityDbContext>(x =>
            //     // x.UseSqlServer(_config.GetConnectionString("IdentityConnection")));
            //        x.UseNpgsql(_config.GetConnectionString("IdentityConnection")));
            
            // services.AddDbContext<StoreContext>(x => 
            //     // x.UseSqlServer(_config.GetConnectionString("DefaultConnection")));
            //     x.UseNpgsql(_config.GetConnectionString("DefaultConnection")));

           services.AddDbContext<ECIdentityDbContext>(options =>
            {

                string connStr;

                // Depending on if in development or production, use either Heroku-provided
                // connection string, or development connection string from env var.
                if (env == "Development")
                {
                    // Use connection string from file.
                    connStr = _config.GetConnectionString("IdentityConnection");
                }
                else
                {
                    // Use connection string provided at runtime by Heroku.
                    var connUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

                    // Parse connection URL to connection string for Npgsql
                    connUrl = connUrl.Replace("postgres://", string.Empty);
                    var pgUserPass = connUrl.Split("@")[0];
                    var pgHostPortDb = connUrl.Split("@")[1];
                    var pgHostPort = pgHostPortDb.Split("/")[0];
                    var pgDb = pgHostPortDb.Split("/")[1];
                    var pgUser = pgUserPass.Split(":")[0];
                    var pgPass = pgUserPass.Split(":")[1];
                    var pgHost = pgHostPort.Split(":")[0];
                    var pgPort = pgHostPort.Split(":")[1];

                    connStr = $"Server={pgHost};Port={pgPort};User Id={pgUser};Password={pgPass};Database={pgDb};SSL Mode=Require;TrustServerCertificate=True";
                }

                // Whether the connection string came from the local development configuration file
                // or from the environment variable from Heroku, use it to set up your DbContext.
                options.UseNpgsql(connStr);
            }); 

            services.AddDbContext<StoreContext>(options =>
            {

                string connStr;

                // Depending on if in development or production, use either Heroku-provided
                // connection string, or development connection string from env var.
                if (env == "Development")
                {
                    // Use connection string from file.
                    connStr = _config.GetConnectionString("DefaultConnection");
                }
                else
                {
                    // Use connection string provided at runtime by Heroku.
                    var connUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

                    // Parse connection URL to connection string for Npgsql
                    connUrl = connUrl.Replace("postgres://", string.Empty);
                    var pgUserPass = connUrl.Split("@")[0];
                    var pgHostPortDb = connUrl.Split("@")[1];
                    var pgHostPort = pgHostPortDb.Split("/")[0];
                    var pgDb = pgHostPortDb.Split("/")[1];
                    var pgUser = pgUserPass.Split(":")[0];
                    var pgPass = pgUserPass.Split(":")[1];
                    var pgHost = pgHostPort.Split(":")[0];
                    var pgPort = pgHostPort.Split(":")[1];

                    connStr = $"Server={pgHost};Port={pgPort};User Id={pgUser};Password={pgPass};Database={pgDb};SSL Mode=Require;TrustServerCertificate=True";
                }

                // Whether the connection string came from the local development configuration file
                // or from the environment variable from Heroku, use it to set up your DbContext.
                options.UseNpgsql(connStr);
            });


            // services.AddDistributedMemoryCache(); 
             services.AddStackExchangeRedisCache(options =>
            {

                string redisString = string.Empty;
                ConfigurationOptions config = null;
                if (env == "Development")
                {
                    redisString = _config.GetConnectionString("RedisUrl");
                    config = ConfigurationOptions.Parse(
                        redisString ,true);

                }
                else{
                    var connUrl = Environment.GetEnvironmentVariable("REDIS_URL");
                    connUrl = connUrl.Replace("redis://", string.Empty);
                    var userInfo = connUrl.Split('@')[0];
                    var host = connUrl.Split('@')[1];
                    var password = userInfo.Split(':')[1];
                    var domain = host.Split(':')[0];
                    var port = host.Split(':')[1];
                    redisString = $"{domain}:{port},abortConnect=False,password={password}";
                    config = ConfigurationOptions.Parse(
                        redisString ,true);
                }

                options.ConfigurationOptions = config;
            }); 

            
            services.AddIdentityCore<ECUser>()
                .AddEntityFrameworkStores<ECIdentityDbContext>()
                .AddSignInManager<SignInManager<ECUser>>();
            
            services.Configure<IdentityOptions>(options =>{
                options.Lockout.AllowedForNewUsers = false;

                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 0;

                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;

                options.User.RequireUniqueEmail = true;
            });


            var Issuer = (env == "Development") ?_config["Token:Issuer"] : Environment.GetEnvironmentVariable("Token:Issuer");
            var tokenKey = (env == "Development") ?_config["Token:Key"] : Environment.GetEnvironmentVariable("Token:Key");
            
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>{
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters{
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey)),
                        ValidIssuer = Issuer,
                        ValidateIssuer = true,
                        ValidateAudience = false
                    };
                });

            services.AddJsonLocalization(opts =>
            {
                // opts.ResourcesPath = "Resources";
            });

            services.Configure<RequestLocalizationOptions>(opts =>
            {
                var supportCultures = new List<CultureInfo>
                    {
                        new CultureInfo("en"),
                        new CultureInfo("zh-TW")
                    };

                opts.DefaultRequestCulture = new RequestCulture("zh-TW");
                opts.SupportedCultures = supportCultures;
                opts.SupportedUICultures = supportCultures;
                opts.RequestCultureProviders = new List<IRequestCultureProvider>{
                    new AcceptLanguageHeaderRequestCultureProvider()
                };
            });

            services.AddScoped<ITokenService,TokenService>();
            services.AddScoped<IProductService,ProductService>();
            services.AddScoped<IOrderService,OrderService>();
            services.AddScoped<IPaymentService,PaymentService>();
            services.AddSingleton<ICachedService,CachedService>();
            services.AddScoped<ILanguageService,LanguageService>();
            services.AddSingleton<IHttpContextAccessor,HttpContextAccessor>();
            services.AddScoped<ILocalizedService,LocalizedService>();
            services.AddScoped<IBasketService,BasketService>();
            services.AddScoped(typeof(IEntityRepository<>),(typeof(EntityRepository<>)));
            services.AddScoped<IUnitOfWork,UnitOfWork>();

            
            services.AddSwaggerGen(c =>{
                                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo{
                                    Title = "Ecommerce",
                                    Version = "v1"
                                });

                                var securitySchema = new OpenApiSecurityScheme{
                                    Description = "JWT Auth Bearer Scheme",
                                    Name = "Authorization",
                                    In = ParameterLocation.Header,
                                    Type = SecuritySchemeType.Http,
                                    Scheme = "bearer",
                                    Reference = new OpenApiReference{
                                        Type = ReferenceType.SecurityScheme,
                                        Id = "Bearer"
                                    }
                                };

                                c.AddSecurityDefinition("Bearer",securitySchema);

                                var securityRequirement = new OpenApiSecurityRequirement { {
                                    securitySchema , new [] {"Bearer"}
                                }};

                                c.AddSecurityRequirement(securityRequirement);
                            });
            services.AddCors(opt => {
                opt.AddPolicy("CorsPolicy",policy=>{
                    policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4000");
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "EcommerceApi v1"));
            }

            // app.UseHttpsRedirection();

            app.UseRouting();
            app.UseStaticFiles();
            app.UseStaticFiles(
                new StaticFileOptions{
                    FileProvider = new PhysicalFileProvider(
                        Path.Combine(Directory.GetCurrentDirectory(),"wwwroot")
                    ),
                    RequestPath = "/content"
            });

            app.UseCors("CorsPolicy");

            app.UseAuthentication();
            app.UseAuthorization();

            var options = app.ApplicationServices
                .GetService<IOptions<RequestLocalizationOptions>>();

            app.UseRequestLocalization(options.Value);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapFallbackToController("Index","Fallback");
            });
            }
    }
}
