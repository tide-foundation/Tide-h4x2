using H4x2_Node;
using H4x2_Node.Binders;
using H4x2_Node.Models.Serialization;
using System.Numerics;

var builder = WebApplication.CreateBuilder(args);

/*
var prism = Environment.GetEnvironmentVariable("PRISM_VAL");
var isPublic = Convert.ToBoolean(Environment.GetEnvironmentVariable("ISPUBLIC"));
*/

var prism = "1554967558028019017635266871044954225154957206644506058050707959306264304941";
var isPublic = true;

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

app.Run();
