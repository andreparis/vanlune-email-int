using Email.Int.Infraestructure.Logging;
using Email.Int.Infraestructure.Security;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System;
using Email.Int.Infrastructure.Messaging.Email;
using Email.Int.Domain.Messaging.Email;

namespace Email.Int.Application.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services)
        {
            IConfigurationRoot configuration = GetConfiguration();
            services.AddSingleton<IConfiguration>(configuration);

#if DEBUG
            services.AddDefaultAWSOptions(configuration.GetAWSOptions());
#endif
            services.AddMediatR(AppDomain.CurrentDomain.Load("Email.Int.Application"));
            services.AddAutoMapper(typeof(Function).Assembly);
            services.AddSingleton<ILogger, Logger>();
            services.AddSingleton<IAwsSecretManagerService, AwsSecretManagerService>();

            services.AddSingleton<IEmailSender, EmailSender>();

            return services;
        }

        private static IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile($"appsettings.json")
                            .AddEnvironmentVariables();

            var configuration = builder.Build();
            return configuration;
        }
    }
}
