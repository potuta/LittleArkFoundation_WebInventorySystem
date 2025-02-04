using LittleArkFoundation_WebInventorySystem.Data;

var builder = WebApplication.CreateBuilder(args);

// Add authentication services
builder.Services.AddAuthentication("CookieAuth")
    .AddCookie("CookieAuth", options =>
    {
        options.LoginPath = "/Home/Index";  // Redirect to login page
        options.AccessDeniedPath = "/Home/Index"; // Redirect if unauthorized
        options.ExpireTimeSpan = TimeSpan.FromDays(7); // Keep user logged in
    });

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<ConnectionService>();

//builder.WebHost.ConfigureKestrel(options =>
//{
//    options.ListenAnyIP(16969); // Change port as needed
//});

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
