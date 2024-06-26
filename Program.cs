using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TwitterClone.Data;
using TwitterClone.Mappings;
using TwitterClone.Models.Domains;
using TwitterClone.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add our DbContext as an injectable dependency
builder.Services.AddDbContext<TwitterCloneDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("TwitterCloneConnectionString"));
});
builder.Services.AddDbContext<TwitterCloneAuthDbContext>(options =>
{
    options.UseLazyLoadingProxies(); // Habilitar carga diferida (lazy loading)
    options.UseSqlServer(builder.Configuration.GetConnectionString("TwitterCloneAuthConnectionString"));
});

// Injectors
builder.Services.AddScoped<ITokenRepository, TokenRepository>();
builder.Services.AddScoped<ITweetRepository, TweetRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<ILikeRepository, LikeRepository>();
builder.Services.AddScoped<ILikeToCommentRepository, LikeToCommentRepository>();
builder.Services.AddScoped<IProfileRepository, ProfileRepository>();

// AutoMapper Config
builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));

// Identity Services
builder.Services.AddIdentityCore<ApplicationUser>()
    .AddRoles<IdentityRole>()
    .AddTokenProvider<DataProtectorTokenProvider<ApplicationUser>>("TwitterClone")
    .AddEntityFrameworkStores<TwitterCloneAuthDbContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
});

// Auth Services
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();