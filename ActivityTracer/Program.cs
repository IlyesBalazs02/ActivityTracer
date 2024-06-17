using ActivityTracer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using ActivityTracer.Models;
using ActivityTracer.Services;

var builder = WebApplication.CreateBuilder(args);

//var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<Microsoft.AspNetCore.Identity.UI.Services.IEmailSender, EmailSender>();

builder.Services.AddTransient<IAppActivityRepository, AppActivityRepository>();
builder.Services.AddDbContext<ActivityDbContext>(opt =>
	opt
    .UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=ActivityDb;Trusted_Connection=True;MultipleActiveResultSets=true")
    .UseLazyLoadingProxies()
    );

builder.Services.AddDefaultIdentity<SiteUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false; //set back to true
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ActivityDbContext>();

builder.Services.AddScoped<FollowingService>();


builder.Services.AddAuthentication().AddFacebook(opt =>
{
    opt.AppId = "950452413482166";
    opt.AppSecret = "427384153dc998e129210735438c2faa";
});

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
