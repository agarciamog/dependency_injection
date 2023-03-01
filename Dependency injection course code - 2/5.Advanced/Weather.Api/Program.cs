using Weather.Api.Weather;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient();

builder.Services.AddTransient<OpenWeatherService>();
builder.Services.AddTransient<IWeatherService>(provider =>
{
    return new LoggedWeatherService(
        provider.GetRequiredService<OpenWeatherService>(),
        provider.GetRequiredService<ILogger<IWeatherService>>()
    );
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
