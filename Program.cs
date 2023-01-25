var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient(name: "Platform",
         options =>
         {
             options.BaseAddress = new Uri("http://localhost:7285/");
             //options.DefaultRequestHeaders.Accept.Add(
             //new MediaTypeWithQualityHeaderValue(
             //"application/json", 1.0));
             //options.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
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

app.Run();
