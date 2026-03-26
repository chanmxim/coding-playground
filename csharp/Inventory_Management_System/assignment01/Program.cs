using assignment01.Areas.OrderManagement.Logic;
using assignment01.Areas.ProductManagement.Logic;
using assignment01.Data;
using assignment01.Models;
using assignment01.Areas.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Diagnostics;
using assignment01.Services;
using Microsoft.AspNetCore.Identity.UI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

// Register Logic in DI container
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<StackTrace>();
builder.Services.AddScoped<ErrorLog>();
builder.Services.AddScoped<ShoppingCartLogic>();
builder.Services.AddScoped<OrderLogic>();
builder.Services.AddScoped<ProductLogic>();

builder.Services.AddSerilog();

// Add session service
builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.IdleTimeout = TimeSpan.FromMinutes(180); // Set session timeout
});

// Configure connection string "DefaultConnection" (connect to a database)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDefaultIdentity<User>(options =>
    {
        options.SignIn.RequireConfirmedAccount = true;
        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequiredLength = 6;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

//Inject out SendGrid email sender
// builder.Services.AddSingleton<IEmailSender, EmailSender>();
builder.Services.AddScoped<IEmailSender, EmailSender>();

var app = builder.Build();


//Seed roles
using (var scope = app.Services.CreateScope())
{
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    try
    {
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        await ContextSeed.SeedRolesAsync(roleManager);
        await ContextSeed.SeedSuperAdminUser(roleManager, userManager);
    }
    catch (Exception ex)
    {
        logger.LogError(
            $"{DateTime.Now.ToUniversalTime()} attempt to seed roles and SuperAdmin user is failed - {ex.Message}");
    }
}


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStatusCodePages(async context =>
{
    if (context.HttpContext.Response.StatusCode == 404)
    {
        context.HttpContext.Response.Redirect("/Home/NotFound?statusCode=404");
    }

    if (context.HttpContext.Response.StatusCode >= 500 && context.HttpContext.Response.StatusCode < 600)
    {
        context.HttpContext.Response.Redirect($"/Home/NotFound?statusCode={context.HttpContext.Response.StatusCode}");
    }
});

app.UseHttpsRedirection();

// Ensure session middleware is called before UseAuthentication and UseAuthorization
app.UseSession();

app.UseRouting();

app.UseAuthentication(); // Ensure authentication comes after routing but before authorization

app.UseAuthorization();

app.MapStaticAssets();

app.UseSerilogRequestLogging();

app.MapControllerRoute(
        name: "areas",
        pattern: "{area:exists}/{controller=Product}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages();
app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();