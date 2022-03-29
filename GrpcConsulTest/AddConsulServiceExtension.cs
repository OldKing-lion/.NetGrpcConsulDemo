using NConsul.AspNetCore;
namespace GrpcConsulTest
{
    public static class AddConsulServiceExtension
    {
        public static IServiceCollection AddConsulService(this IServiceCollection services, IConfiguration configuration)
        {
            var options = configuration.Get<ConsulServiceOption>();
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
            services.AddConsul(options.ConsulUrl)
                .AddGRPCHealthCheck(options.HealthCheckHost,
                    options.Usetls, options.TimeOutSeconds,
                    options.IntevalSeconds)
                .RegisterService(options.ServiceName,
                    options.ServiceHost,
                    options.ServicePort,
                    options.Tags.ToArray());
            return services;
        }
    }
}
