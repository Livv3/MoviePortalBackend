global using Microsoft.EntityFrameworkCore;
global using movies_DAL;
using movies_BLL.services;
using movies_BLL.models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using movies_BLL.ExternalDataMapping;
using movies_DAL.Repositories;
using movies_BLL.IRepositories;
using System.Security.Claims;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddScoped<IMovieService, MovieService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<IStreamingsiteService, StreamingsiteService>();
builder.Services.AddScoped<IUserMoviesService, UserMoviesService>();
// Add repository to the container
builder.Services.AddScoped<IMovieRepository, MovieRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<IStreamingsiteRepository, StreamingsiteRepository>();
builder.Services.AddScoped<IUserMoviesRepository, UserMoviesRepository>();



builder.Services.AddControllers();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

// Add database
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});


// Add Identity
builder.Services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<DataContext>()
            .AddDefaultTokenProviders();
builder.Services.Configure<IdentityOptions>(options =>
{
    //password
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;

    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(100);
    options.Lockout.MaxFailedAccessAttempts = 10;
    options.User.RequireUniqueEmail = true;

});

// JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        RoleClaimType = ClaimTypes.Role
    };
});


// Create an admin user
using (var serviceScope = builder.Services.BuildServiceProvider().CreateScope())
{
    var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<User>>();
    var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    // Create the "Admin" role if it doesn't exist
    if (!roleManager.RoleExistsAsync("Admin").Result)
    {
        roleManager.CreateAsync(new IdentityRole("Admin")).Wait();
    }

    var adminUser = userManager.FindByNameAsync("admin").Result;

    if (adminUser == null)
    {
        adminUser = new User
        {
            Id = Guid.NewGuid().ToString(),
            UserName = "admin",
            Email = "admin@example.com"
        };
        var result = userManager.CreateAsync(adminUser, "Password1").Result;

        if (result.Succeeded)
        {
            userManager.AddToRoleAsync(adminUser, "Admin").Wait();
        }
    }
}

//builder.Services.AddHttpClient();

builder.Services.AddHttpClient<IMovieRepository, MovieRepository>();
builder.Services.AddAutoMapper(typeof(MovieMappingProfile));



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
{
    builder.WithOrigins("http://localhost:5175", "http://localhost:4200")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
}));



var app = builder.Build();


app.UseSwagger();

app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseCors("corsapp");

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
