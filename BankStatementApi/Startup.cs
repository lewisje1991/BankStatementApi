using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using BankStatementApi.Services;
using BankStatementApi.Factories;
using BankStatementApi.Repositories;
using BankStatementApi.Models;

namespace BankStatementApi
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
            services.AddDbContext<BankStatementApiContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddMvc();

            //Factories
            services.AddTransient<CsvReaderFactory>();

            //Servcies
            services.AddTransient<ITransactionService, TransactionService>();
            services.AddTransient<ICsvService, CsvService>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<ITransactionReportService, TransactionReportService>();

            //Repos
            services.AddTransient<ITransactionRepository, TransactionRepository>();
            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
