using Microsoft.AspNetCore.Builder;
using WebCalcAPI.Contracts.Services;
using WebCalcAPI.Middleware;
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
            services.AddSingleton<ICalculationService, CalculationService>();
            services.AddSingleton<IAsyncReplyRequestService, AsyncReplyRequestService>();
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
                endpoints.MapControllers());
            
        }
    }
}
