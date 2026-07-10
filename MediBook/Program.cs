using MediBook.Data;
using MediBook.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<DbConnectionFactory>();

// Add Response Compression
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
});

// Configure Session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(2);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    options.Cookie.SameSite = SameSiteMode.Strict;
});
builder.Services.AddHttpContextAccessor();

// Add Anti-forgery
builder.Services.AddAntiforgery(options =>
{
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    options.Cookie.SameSite = SameSiteMode.Strict;
});

// Register Repositories
builder.Services.AddTransient<MediBook.Repositories.Interfaces.IUserRepository, MediBook.Repositories.UserRepository>();
builder.Services.AddTransient<MediBook.Repositories.Interfaces.IPatientRepository, MediBook.Repositories.PatientRepository>();
builder.Services.AddTransient<MediBook.Repositories.Interfaces.IDoctorRepository, MediBook.Repositories.DoctorRepository>();
builder.Services.AddTransient<MediBook.Repositories.Interfaces.IAppointmentRepository, MediBook.Repositories.AppointmentRepository>();

// Register Services
builder.Services.AddTransient<MediBook.Services.Interfaces.IAccountService, MediBook.Services.AccountService>();
builder.Services.AddTransient<MediBook.Services.Interfaces.IDashboardService, MediBook.Services.DashboardService>();
builder.Services.AddTransient<MediBook.Services.Interfaces.IDoctorService, MediBook.Services.DoctorService>();
builder.Services.AddTransient<MediBook.Services.Interfaces.IAppointmentService, MediBook.Services.AppointmentService>();
builder.Services.AddTransient<MediBook.Services.Interfaces.ISettingsService, MediBook.Services.SettingsService>();
builder.Services.AddTransient<MediBook.Services.Interfaces.IDoctorDashboardService, MediBook.Services.DoctorDashboardService>();
builder.Services.AddTransient<MediBook.Services.Interfaces.IAdminDashboardService, MediBook.Services.AdminDashboardService>();
builder.Services.AddTransient<MediBook.Services.Interfaces.IAuditService, MediBook.Services.AuditService>();
builder.Services.AddTransient<MediBook.Services.Interfaces.IPatientService, MediBook.Services.PatientService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.Use(async (context, next) =>
{
    context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
    context.Response.Headers.Append("X-Frame-Options", "DENY");
    context.Response.Headers.Append("X-XSS-Protection", "1; mode=block");
    context.Response.Headers.Append("Referrer-Policy", "strict-origin-when-cross-origin");
    await next();
});

app.UseResponseCompression();
app.UseMiddleware<ExceptionMiddleware>();

if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStatusCodePagesWithReExecute("/Error/{0}");

app.UseRouting();

app.UseSession();
app.UseMiddleware<AuthenticationMiddleware>();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}")
    .WithStaticAssets();

using (var scope = app.Services.CreateScope())
{
    var connectionFactory = scope.ServiceProvider.GetRequiredService<DbConnectionFactory>();
    await DbInitializer.InitializeAsync(connectionFactory);
}

app.Run();
