// See https://aka.ms/new-console-template for more information
using Consul;
using Grpc.Net.Client;
using GrpcConsulTest;

var serviceName = "Test";
var consulClient = new ConsulClient(c => c.Address = new Uri("http://localhost:8500"));
var services = await consulClient.Catalog.Service(serviceName);
if (services.Response.Length == 0)
{
    throw new Exception($"未发现服务 {serviceName}");
}

var service = services.Response[0];
var address = $"http://{service.ServiceAddress}:{service.ServicePort}";

Console.WriteLine($"获取服务地址成功：{address}");

//启用通过http使用http2.0
AppContext.SetSwitch(
    "System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
var channel = GrpcChannel.ForAddress(address);
var greeterClient = new Greeter.GreeterClient(channel);
var greeterReply = await greeterClient.SayHelloAsync(new HelloRequest { Name="HH"});
Console.WriteLine("调用打招呼服务：" + greeterReply.Message);
Console.ReadKey();