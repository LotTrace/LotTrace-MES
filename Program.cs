using LotTrace_MES.src.Application.Interfaces;
using LotTrace_MES.src.Application.Service;
using LotTrace_MES.src.Domain.Interfaces;
using LotTrace_MES.src.Infrastructure;
using LotTrace_MES.src.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LotTrace_MES
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllers();
            // DB 설정
            builder.Services.AddDbContext<AppDbContext>(opt =>
                opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Repository DI 등록
            builder.Services.AddScoped<ILineRepository, LineRepository>();
            builder.Services.AddScoped<IWorkerRepository, WorkerRepository>();
            builder.Services.AddScoped<ILotRepository, LotRepository>();
            builder.Services.AddScoped<ILogHistoriesRepository, LogHistoriesRepository>();
            builder.Services.AddScoped<IProductRepository, ProductRepository>();

            // Service DI 등록
            builder.Services.AddScoped<ILineService, LineService>();
            builder.Services.AddScoped<IWorkerService, WorkerService>();
            builder.Services.AddScoped<ILogHistoriesService, LogHistoriesService>();
            builder.Services.AddScoped<ILotService, LotService>();
            builder.Services.AddScoped<IProductService, ProductService>();

            // Swagger 설정
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            if(app.Environment.IsDevelopment()) // 개발 환경에서만 사용 가능하게 설정
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.MapControllers();


            app.Run();
        }
    }
}
