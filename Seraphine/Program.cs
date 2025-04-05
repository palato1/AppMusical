using Scalar.AspNetCore;
using Seraphine.Core;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowAnyOrigin()
        );
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDependencyInjectionServices(builder.Configuration);

var app = builder.Build();

DatabaseInitializer.ApplyMigrations(app); // Aplicar as migrations antes de iniciar a API

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    // app.UseSwaggerUI();

    // Mapeie o Scalar UI
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("Seraphine");
        options.WithTheme(ScalarTheme.Saturn);
        options.WithOpenApiRoutePattern("/swagger/v1/swagger.json");
    });
}

app.UseCors("AllowAll");

app.UseMiddleware<LoginMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
