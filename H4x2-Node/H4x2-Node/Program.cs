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
using H4x2_Node.Models.Serialization;
using Microsoft.AspNetCore.HttpOverrides;
using System.Numerics;

var builder = WebApplication.CreateBuilder(args);


var prism = Environment.GetEnvironmentVariable("PRISM_VAL");
var isPublic = Convert.ToBoolean(Environment.GetEnvironmentVariable("ISPUBLIC"));


// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddControllers(options => options.ModelBinderProviders.Insert(0, new BinderProvider()))
        .AddJsonOptions(opt =>
        {
            opt.JsonSerializerOptions.Converters.Add(new PointSerializer());
        });

builder.Services.AddSingleton(
    new Settings
    {
        PRISM = BigInteger.Parse(prism)
    });
builder.Services.AddLazyCache();
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.MapGet("/isPublic", () => isPublic);

if (isPublic)
{
    app.MapGet("/prizeKey", () => prism); // only show partial prize key on public node
}
app.UseCors(builder => builder.AllowAnyOrigin());
//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.UseForwardedHeaders();

app.Run();
