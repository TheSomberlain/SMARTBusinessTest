using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SMARTBusinessTest.Application.Constants;
using SMARTBusinessTest.Application.Interfaces;
using SMARTBusinessTest.Application.Services;
using SMARTBusinessTest.Infrastructure;
using SMARTBusinessTest.Web.Filters;

namespace SMARTBusinessTest.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddLogging();
            builder.Services.AddControllers(options =>
            {
                options.Filters.Add<ContractExceptionFilter>();
                options.Filters.Add<ApiAuthFilter>();
            });
            builder.Services.AddTransient<IPlacementContractService, PlacementContractService>();
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddScoped<ContractExceptionFilter>();
            builder.Services.AddScoped<ApiAuthFilter>();
            builder.Services.AddSwaggerGen(options =>
            {
                options.OperationFilter<SwaggerAuthFilter>();
                options.SupportNonNullableReferenceTypes();
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "SMARTBusinessTestAPI", Version = "v1" });
            });
            builder.Services.AddDbContext<EquipmentContractsDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString(DbConstants.ConnectionSetting)));
            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI( c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
                });
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}