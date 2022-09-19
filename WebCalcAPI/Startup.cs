using Microsoft.AspNetCore.Authentication.JwtBearer;
using WebCalcAPI.Contracts.Services;
using WebCalcAPI.Middleware;
using WebCalcAPI.Models;
using WebCalcAPI.Services;

namespace WebCalcAPI
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
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddControllersWithViews();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;

            });
            services.AddSingleton<ICalculationService, CalculationService>();
            services.AddSingleton<IAsyncReplyRequestService<CalculationModel>, AsyncReplyRequestService<CalculationModel>>();
        }

        //requestpipeline
        //middleware
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Configure the HTTP request pipeline.
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseMiddleware<LoggerMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
                //endpoints.Map("/api/requestStatus/{uniqId}", async context => {
                //    context.Response.ContentType = "text/html; charset=utf-8";
                //    await context.Response.WriteAsync($"{context.Request}");
                //});
            });


        }
    }
}
