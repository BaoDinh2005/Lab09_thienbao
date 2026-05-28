using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Lab09_thienbao.Data;

namespace Lab09_thienbao
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // 1. NƠI CẤU HÌNH SERVICES (KẾT NỐI DB, CORS, SWAGGER)
        public void ConfigureServices(IServiceCollection services)
        {
            // Cấu hình kết nối SQL Server
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            // Cấu hình CORS cho phép Frontend gọi AJAX
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            services.AddControllers();

            // Cấu hình Swagger để kiểm tra API
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Lab09_thienbao API", Version = "v1" });
            });
        }

        // 2. NƠI CẤU HÌNH HTTP REQUEST PIPELINE (MIDDLEWARE)
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                // Kích hoạt Swagger khi chạy debug
                app.UseSwagger();
                app.UseDefaultFiles(); 
                app.UseStaticFiles();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Lab09_thienbao API v1"));
            }

            app.UseRouting();

            // KÍCH HOẠT CORS (Bắt buộc phải đặt trước UseAuthorization)
            app.UseCors("AllowAll");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}