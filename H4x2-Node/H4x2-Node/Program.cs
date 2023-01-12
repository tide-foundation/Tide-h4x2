// 
// Tide Protocol - Infrastructure for a TRUE Zero-Trust paradigm
// Copyright (C) 2022 Tide Foundation Ltd
// 
// This program is free software and is subject to the terms of 
// the Tide Community Open Code License as published by the 
// Tide Foundation Limited. You may modify it and redistribute 
// it in accordance with and subject to the terms of that License.
// This program is distributed WITHOUT WARRANTY of any kind, 
// including without any implied warranty of MERCHANTABILITY or 
// FITNESS FOR A PARTICULAR PURPOSE.
// See the Tide Community Open Code License for more details.
// You should have received a copy of the Tide Community Open 
// Code License along with this program.
// If not, see https://tide.org/licenses_tcoc2-0-0-en
//

using H4x2_Node;
using H4x2_Node.Binders;

using Microsoft.AspNetCore.HttpOverrides;
using System.Numerics;
using H4x2_Node.Middleware;
using H4x2_TinySDK.Ed25519;
using H4x2_Node.Helpers;
using H4x2_Node.Services;

var builder = WebApplication.CreateBuilder(args);


/*
var prism = Environment.GetEnvironmentVariable("PRISM_VAL");
var isThrottled = Convert.ToBoolean(Environment.GetEnvironmentVariable("IS_THROTTLED"));
var insightsKey = Environment.GetEnvironmentVariable("INSIGHT_KEY");
*/
var prism = "2";
var isThrottled = false;
var publicKey = Point.FromBytes(Convert.FromBase64String(args[0]));

builder.Services.AddControllers(options => options.ModelBinderProviders.Insert(0, new BinderProvider()));

builder.Services.AddSingleton(
    new Settings
    {
        PRISM = BigInteger.Parse(prism)
    });

builder.Services.AddLazyCache();

builder.Services.AddControllersWithViews();

var services = builder.Services;

services.AddDbContext<DataContext>();
services.AddScoped<IUserService, UserService>();


var app = builder.Build();

app.MapGet("/isThrottled", () => isThrottled);
app.MapGet("/public", () => publicKey.ToBase64());

if (isThrottled)
{
    app.UseThrottling(); // neat
}

app.UseStaticFiles();

app.UseRouting();
app.UseCors(builder => builder.AllowAnyOrigin()); // change this when ORKs host enclave themselves

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.UseForwardedHeaders();
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
