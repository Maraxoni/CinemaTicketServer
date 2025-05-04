using CinemaTicketServer.Classes;
using CinemaTicketServer.Services;
using CoreWCF;
using CoreWCF.Configuration;
using CoreWCF.Description;
using CoreWCF.Dispatcher;
using Microsoft.Extensions.DependencyInjection;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CinemaTicketServer
{
    public class ServerMain
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins", policy =>
                {
                    policy.AllowAnyOrigin()  // Allows all origins
                          .AllowAnyMethod()  // Allows any HTTP method (GET, POST, etc.)
                          .AllowAnyHeader(); // Allows any header
                });
            });

            builder.Services.AddSingleton<IDatabaseService, DatabaseService>();
            builder.Services.AddSingleton<ReservationService>();
            builder.Services.AddSingleton<IDispatchMessageInspector, ServerHandlerInspector>();
            builder.Services.AddSingleton<IServiceBehavior, GlobalInspectorBehavior>();
            builder.Services.AddServiceModelServices();
            builder.Services.AddServiceModelMetadata();


            var app = builder.Build();

            //app.Urls.Add("http://192.168.50.183:8080");
            app.Urls.Add("http://localhost:8080");

            app.UseCors("AllowAllOrigins");
            app.UseRouting();

            var binding = new BasicHttpBinding
            {
                MessageEncoding = WSMessageEncoding.Text
            };

            app.UseServiceModel(serviceBuilder =>
            {
                serviceBuilder.AddService<DatabaseService>();
                serviceBuilder.AddServiceEndpoint<DatabaseService, IDatabaseService>(
                    binding, "/DatabaseService");
                serviceBuilder.AddService<ReservationService>();
                serviceBuilder.AddServiceEndpoint<ReservationService, IReservationService>(
                    binding, "/ReservationService");

             
                var metadataBehavior = app.Services.GetRequiredService<ServiceMetadataBehavior>();
                metadataBehavior.HttpGetEnabled = true;
                metadataBehavior.HttpGetUrl = new Uri("http://localhost:8080/DatabaseService/mex");
            });


            app.Run();
        }
    }
}