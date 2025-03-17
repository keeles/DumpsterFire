using System.Data.Common;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add db connection
builder.Services.AddDbContext<ASP.NETCore.ApplicationDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(9, 0, 1))
    )
);

//Configuration for sessions
builder.Services.AddHttpContextAccessor();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseStatusCodePagesWithReExecute("/Error/NotFound", "?statusCode={0}");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else if (app.Environment.IsStaging())
{
    app.UseExceptionHandler("/Error/Index");
    app.UseStatusCodePagesWithReExecute("/Error/NotFound", "?statusCode={0}");
}
else if (app.Environment.IsEnvironment("Testing"))
{
    app.UseDeveloperExceptionPage();
    app.UseStatusCodePagesWithReExecute("/Error/NotFound", "?statusCode={0}");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseExceptionHandler("/Error/Index");
    app.UseStatusCodePagesWithReExecute("/Error/NotFound", "?statusCode={0}");
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(name: "Thread", pattern: "{controller=Thread}/{action=Index}/{id?}");
app.MapControllerRoute(name: "Board", pattern: "{controller=Board}/{action=Index}/{id?}");
app.MapControllerRoute(name: "Post", pattern: "{controller=Post}/{action=Index}/{id?}");
app.MapControllerRoute(name: "Auth", pattern: "{controller=Auth}/{action=Index}/{id?}");
app.MapControllerRoute(name: "Error", pattern: "{controller=Error}/{action=Index}");

app.Run();
