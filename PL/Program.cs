using API.Middlewares;
using Application;
using Application.Features.Authentication;
using Domain.Departments;
using Domain.Doctors;
using Domain.Interfaces;
using Domain.Interfaces.Services;
using Domain.Medicines;
using Domain.Nurses;
using Domain.Repositories;
using Domain.Schedules;
using Domain.Users;
using FluentValidation;
using Infastructure.Presistence.Database;
using Infastructure.Repositories;
using Infrastructure.Repositories;
using Infrastructure.Services;
using API.Services;
using Application.Common;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;
using System.Security.Claims;
using System.Text;

namespace Presentation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Logging.AddJsonConsole();

            // ================== Controllers ==================
            builder.Services.AddControllers();
            builder.Services.AddHealthChecks();

            // ================== Swagger ==================
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // ================== DbContext ==================
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddDataProtection();

            builder.Services.AddIdentityCore<IdentityUser>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireDigit = true;
                options.Password.RequireNonAlphanumeric = true;
                options.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<ApplicationDbContext>();

            var jwtOptions = builder.Configuration.GetSection(JWTOptions.SectionName).Get<JWTOptions>()
                ?? throw new InvalidOperationException("The JWT configuration section is required.");
            builder.Services.AddOptions<JWTOptions>()
                .Bind(builder.Configuration.GetSection(JWTOptions.SectionName))
                .Validate(options => !string.IsNullOrWhiteSpace(options.Issuer), "JWT issuer is required.")
                .Validate(options => !string.IsNullOrWhiteSpace(options.Audience), "JWT audience is required.")
                .Validate(options => options.LifetimeSeconds > 0, "JWT lifetime must be greater than zero.")
                .Validate(options => options.RefreshTokenLifetimeDays is > 0 and <= 365,
                    "Refresh token lifetime must be between 1 and 365 days.")
                .Validate(options => Encoding.UTF8.GetByteCount(options.SigningKey) >= 32,
                    "JWT signing key must contain at least 32 UTF-8 bytes.")
                .ValidateOnStart();

            builder.Services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = jwtOptions.Issuer,
                        ValidateAudience = true,
                        ValidAudience = jwtOptions.Audience,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SigningKey)),
                        RoleClaimType = ClaimTypes.Role,
                        NameClaimType = ClaimTypes.Name,
                        ClockSkew = TimeSpan.FromSeconds(30)
                    };
                });
            builder.Services.AddAuthorization(options =>
            {
                options.FallbackPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                options.AddPolicy("Administrative", policy => policy.RequireRole("Admin"));
                options.AddPolicy("Clinical", policy => policy.RequireRole("Admin", "Doctor", "Nurse"));
                options.AddPolicy("CareProviders", policy => policy.RequireRole("Admin", "Doctor"));
                options.AddPolicy("Patients", policy => policy.RequireRole("Patient"));
            });
            builder.Services.AddRateLimiter(options =>
            {
                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
                options.AddPolicy("auth", context => RateLimitPartition.GetFixedWindowLimiter(
                    context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                    _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 10,
                        Window = TimeSpan.FromMinutes(1),
                        QueueLimit = 0,
                        AutoReplenishment = true
                    }));
            });

            // ================== Repositories + UoW ==================
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<ITransactionManager, Infrastructure.Persistence.EfTransactionManager>();
            builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
            builder.Services.AddScoped<IPatientRepository, PatientRepository>();
            builder.Services.AddScoped<IMedicineRepository, MedicineRepository>();
            builder.Services.AddScoped<INurseRepository, NurseRepository>();
            builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
            builder.Services.AddScoped<IShiftRepository, ShiftRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IAuthTokenService, JwtTokenService>();
            builder.Services.AddSingleton<IRefreshTokenService>(
                new CryptographicRefreshTokenService(jwtOptions.RefreshTokenLifetimeDays));
            builder.Services.AddSingleton<ITwoFactorCodeService, TotpCodeService>();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();

            // ================== MediatR ==================
            builder.Services.AddMediatR(typeof(ApplicationAssembly));

            // ================== AutoMapper ==================
            builder.Services.AddAutoMapper(typeof(ApplicationAssembly));

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // ================== FluentValidation (Global) ==================
            builder.Services.AddValidatorsFromAssembly(typeof(ApplicationAssembly).Assembly);
            builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));
            builder.Services.AddAutoMapper(typeof(ApplicationAssembly).Assembly);

            var app = builder.Build();
            app.UseMiddleware<ExceptionHandlingMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseRateLimiter();
            app.UseAuthorization();

            app.MapControllers();
            app.MapHealthChecks("/health").AllowAnonymous();

            app.Run();
        }
    }
}
