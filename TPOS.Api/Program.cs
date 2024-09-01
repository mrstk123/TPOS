using Microsoft.EntityFrameworkCore;
using TPOS.Api.Services;
using TPOS.Infrastructure;
using TPOS.Infrastructure.DbInitializer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureServices();   // Custom Services

var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

SeedDatabase(app);
app.Run();

static void SeedDatabase(WebApplication app)
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;

        try
        {
            var databaseInitializer = services.GetRequiredService<IDbInitializer>();
            databaseInitializer.SeedAsync().Wait();
        }
        catch (Exception)
        {
            //var logger = services.GetRequiredService<ILogger<Program>>();
            //logger.LogCritical(LoggingEvents.INIT_DATABASE, ex, LoggingEvents.INIT_DATABASE.Name);

            //throw new Exception(LoggingEvents.INIT_DATABASE.Name, ex);
        }
    }
}