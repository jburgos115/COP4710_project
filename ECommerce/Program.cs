using ECommerce.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
		builder.Configuration.GetConnectionString("DefaultConnection")
	));

// Enable IConfiguration to read appsettings for connection string
Host.CreateDefaultBuilder(args);

/*******************/
/* CLAIMS SERVICES */
/*******************/

builder.Services.AddAuthentication("MyCookieAuth").AddCookie("MyCookieAuth", options => {
    options.Cookie.Name = "MyCookieAuth";
    options.LoginPath = "/LoginRegister/Login";
    options.AccessDeniedPath = "/LoginRegister/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromDays(1);
});

builder.Services.AddAuthorization(options => {
    options.AddPolicy("UserCredentials",
        policy => policy.RequireClaim("User", "General"));
});

/***********************/
/* END CLAIMS SERVICES */
/***********************/

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
