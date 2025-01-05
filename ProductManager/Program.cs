using Microsoft.EntityFrameworkCore;
using ProductManager.Data;
using ProductManager.Repositories;
using ProductManager.Services;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;


builder.Services.AddRazorPages();
builder.Services.AddDbContext<AppDbContext>(options =>
       options.UseNpgsql(config.GetConnectionString("DefaultConnection")));

builder.Services.AddTransient<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseStaticFiles();
app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
