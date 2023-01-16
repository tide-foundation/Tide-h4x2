using H4x2_Vendor.Helpers;
using H4x2_Vendor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var services = builder.Services;
services.AddDbContext<DataContext>();
services.AddScoped<IUserService, UserService>();

var app = builder.Build();

app.MapControllers();

if(app.Environment.IsDevelopment()) // To create table if not exists
{
    using(var scope = app.Services.CreateScope())
    {
        var dataContext = scope.ServiceProvider.GetRequiredService<DataContext>();
        dataContext.Database.EnsureCreated();
    }
}

app.Run();


