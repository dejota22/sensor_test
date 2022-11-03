using Core;
using Core.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SensorApi.Interfaces;
using SensorService;
using System.Text;
using Unity;

namespace SensorApi
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
            services.AddControllers();

            //Injeção Dependencia DbContext 
            services.AddDbContext<SensorContext>(options => options.UseMySQL(Configuration.GetConnectionString("localdb")));

            //Injeção dependencia Services            
            services.AddTransient<IDeviceMeasureService, DeviceMeasureService>();
            services.AddTransient<IMotorService, MotorService>();
            services.AddTransient<IDeviceService, DeviceService>();
            services.AddTransient<IUserService, UserService>();

            services.AddTransient<IFixationTypeService, FixationTypeService>();
            services.AddTransient<ICouplingTypeService, CouplingTypeService>();
            services.AddTransient<ICardanShaftTypeService, CardanShaftTypeService>();
            services.AddTransient<ICompressorTypeService, CompressorTypeService>();
            services.AddTransient<IPumpTypeService, PumpTypeService>();
            services.AddTransient<IMachineService, MachineService>();            

            //Injeção dependencia Mappers
            services.AddAutoMapper(typeof(Startup).Assembly);

            //Authentication Token 
            var key = "3FJRhIppOMZ3Z0MkKsBfiej4M9Ms1j5k";
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key))
                };
            });
            
            UnityContainer container = new UnityContainer();
            container.RegisterType<IUserService, UserService>();
            UserService _userService = container.Resolve<UserService>();            

            services.AddSingleton<IJwtAuth>(new Auth(key, _userService));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "IOTNetWebAPI", Version = "v1" });
            });

            services.AddRazorPages();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "IOTNetWebAPI v1"));
            //}

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //loggerFactory.AddFile("Logs/log_{Date}.txt");
        }
    }
}
