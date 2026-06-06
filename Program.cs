using LotTrace_MES.src.Application.Interfaces;
using LotTrace_MES.src.Application.Service;
using LotTrace_MES.src.Domain.Interfaces;
using LotTrace_MES.src.Infrastructure;
using LotTrace_MES.src.Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Text;

namespace LotTrace_MES
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllers();

            // JWT 인증 설정 (서버 보안 로직은 그대로 유지)
            var jwtSettings = builder.Configuration.GetSection("Jwt");
            var keyString = jwtSettings["Key"];
            var issuer = jwtSettings["Issuer"];
            var audience = jwtSettings["Audience"];

            if (string.IsNullOrEmpty(keyString))
            {
                throw new InvalidOperationException("Fatal Error: JWT signing key is missing in configuration!");
            }

            if (string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience))
            {
                throw new InvalidOperationException("Fatal Error: JWT issuer or audience is missing in configuration (appsettings.json)!");
            }


            var key = Encoding.ASCII.GetBytes(keyString);

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidateAudience = true,
                    ValidAudience = jwtSettings["Audience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });
            builder.Services.AddAuthorization();

            // DB 설정
            builder.Services.AddDbContext<AppDbContext>(opt =>
                opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Repository DI 등록
            builder.Services.AddScoped<ILineRepository, LineRepository>();
            builder.Services.AddScoped<IWorkerRepository, WorkerRepository>();
            builder.Services.AddScoped<ILotRepository, LotRepository>();
            builder.Services.AddScoped<ILogHistoriesRepository, LogHistoriesRepository>();
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IMaterialRepository, MaterialRepository>();
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();

            // Service DI 등록
            builder.Services.AddScoped<ILineService, LineService>();
            builder.Services.AddScoped<IWorkerService, WorkerService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<ILogHistoriesService, LogHistoriesService>();
            builder.Services.AddScoped<ILotService, LotService>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IMaterialService, MaterialService>();
            builder.Services.AddScoped<IOrderService, OrderService>();

            builder.Services.AddOpenApi(options =>
            {
                options.AddDocumentTransformer((document, context, cancellationToken) =>
                {
                    document.Info.Title = "LotTrace MES API";
                    document.Info.Version = "v1";
                    document.Info.Description = ".NET 10 Scalar API Reference (Authentication UI Disabled)";
                    
                    return Task.CompletedTask;
                });
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                
                app.MapScalarApiReference(options =>
                {
                    options.WithTitle("LotTrace MES API")
                           .WithTheme(ScalarTheme.Moon);
                });
            }

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
