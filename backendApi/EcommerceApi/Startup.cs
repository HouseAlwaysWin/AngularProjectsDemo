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
using AutoMapper;
using EcommerceApi.Core.Data;
using EcommerceApi.Core.Data.Repositories;
using EcommerceApi.Core.Data.Repositories.Interfaces;
using StackExchange.Redis;
using Microsoft.AspNetCore.Http;

namespace EcommerceApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            _config = configuration;
        }

        public IConfiguration _config { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AutomapperProfiles));
            services.AddControllers();
            services.AddDbContext<ECIdentityDbContext>(x =>
                x.UseSqlServer(_config.GetConnectionString("IdentityConnection")));
            
            services.AddDbContext<StoreContext>(x => 
                x.UseSqlServer(_config.GetConnectionString("DefaultConnection")));
            
            services.AddSingleton<IConnectionMultiplexer>(r =>{
                var redisConfig = ConfigurationOptions.Parse(
                    _config.GetConnectionString("Redis")
                ,true);
                return ConnectionMultiplexer.Connect(redisConfig);
            });
            
            services.AddIdentityCore<ECUser>()
                // .AddErrorDescriber<LocalizedIdentityErrorDescriber>()
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

            
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>{
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters{
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Token:Key"])),
                        ValidIssuer = _config["Token:Issuer"],
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

            services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(_config.GetConnectionString("redis")));
            services.AddScoped<ITokenService,TokenService>();
            services.AddScoped<IProductRepository,ProductRepository>();
            services.AddScoped<IProductService,ProductService>();
            services.AddScoped<IOrderService,OrderService>();
            services.AddScoped<IPaymentService,PaymentService>();
            services.AddScoped(typeof(IGenericRepository<>),(typeof(GenericRepository<>)));
            services.AddScoped<IUnitOfWork,UnitOfWork>();
            services.AddScoped<IBasketRepository,BasketRepository>();

            services.AddScoped<IRedisCachedService,RedisCachedService>();
            services.AddScoped<ILanguageService,LanguageService>();
            services.AddSingleton<IHttpContextAccessor,HttpContextAccessor>();
            services.AddScoped<ILocalizedService,LocalizedService>();


            services.AddSwaggerGen();
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

            app.UseCors("CorsPolicy");

            app.UseAuthentication();
            app.UseAuthorization();

            var options = app.ApplicationServices
                .GetService<IOptions<RequestLocalizationOptions>>();

            app.UseRequestLocalization(options.Value);


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
