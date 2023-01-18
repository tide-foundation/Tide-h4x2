﻿using H4x2_Vendor.Helpers;
using H4x2_Vendor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var services = builder.Services;
services.AddDbContext<DataContext>();
services.AddScoped<IUserService, UserService>();

var app = builder.Build();
app.UseStaticFiles();
app.UseCors(builder => builder.AllowAnyOrigin()); // change this when ORKs host enclave themselves

app.MapControllers();

using(var scope = app.Services.CreateScope()) // Create table if does not exist
{
    var dataContext = scope.ServiceProvider.GetRequiredService<DataContext>();
    dataContext.Database.EnsureCreated();
}


app.Run();


