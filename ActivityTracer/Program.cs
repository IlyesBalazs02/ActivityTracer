using ActivityTracer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using ActivityTracer.Models;
using ActivityTracer.Services;
using ActivityTracer.Hubs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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
	.AddDefaultTokenProviders()
	.AddEntityFrameworkStores<ActivityDbContext>();

builder.Services.AddScoped<FollowingService>();


builder.Services.AddAuthentication().AddFacebook(opt =>
{
	opt.AppId = "950452413482166";
	opt.AppSecret = "427384153dc998e129210735438c2faa";
});

builder.Services.AddAuthentication(option =>
{
	option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
	option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
	options.SaveToken = true;
	options.RequireHttpsMetadata = true;
	options.TokenValidationParameters = new TokenValidationParameters()
	{
		ValidateIssuer = true,
		ValidateAudience = true,
		ValidAudience = "http://www.security.org",
		ValidIssuer = "http://www.security.org",
		IssuerSigningKey = new SymmetricSecurityKey
	  (Encoding.UTF8.GetBytes("nagyonhosszutitkoskodhelye"))
	};
});

builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}
else
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.MapHub<EventHub>("/events");

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
