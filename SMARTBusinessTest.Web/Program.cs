using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SMARTBusinessTest.Application.Constants;
using SMARTBusinessTest.Application.Interfaces;
using SMARTBusinessTest.Application.Services;
using SMARTBusinessTest.Infrastructure;
using SMARTBusinessTest.Web.Filters;
using Microsoft.Extensions.Logging.AzureAppServices;
using Microsoft.Extensions.Logging.ApplicationInsights;

namespace SMARTBusinessTest.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Logging.AddAzureWebAppDiagnostics();
            builder.Services.Configure<AzureFileLoggerOptions>(options =>
            {
                options.FileName = "logs-";
                options.FileSizeLimit = 50 * 1024;
                options.RetainedFileCountLimit = 5;
            });
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

            app.UseSwagger();
            app.UseSwaggerUI( c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
            });

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}