using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using School_Management_System.Infrastructure;
using School_Management_System.Models;
using School_Management_System.Models.school_related;
using School_Management_System.Models.user_lists;
using School_Management_System.Services;
using System.IO;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();


builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo("/app/DataProtection-Keys"))
    .SetApplicationName("SchoolManagementSystem");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 44))
    )
);

builder.Services.AddScoped(typeof(ICrudRepository<>), typeof(CrudRepository<>));
builder.Services.AddScoped<UserService>();

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

        // Lookup registered user in ListOfUsers
        var regUser = await db.ListOfUsers
            .Include(x => x.School)
            .Include(x => x.Classroom)
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
                ClassroomID = regUser.ClassroomID,
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

            // Update role if changed in ListOfUsers
            if (user.Role != regUser.Role)
            {
                user.Role = regUser.Role;
                await db.SaveChangesAsync();
            }
        }

        // Add claims for authentication cookie
        var claimsIdentity = ctx.Principal.Identity as ClaimsIdentity;
        if (claimsIdentity != null)
        {
            // Remove old role claims if exist
            var existingRoleClaim = claimsIdentity.FindFirst(ClaimTypes.Role);
            if (existingRoleClaim != null)
            {
                claimsIdentity.RemoveClaim(existingRoleClaim);
            }

            // Add role claim
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, user.Role.ToString()));
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

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
