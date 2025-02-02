using NamedPipesServer;
using System.Reflection;


var moduleAssemblies = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "Module*.dll")
    .Select(Assembly.LoadFrom)
    .ToArray();


var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));

var host = builder.Build();
host.Run();