using CinemaTicketServer.Classes;
using CinemaTicketServer.Services;
using CoreWCF;
using CoreWCF.Channels;
using CoreWCF.Configuration;
using CoreWCF.Description;
using CoreWCF.Dispatcher;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Web.Services.Description;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CinemaTicketServer
{
    public class ServerMain
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.WebHost.ConfigureKestrel(options =>
            {
                options.AllowSynchronousIO = true;

                options.Listen(IPAddress.Loopback, 8080);
                options.Listen(IPAddress.Parse("192.168.19.154"), 8081, listenOptions =>
                {
                    listenOptions.UseHttps("cert.pfx", "haslo_do_cert");
                });
            });


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

            app.UseCors("AllowAllOrigins");
            app.UseRouting();

            // HTTP binding without security
            var httpBinding = new BasicHttpBinding(BasicHttpSecurityMode.None)
            {
                MessageEncoding = WSMessageEncoding.Text
            };

            // HTTPS binding with security
            var httpsBinding = new BasicHttpBinding(BasicHttpSecurityMode.Transport)
            {
                MessageEncoding = WSMessageEncoding.Text
            };

            app.UseServiceModel(serviceBuilder =>
            {
                serviceBuilder.AddService<DatabaseService>();
                serviceBuilder.AddServiceEndpoint<DatabaseService, IDatabaseService>(httpsBinding, "/DatabaseService");
                serviceBuilder.AddServiceEndpoint<DatabaseService, IDatabaseService>(httpBinding, "/DatabaseService");

                serviceBuilder.AddService<ReservationService>();
                serviceBuilder.AddServiceEndpoint<ReservationService, IReservationService>(httpsBinding, "/ReservationService");
                serviceBuilder.AddServiceEndpoint<ReservationService, IReservationService>(httpBinding, "/ReservationService");

                var metadataBehavior = app.Services.GetRequiredService<ServiceMetadataBehavior>();
                metadataBehavior.HttpGetEnabled = true;
                metadataBehavior.HttpsGetEnabled = true;
                metadataBehavior.HttpGetUrl = new Uri("http://localhost:8080");
                metadataBehavior.HttpsGetUrl = new Uri("https://192.168.19.154:8081");
            });

            app.Run();
        }
    }
}