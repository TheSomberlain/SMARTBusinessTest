using Microsoft.EntityFrameworkCore;
using SMARTBusinessTest.Application.Filters;
using SMARTBusinessTest.Application.Interfaces;
using SMARTBusinessTest.Application.Services;
using SMARTBusinessTest.Data;

namespace SMARTBusinessTest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddLogging();
            builder.Services.AddControllers(options =>
            {
                options.Filters.Add<ContractExceptionFilter>();
            });
            builder.Services.AddTransient<IPlacementContractService, PlacementContractService>();
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddScoped<ContractExceptionFilter>();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<EquipmentContractsDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}