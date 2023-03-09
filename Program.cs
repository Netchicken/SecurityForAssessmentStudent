using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using SecurityForAssessmentStudent.Data;


var CORSAllowSpecificOrigins = "CORSAllowed";
var builder = WebApplication.CreateBuilder(args);

//add CORS to project
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: CORSAllowSpecificOrigins,
                         policy =>
                         {
                             policy.WithOrigins("http://localhost:3001", "http://localhost:3000", "http://www.contoso.com");
                         });
});


// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddRazorPages();

builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings.
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
    options.SignIn.RequireConfirmedEmail = false;



    // Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings.
    options.User.AllowedUserNameCharacters =
    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = false;
});


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RoleAdminPolicy", policyBuilder => policyBuilder.RequireRole("Admin"));
    options.AddPolicy("ClaimAdminPolicy", policyBuilder => policyBuilder.RequireClaim("Admin"));

    options.AddPolicy("ViewRolesPolicy", policyBuilder => policyBuilder.RequireAssertion(context =>
    {
        var joiningDateClaim = context.User.FindFirst(c => c.Type == "Joining Date")?.Value;
        var joiningDate = DateTime.Parse(joiningDateClaim);
        return context.User.HasClaim("Permission", "View Roles") && joiningDate > DateTime.MinValue && joiningDate < DateTime.Now.AddMonths(-6);

    }));
    //context.User.HasClaim("Permission", "View Roles") && 
});

builder.Services.AddSwaggerGen();


builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeFolder("/RoleManager", "ViewRolesPolicy");

});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCors(CORSAllowSpecificOrigins);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapRazorPages();

app.Run();
