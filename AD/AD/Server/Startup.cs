using AutoMapper;
using Hangfire;
using Hangfire.MemoryStorage;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using AD.Client.Repositories.Interfaces;
using AD.Server.Data;
using AD.Server.Helpers;
using AD.Server.Models;
using AD.Server.Repositories;
using AD.Server.Services;
using AD.Server.Services.Interfaces;
using Serilog.Context;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AD.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options => 
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(Configuration["jwt:key"])),
                ClockSkew = TimeSpan.Zero
            });

            //services.AddAuthorization();

            services.AddSwaggerGen(c =>
            {
                //First we define the security scheme
                c.AddSecurityDefinition("Bearer", //Name the security scheme
                    new OpenApiSecurityScheme
                    {
                        Description = "JWT Authorization header using the Bearer scheme.",
                        Type = SecuritySchemeType.Http, //We set the scheme type to http since we're using bearer authentication
                        Scheme = "bearer" //The name of the HTTP Authorization scheme to be used in the Authorization header. In this case "bearer".
                    });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement{
                {
                     new OpenApiSecurityScheme{
                         Reference = new OpenApiReference{
                             Id = "Bearer", //The name of the previously defined security scheme.
                             Type = ReferenceType.SecurityScheme
                     }},new List<string>()}
                 });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var filePath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(filePath);
            });

            //services.AddHangfire(c => c.UseMemoryStorage());
            services.AddHangfire(configuration => configuration
                       .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                       .UseSimpleAssemblyNameTypeSerializer()
                       .UseRecommendedSerializerSettings()
                       .UseSqlServerStorage(Configuration.GetConnectionString("DefaultConnection"), new SqlServerStorageOptions
                       {
                           CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                           SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                           QueuePollInterval = TimeSpan.Zero,
                           UseRecommendedIsolationLevel = true,
                           DisableGlobalLocks = true
                       }));
            services.AddHangfireServer();

            services.AddAutoMapper(typeof(Startup));
            services.AddHttpContextAccessor();
            services.AddScoped<IFileStorageService, InAppStorageService>();

            services.AddScoped(typeof(IRepository<>), typeof(EntityFrameworkRepository<>));
            services.AddScoped<IUserServices, UserServices>();
            services.AddScoped<ITagServices, TagServices>();
            services.AddScoped<ICurrencyServices, CurrencyServices>();
            services.AddScoped<ICountryServices, CountryServices>();
            services.AddScoped<ICityServices, CityServices>();
            services.AddScoped<ICategoryServices, CategoryServices>();
            services.AddScoped<IUserCServices, UserCServices>();
            services.AddScoped<IStoreServices, StoreServices>();
            services.AddScoped<IHomeServices, HomeServices>();
            services.AddScoped<IErrorLogsServices, ErrorLogsServices>();
            services.AddScoped<IAuditServices, AuditServices>();
            services.AddScoped<IStoreUserServices, StoreUserServices>();
            services.AddScoped<IMeasruingUnitServices, MeasruingUnitServices>();
            services.AddScoped<IProductServices, ProductServices>();
            services.AddScoped<IFavouritesServices, FavouritesServices>();
            services.AddScoped<IOrderSrevices, OrderServices>();
            services.AddScoped<IMessageServices, MessageServices>();
            services.AddScoped<IRoleServices, RoleServices>();
            services.AddScoped<IPaymentMethodServices, PaymentMethodServices>();
            services.AddScoped<INotificationServices, NotificationServices>();

            services.AddMvc().AddNewtonsoftJson(Options => Options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddControllersWithViews();
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
            IWebHostEnvironment env,
            IBackgroundJobClient backgroundJobClient,
            IRecurringJobManager recurringJobManager,
            IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseCustomApiExceptionHandler();

            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.Use(async (httpContext, next) =>
            {
                var username = httpContext.User.Identity.IsAuthenticated ? httpContext.User.Identity.Name : "anonymous";

                LogContext.PushProperty("UserName", username);

                await next.Invoke();
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
                endpoints.MapHangfireDashboard();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "APICurrency v1"));

            CreateRoles(serviceProvider).Wait();

            app.UseHangfireDashboard();
            backgroundJobClient.Enqueue(() => serviceProvider.GetService<IHomeServices>().CurrencyService());
            backgroundJobClient.Enqueue(() => serviceProvider.GetService<IHomeServices>().SendNotificationsConfirmToReceivedOrders());

            //recurringJobManager.AddOrUpdate(
            //    "CurrencyService",
            //    () => serviceProvider.GetService<IHomeServices>().CurrencyService(),
            //    "* * * * *"
            //    );
        }
        private static async Task CreateRoles(IServiceProvider serviceProvider)
        {
            //initializing custom roles   
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            string[] roleNames = { "Admin", "User" };
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = await RoleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    //create the roles and seed them to the database: Question 1  
                    roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            IdentityUser user = await UserManager.FindByEmailAsync("Admin");

            if (user == null)
            {
                user = new IdentityUser()
                {
                    UserName = "Admin",
                    Email = "Admin",
                    PhoneNumber = "0941425818"
                };
                await UserManager.CreateAsync(user, "P@ssw0rd");
            }
            await UserManager.AddToRoleAsync(user, "Admin");
        }
    }
}
