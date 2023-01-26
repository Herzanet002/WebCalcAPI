using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using WebCalcAPI.Contracts.Services;
using WebCalcAPI.Middleware;
using WebCalcAPI.Models;
using WebCalcAPI.Models.Users;
using WebCalcAPI.Services;

namespace WebCalcAPI;

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
        services.AddSwaggerGen(option =>
        {
            var securityDefinition = new OpenApiSecurityScheme
            {
                Name = "Bearer",
                BearerFormat = "JWT",
                Scheme = "bearer",
                Description = "Specify the authorization token",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http
            };
            option.AddSecurityDefinition("jwt_auth", securityDefinition);

            // Make sure swagger UI requires a Bearer token specified
            var securityScheme = new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "jwt_auth",
                    Type = ReferenceType.SecurityScheme
                }
            };
            var securityRequirements = new OpenApiSecurityRequirement
            {
                {securityScheme, Array.Empty<string>()}
            };
            option.AddSecurityRequirement(securityRequirements);
        });
        services.AddControllersWithViews();

        services.Configure<List<UserModel>>(
            Configuration.GetSection("ValidUsers"));
        services.Configure<JwtOptions>(
            Configuration.GetSection("JwtOptions"));

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["JwtOptions:Issuer"],
                    ValidAudience = Configuration["JwtOptions:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtOptions:Key"]))
                };
            });

        services.AddSingleton<ICalculationService, CalculationService>();
        services
            .AddSingleton<IAsyncReplyRequestService<CalculationResultModel>,
                AsyncReplyRequestService<CalculationResultModel>>();
        services.AddSingleton<IAuthenticateService, AuthenticateService>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json",
                    $"My API v{Assembly.GetExecutingAssembly().GetName().Version}");
            });
        }

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseMiddleware<LoggerMiddleware>();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapGet("/", async context => { await context.Response.WriteAsync("Hello World!"); });
        });
    }
}