using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System;
using System.Net.Sockets;

namespace Iris.Crosscutting.Infrastructure.Contexts.MongoDb
{
    public static class MongoDbExtension
    {
        public static void AddMongoDbContext(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var config = new MongoDbContextConfig();

            configuration.Bind("MongoDb", config);

            if (string.IsNullOrEmpty(config.ConnectionString)) throw new Exception("MongoDb connection is empty.");

            services.AddSingleton(config);
            services.AddSingleton(t =>
            {
                var settings = MongoClientSettings.FromConnectionString(config.ConnectionString);

                void SocketConfigurator(Socket s) => s.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);

                settings.SocketTimeout = TimeSpan.FromMinutes(5);
                settings.ConnectTimeout = TimeSpan.FromSeconds(60);
                settings.MaxConnectionIdleTime = TimeSpan.FromSeconds(30);
                settings.ClusterConfigurator = builder => builder
                    .ConfigureTcp(tcp => tcp.With(socketConfigurator: (Action<Socket>)SocketConfigurator));

                return new MongoClient(settings);
            });

            services.AddScoped<IMongoDbContext, MongoDbContext>();
        }
    }
}