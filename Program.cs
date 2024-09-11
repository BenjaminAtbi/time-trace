using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using time_trace.Data;

var builder = WebApplication.CreateBuilder(args);


// Add services
if (builder.Environment.IsDevelopment())
{
    var DBConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(DBConnectionString));
    builder.Services.AddDatabaseDeveloperPageExceptionFilter(); 
} else
{
    var DBConnectionString = builder.Configuration.GetConnectionString("AZURE_POSTGRESQL_CONNECTIONSTRING");
    builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(DBConnectionString));
    builder.Services.AddStackExchangeRedisCache(options =>
    {
        options.Configuration = builder.Configuration["AZURE_REDIS_CONNECTIONSTRING"];
        options.InstanceName = "SampleInstance";
    });
}

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
.AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

//logging env
app.Logger.LogInformation("initialization logging is a go");
app.Logger.LogInformation("Environment: " + builder.Environment.EnvironmentName);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
