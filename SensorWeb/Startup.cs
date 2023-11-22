using Core;
using Core.Service;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SensorService;
using SensorWeb.Resources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace SensorWeb
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
            services.AddControllersWithViews().AddRazorRuntimeCompilation();

            //Injeção Dependencia DbContext 
            services.AddDbContext<SensorContext>(options => options.UseMySQL(Configuration.GetConnectionString("localdb")));

            //Injeção dependencia Services
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IUserTypeService, UserTypeService>();
            services.AddTransient<ICompanyService, CompanyService>();
            services.AddTransient<ICompanyTypeService, CompanyTypeService>();
            services.AddTransient<ICompanyUnitService, CompanyUnitService>();
            services.AddTransient<IContactService, ContactService>();
            services.AddTransient<ICompanyAlertContactService, CompanyAlertContactService>();
            services.AddTransient<IDeviceConfigurationService, DeviceConfigurationService>();
            services.AddTransient<IDeviceMeasureService, DeviceMeasureService>();
            services.AddTransient<IReceiveService, ReceiveService>();

            services.AddTransient<IFixationTypeService, FixationTypeService>();
            services.AddTransient<ICouplingTypeService, CouplingTypeService>();
            services.AddTransient<ICardanShaftTypeService, CardanShaftTypeService>();
            services.AddTransient<ICompressorTypeService, CompressorTypeService>();
            services.AddTransient<IPumpTypeService, PumpTypeService>();
            services.AddTransient<IMachineService, MachineService>();
            services.AddTransient<IDeviceService, DeviceService>();
            services.AddTransient<IMotorService, MotorService>();
            services.AddTransient<ICompanySubService, CompanySubService>();
            services.AddTransient<ICompanyUserService, CompanyUserService>();
            //Injeção dependencia Mappers
            services.AddAutoMapper(typeof(Startup).Assembly);

            services.AddRazorPages();

            services.AddMvc().AddDataAnnotationsLocalization(options =>
            {
                options.DataAnnotationLocalizerProvider = (type, factory) =>
                {
                    var assemblyName = new AssemblyName(typeof(CommonResources).GetTypeInfo().Assembly.FullName);
                    return factory.Create(nameof(CommonResources), assemblyName.Name);
                };
            });

            services.AddLocalization(options => options.ResourcesPath = "resources");

            services.Configure<RequestLocalizationOptions>(
                options =>
                {
                    var supportedCultures = new List<CultureInfo>
                    {
                    new CultureInfo("en-US"),
                    new CultureInfo("pt-BR"),
                    new CultureInfo("es-ES")
                    };

                    options.DefaultRequestCulture = new RequestCulture(culture: "en-US", uiCulture: "en-US");
                    options.SupportedCultures = supportedCultures;
                    options.SupportedUICultures = supportedCultures;
                });

            services.AddSingleton<CommonLocalizationService>();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Login";
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                });

            //services.Configure<IISOptions>(options =>
            //{
            //    options.ForwardClientCertificate = false;
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Index");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Login}/{action=Login}/{id?}");
            });

            loggerFactory.AddFile("Logs/log_{Date}.txt");

            var localizeOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();

            app.UseRequestLocalization(localizeOptions.Value);

            var localizationOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>().Value;
            app.UseRequestLocalization(localizationOptions);
        }
    }
}
