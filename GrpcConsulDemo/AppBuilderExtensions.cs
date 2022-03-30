using Consul;

namespace GrpcConsulDemo
{
    public static class AppBuilderExtensions
    {
        public static IApplicationBuilder RegisterConsul(this IApplicationBuilder app, IConfiguration configuration, IHostApplicationLifetime lifetime)
        {
            var options = configuration.Get<ConsulServiceOption>();
            var consul = new ConsulClient(x =>
            {
                x.Address = new Uri(options.ConsulUrl);
            });


            var registration = new AgentServiceRegistration
            {
                ID = Guid.NewGuid().ToString(),//服务实例唯一标识
                Name = options.ServiceName,//服务名称
                Address = options.ServiceHost,//服务IP地址
                Port = options.ServicePort,
                Check = new AgentServiceCheck//健康检查
                {
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),//有人说这个是标识服务启动多久后注册，但是字面意思好像是取消注册,应该是健康检查多久没通过取消注册
                    Interval = TimeSpan.FromSeconds(5),//间隔时间
                    Timeout = TimeSpan.FromSeconds(5),//超时时间
                    GRPC = options.HealthCheckHost,
                    GRPCUseTLS = options.Usetls
                }
            };

            consul.Agent.ServiceRegister(registration);//服务注册
            //程序终止取消注册
            lifetime.ApplicationStopping.Register(() =>
            {
                consul.Agent.ServiceDeregister(registration.ID).Wait();
            });
            return app;
        }
    }
}
