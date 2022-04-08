using Microsoft.EntityFrameworkCore;
using PlayLister.Client.Mapping;
using PlayLister.Client.Middlewares;
using PlayLister.Infrastructure.Context;
using PlayLister.Infrastructure.Repositories.Extensions;
using PlayLister.Services;
using PlayLister.Services.Extensions;
using PlayLister.Services.Models.Spotify;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddDbContext<PlayListerContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddScoped<DbContext, PlayListerContext>();
builder.Services.AddServices();
builder.Services.AddRepositories();
builder.Services.AddAutoMapper(typeof(MappingConfiguration));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();



app.UseAuthorization();
app.UseMiddleware<RefreshTokenMiddleware>();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
