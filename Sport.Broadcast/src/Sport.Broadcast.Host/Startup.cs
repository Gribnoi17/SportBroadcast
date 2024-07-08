using Microsoft.AspNetCore.Mvc;
using Sport.Broadcast.AppServices.Infrastructure.Extensions;
using Sport.Broadcast.DataAccess.Infrastructure.Extensions;
using Sport.Broadcast.Host.Broadcasts.Hubs;
using Sport.Broadcast.Host.Infrastructure.Extensions;
using Sport.Broadcast.Host.Infrastructure.Middleware;

namespace Sport.Broadcast.Host
{
    public class Startup
    {
        private IConfiguration Configuration { get; }
        private IWebHostEnvironment WebHostEnvironment { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            Configuration = configuration;
            WebHostEnvironment = webHostEnvironment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHealthChecks();

            services.AddHandlers();
            
            services.AddRepository();

            services.AddSignalR();

            services.AddValidators();
            
            services.AddLoggerService(Configuration);

            services.AddSportBroadcastDbContext(Configuration);

            services.AddSwaggerGen();

            services.AddMiddlewareService(Configuration);

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI();

            app.UseExceptionHandler("/Error");

            app.UseHsts();

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();
            
            app.UseMiddleware<HeaderMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/health");

                endpoints.MapGet("/", () => "Hello World. I am Danil!");

                endpoints.MapHub<BroadcastHub>("/broadcast");
                
                endpoints.MapControllers();
            });
        }
    }
}