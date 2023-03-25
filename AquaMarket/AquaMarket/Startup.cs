using AquaServer.Domain.Account.Actions.Helper;
using AquaServer.Domain.Account.Actions.Operations;
using AquaServer.Domain.Account.Factory;
using AquaServer.Domain.Database;
using AquaServer.Domain.Market.Cart;
using AquaServer.Domain.Market.Catalog;
using AquaServer.Domain.Market.Catalog.Filters;
using AquaServer.Extensions;
using AquaServer.Extensions.Helper;
using AquaServer.Extensions.Mappers;
using AquaServer.Extensions.Wrapper;
using AquaServer.Interfaces.Database;
using AquaServer.Interfaces.Services;
using AquaServer.Interfaces.Services.Account;
using AquaServer.Interfaces.Services.Market;
using AquaServer.Interfaces.Wrapper;
using AquaServer.Middleware;
using AquaServer.Services.Account;
using AquaServer.Services.Account.Helper;
using AquaServer.Services.EmployeeRoleActions;
using AquaServer.Services.General;
using AquaServer.Services.Market;
using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace AquaServer
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
            services.AddSingleton<WWWRootResources>();
            services.AddSingleton(_ => Configuration);
            services.AddControllers().AddNewtonsoftJson();
            services.AddRazorPages();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AquaServer", Version = "v1" });
            });

            services.AddDbContext<AquamContext>(options =>
            {
                //options.UseInMemoryDatabase("aquaM");
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
                options.EnableDetailedErrors();
                options.LogTo(Console.WriteLine);
            });

            services.AddHttpContextAccessor();

            services.AddNotyf(config => { config.DurationInSeconds = 10; config.IsDismissable = true; config.Position = NotyfPosition.TopRight; });

            services.AddTransient<UrlBuilder>();
            services.AddTransient(typeof(Map<,>));
            services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
            services.AddTransient(c => c.GetService<IHttpContextAccessor>().HttpContext.User);
            services.AddTransient(typeof(ICurrentUser), typeof(CurrentUser));
            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            services.AddTransient(typeof(IMessagingService), typeof(EmailService));
            services.AddTransient<IPasswordService, PasswordService>();

            services.AddScoped(typeof(IHttpContextWrapper), typeof(HttpContextWrapper));
            services.AddScoped<CookieService>();
            services.AddScoped<AuthTokenService>();
            services.AddScoped<Map>();

            //domain
            services.AddScoped(typeof(IOrderManagement), typeof(OrderManager));
            services.AddScoped(typeof(IOrderViewer), typeof(OrderViewer));

            services.AddScoped(typeof(ICartViewer), typeof(OrderViewer));
            services.AddScoped(typeof(IClientOrderManagement), typeof(OrderManager));
            services.AddScoped(typeof(IOrderManagement), typeof(OrderManager));

            services.AddScoped(typeof(IBaseAccountOperation), typeof(BaseAccountOperation));
            services.AddScoped(typeof(IEmployeeAccountOperation), typeof(EmployeeAccountOperation));
            services.AddScoped(typeof(IClientActionOperation), typeof(ClientAccountOperation));
            services.AddScoped<AccountOperationFactory>();

            services.AddScoped<CartService>();
            services.AddScoped(typeof(MarketFilterService<>));
            services.AddScoped<CatalogueViewer>();
            services.AddScoped<OrderService>();
            services.AddScoped<ManagerService>();
            services.AddScoped<StockmanService>();
            services.AddScoped<AccountService>();

            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.PageViewLocationFormats.Add("/Pages/Partial/{0}" + RazorViewEngine.ViewExtension);
            });

            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder(CookieAuthenticationDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser().Build();
            }).AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme =
                options.DefaultSignInScheme =
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;

            }).AddCookie(options =>
               {
                   options.SlidingExpiration = true;
                   options.ExpireTimeSpan = TimeSpan.FromHours(9);
               });


            services.AddCors();
            services.AddResponseCaching();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AquaServer v1"));
            }

            app.UseMiddleware<ExceptionHandlerMiddleware>();
            app.UseRewriter();
            app.UseRouting();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseNotyf();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCookiePolicy(
                new()
                {
                    MinimumSameSitePolicy = SameSiteMode.None,
                    Secure = CookieSecurePolicy.Always,
                    HttpOnly = HttpOnlyPolicy.Always
                });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Catalogue}/{id?}"
                    );
                endpoints.MapRazorPages();
            });
            app.UseResponseCaching();
        }
    }
}
