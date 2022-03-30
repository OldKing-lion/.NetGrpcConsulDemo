using Microsoft.Extensions.Options;

namespace GrpcConsulDemo
{
    public class ConsulServiceOption:IOptions<ConsulServiceOption>
    {
        public ConsulServiceOption()
        {
        }
        public string ConsulUrl { get; set; } = "127.0.0.1:8500";
        public string HealthCheckHost { get; set; } = "http://localhost:5192";
        public bool Usetls { get; set; } = false;
        public int TimeOutSeconds { get; set; } = 10;
        public int IntevalSeconds { get; set; } = 10;
        public string ServiceName { get; set; } = string.Empty;
        public string ServiceHost { get; set; } = string.Empty;
        public int ServicePort { get; set; }
        public List<string> Tags { get; set; } = new List<string>();

        ConsulServiceOption IOptions<ConsulServiceOption>.Value => this;
    }
}
