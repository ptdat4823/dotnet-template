using Microsoft.EntityFrameworkCore;
using TMS.Application.Interfaces.Repositories;
using TMS.Application.Interfaces.Services;
using TMS.Application.Services;
using TMS.Infrastructure.Data;
using TMS.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDBContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("TMS.Infrastructure") // 👈 Fix for migration location
    )
);

// 🔹 Register Dependencies
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddControllers();

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDBContext>();
    if (!context.Database.CanConnect())
    {
        throw new Exception("Can't connect to the database");
    }

    context.Database.Migrate();
}

app.UseRouting();
app.MapControllers();
app.Run();