using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using SMS.Infrastructure;
using SMS.Infrastructure.Interfaces;
using SMS.Models;
using SMS.Models.school_related;
using SMS.Models.user_lists;
using SMS.Services;
using SMS.Services.Interfaces;
using SMS.Services.Templates;
using SMS.Tools;
using System.IO;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();


builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo("/app/DataProtection-Keys"))
    .SetApplicationName("SchoolManagementSystem");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 44))
    )
);

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped(typeof(IGenericService<>), typeof(GenericService<>));

builder.Services.AddScoped<ContextHandler>();
builder.Services.AddScoped<ClassroomService>();
builder.Services.AddScoped<StaticImageService>();



builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = "Google";
})
.AddCookie()
.AddGoogle("Google", options =>
{
    options.ClientId = builder.Configuration["Google:ClientId"];
    options.ClientSecret = builder.Configuration["Google:ClientSecret"];
    options.CallbackPath = "/signin-google";
    options.SaveTokens = true;
    options.Events.OnCreatingTicket = async ctx =>
    {
        var fullName = ctx.Principal.FindFirstValue(ClaimTypes.Name)?.Trim();
        var email = ctx.Principal.FindFirstValue(ClaimTypes.Email)?.Trim().ToLower();
        var phone = ctx.Principal.FindFirstValue(ClaimTypes.MobilePhone);

        if (string.IsNullOrWhiteSpace(email)) return;

        var db = ctx.HttpContext.RequestServices.GetRequiredService<ApplicationDbContext>();

        // Lookup user in Users table
        var user = await db.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email);

        var regUser = await db.ListOfUsers
            .Include(x => x.School)
            .FirstOrDefaultAsync(x => x.Email.ToLower() == email);

        if (regUser == null)
        {
            ctx.Fail("Email not registered in ListOfUsers");
            return;
        }

        if (user == null)
        {
            user = new User
            {
                Email = email,
                Name = fullName ?? email,
                SchoolID = regUser.SchoolID,
                Phone = phone ?? string.Empty,
                Role = regUser.Role
            };
            db.Users.Add(user);
            await db.SaveChangesAsync();
        }
        else
        {
            if (string.IsNullOrWhiteSpace(user.Name) && !string.IsNullOrWhiteSpace(fullName))
            {
                user.Name = fullName;
                await db.SaveChangesAsync();
            }

            if (user.Role != regUser.Role)
            {
                user.Role = regUser.Role;
                await db.SaveChangesAsync();
            }
        }

        var claimsIdentity = ctx.Principal.Identity as ClaimsIdentity;
        if (claimsIdentity != null)
        {
            var existingRoleClaim = claimsIdentity.FindFirst(ClaimTypes.Role);
            if (existingRoleClaim != null)
            {
                claimsIdentity.RemoveClaim(existingRoleClaim);
            }

            claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, user.Role.ToString()));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.PrimarySid, user.Id.ToString()));
        }
    };
});
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
