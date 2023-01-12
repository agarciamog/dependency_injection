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

Manual use of DI with GetRequiredService
```
builder.Services.AddScoped<T>( provider => {
    var serviceA = provider.GetRequiredService<A>();
    var serviceB = provider.GetRequiredService<B>();
    
    return new T(serviceA, serviceB);
});
```

## Life Cycle
- Transient: created each time they are requested
- Scoped: created once per request
- Singleton: created once per service lifetime

