# Notes

## Composition vs Inheritance

### What Is the Difference Between Inheritance and Composition?
Inheritance is all about the specialization of a general concept. The derived class is a specialized version of the base class and promotes code reuse. It implicitly inherits all non-private members of its base classes, whether direct or indirect. It can also hide or override the inherited members.

On the other hand, the composition is about the association of objects of different classes. It enables code reuse by adding a reference to another object instead of inheriting the complete implementation.

### Composition Over Inheritance
The main problem with inheritance is that it may lead to a deep hierarchy of classes. This hierarchy is fragile, and the implementation of derived classes can break or be forced to change with any change at the top of the hierarchy.

## ServiceCollection
A list of service descriptors that describes a service with its service type, implementation, and lifetime.

## ServiceProvider
The dependency engine, it knows how to resolve registered services. Use `provider.GetRequiredService<T>()` over `provider.GetService<T>()`

1. Manual use of DI with GetRequiredService
```csharp
builder.Services.AddScoped<T>( provider => {
    var serviceA = provider.GetRequiredService<A>();
    var serviceB = provider.GetRequiredService<B>();
    
    return new T(serviceA, serviceB);
});
```
2. Get ServiceProvider using HttpContext (service locator anti-pattern)
```csharp
[HttpGet("weather")]
public IEnumerable<WeatherForecast> Get(string city)
{
    IServiceProvider provider = HttpContext.RequestServices;
    var myService = provider.GetRequiredService<IMyService>();

    return myService.GetWeather(city);
}
```
## Life Cycle
- Transient: created each time they are requested
- Scoped: created once per request
- Singleton: created once per service lifetime

## Registering Multiple Interface Implementations
If you don't implement IEnumerable in CTOR then you'll get the last registered service, InMemoryWeatherService.
```csharp
builder.Services.AddTransient<IWeatherService, OpenWeatherService>();
builder.Services.AddTransient<IWeatherService, InMemoryWeatherService>();
```
```csharp
private readonly IEnumerable<IWeatherService> _weatherServices

public MyClassCtor(IEnumerable<IWeatherService> weatherServices)
{
    _weatherServices = weatherServices;
    var firstWeatherService = weatherServices.First();
}
```

## The ServiceDescriptor
How a service should be registered with IoC container. Describes a service with its service type, implementation, and lifetime.
```csharp
builder.Services.AddTransient<IWeatherService, OpenWeatherService>();

// Without the syntatic sugar
var weatherServiceDescriptorA = new ServiceDescriptor
(
    typeof(IweatherService),
    typeof(OpenWeatherService),
    ServiceLifetime.Transient 
)
builder.Services.Add(weatherServiceDescriptorA);

var weatherServiceDescriptorB = 
    new ServiceDescriptor(
        typeof(IWeatherService), 
        provider => { 
            return new OpenWeatherService(provider.GetRequiredService<IHttpClientFactory>());
        },
        ServiceLifetime.Transient
    );
    
builder.Services.Add(weatherServiceDescriptorB);
```
## TryAdd
Add service, if one already exits then it is not added to IoC container

## TryAddEnumerable
Use TryAddEnumerable when registering a service implementation of a service type that supports multiple registrations of the same service type. Using Add is not idempotent and can add duplicate ServiceDescriptor instances if called twice. Using TryAddEnumerable will prevent registration of multiple implementation types.
```csharp
builder.Services.TryAddEnumerable(
    new []{ 
        openWeatherServiceDiscriptor,
        inMemoryWeatherServiceDescriptor
    }); 
```
## Creating Custom Scope
Define a custom scope in another thread or console app
```csharp
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
```

## Service Locator Anti-Pattern
A Service Locator supplies application components outside the Composition Root with access to an unbounded set of Dependencies. Basically, using a service locator rather than DI in constructor.

This anti-pattern makes it hard to tests and intent can't be easily understood from constructor, you'll have to look at source code.

> #### Assembly Scanning
> A pattern that describes the automatic registration of one or more dependencies within your application.
>
> Assembly scanning is very powerful when you need to bulk register your dependencies without having to explicitly register each instance. By following some convention, your code will be automatically connected with your application without any developer intervention. This can improve consistency in your application and improve developer productivity.
>
> See [Nick's example](https://nickchapsas.com/courses/1610935/lectures/36911120) and [this example](https://medium.com/pragmatic-programming/net-things-assembly-scanning-bd4330d9d0a9) on the Assembly Scanning pattern that uses the Service Locator Anti-Pattern.

## Creating Wrapper-Decorators with DI
This example creates a wrapper decorator around a service for adding benchmarking.

`LoggedWeatherSevice` will get injected by controller, and `OpenWeatherService` will be injected in `LoggedWeatherService`.
```csharp
builder.Services.AddTransient<OpenWeatherService>();
builder.Services.AddTransient<IWeatherService>(provider =>
{
    return new LoggedWeatherService(
        provider.GetRequiredService<OpenWeatherService>(),
        provider.GetRequiredService<ILogger<IWeatherService>>()
    );
});
```

```csharp
public class LoggedWeatherService : IWeatherService
{
    private readonly IWeatherService _weatherService;
    private readonly ILogger<IWeatherService> _logger;
    
    public LoggedWeatherService(IWeatherService weatherService, 
        ILogger<IWeatherService> logger)
    {
        _weatherService = weatherService;
        _logger = logger;
    }
    public async Task<WeatherResponse?> GetCurrentWeatherAsync(string city)
    {
        var sw = Stopwatch.StartNew();
        try
        {
            return await _weatherService.GetCurrentWeatherAsync(city);
        }
        finally
        {
            sw.Stop();
            _logger.LogInformation(
                "Time elapsed: {0}ms", sw.ElapsedMilliseconds);
        }
    }
}
```

## 