using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ourTube.Repositories;
using ourTube.Repositories.Interfaces;
using OurTube.Repositories;
using OurTube.Repositories.Interfaces.Common;
using Microsoft.IdentityModel.Tokens;
using outTube.Data;
using outTube.Models;
using outTube.Services;
using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore; // <-- Required for AddFluentValidationAutoValidation / AddFluentValidationClientsideAdapters

namespace outTube
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();


            // Register FluentValidation
            builder.Services.AddFluentValidationAutoValidation()
                            .AddFluentValidationClientsideAdapters();

            // Register all validators from the assembly
            builder.Services.AddValidatorsFromAssemblyContaining<VideoCreateValidator>();

            // Register HttpContextAccessor (needed for SubscribeValidator)
            builder.Services.AddHttpContextAccessor();




            // 1. Register DbContext
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<IVideoRepo, VideoRepo>();
            builder.Services.AddScoped<ITokenService, TokenService>();

            // 2. Register Identity
            builder.Services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            // 3. Register JWT Authentication
            var jwtSettings = builder.Configuration.GetSection("Jwt");
            var keyStr = jwtSettings["Key"] ?? "A_Very_Long_And_Extra_Secret_OurTube_Project_Key_2025_Its_Finally_Long_Enough_Now!";
            var key = Encoding.UTF8.GetBytes(keyStr);

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings["Issuer"] ?? "ourTube",
                    ValidateAudience = true,
                    ValidAudience = jwtSettings["Audience"] ?? "ourTubeUsers",
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Cookies["X-Access-Token"];
                        return Task.CompletedTask;
                    }
                };
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            // 4. Use Authentication & Authorization
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
