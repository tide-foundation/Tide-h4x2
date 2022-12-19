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

var builder = WebApplication.CreateBuilder(args);


var prism = Environment.GetEnvironmentVariable("PRISM_VAL");
var isThrottled = Convert.ToBoolean(Environment.GetEnvironmentVariable("IS_THROTTLED"));
var insightsKey = Environment.GetEnvironmentVariable("INSIGHT_KEY");

builder.Services.AddControllers(options => options.ModelBinderProviders.Insert(0, new BinderProvider()));

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

if(insightsKey != null)
{
    builder.Services.AddApplicationInsightsTelemetry(insightsKey);  

}

var app = builder.Build();

app.MapGet("/isThrottled", () => isThrottled);

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

app.Run();
