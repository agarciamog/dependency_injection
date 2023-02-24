using CustomScope.ConsoleApp;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();

services.AddScoped<ExampleService>();

var serviceProvider = services.BuildServiceProvider();

// Use IServiceScopeFactory
var serviceScopeFactory = serviceProvider.GetRequiredService<IServiceScopeFactory>();

using (var serviceScope = serviceScopeFactory.CreateScope())
{
   var service1 = serviceScope.ServiceProvider.GetRequiredService<ExampleService>();
   Console.WriteLine(service1.Id);
}

using (var serviceScope = serviceScopeFactory.CreateScope())
{
   var service2 = serviceScope.ServiceProvider.GetRequiredService<ExampleService>();
   Console.WriteLine(service2.Id);
}