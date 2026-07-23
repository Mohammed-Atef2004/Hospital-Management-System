using API.Middlewares;
using Application;
using Domain.Departments;
using Domain.Doctors;
using Domain.Interfaces.Services;
using Domain.Medicines;
using Domain.Nurses;
using Domain.Repositories;
using Domain.Schedules;
using FluentValidation;
using Infastructure.Presistence.Database;
using Infastructure.Repositories;
using Infrastructure.Repositories;
using Infrastructure.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Presentation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ================== Controllers ==================
            builder.Services.AddControllers();

            // ================== Swagger ==================
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // ================== DbContext ==================
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // ================== Repositories + UoW ==================
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
            builder.Services.AddScoped<IPatientRepository, PatientRepository>();
            builder.Services.AddScoped<IMedicineRepository, MedicineRepository>();
            builder.Services.AddScoped<INurseRepository, NurseRepository>();
            builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
            builder.Services.AddScoped<IShiftRepository, ShiftRepository>();

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

            // ================== JWT Authentication ==================
            //var jwtSection = builder.Configuration.GetSection("JWT");
            //var signingKey = Encoding.UTF8.GetBytes(jwtSection["SigningKey"]);

            //builder.Services.AddAuthentication(options =>
            //{
            //    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //})
            //.AddJwtBearer(options =>
            //{
            //    options.RequireHttpsMetadata = false;
            //    options.SaveToken = true;
            //    options.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidateIssuer = true,
            //        ValidateAudience = true,
            //        ValidateLifetime = true,
            //        ValidateIssuerSigningKey = true,

            //        ValidIssuer = jwtSection["Issuer"],
            //        ValidAudience = jwtSection["Audience"],
            //        IssuerSigningKey = new SymmetricSecurityKey(signingKey)
            //    };
            //});

            var app = builder.Build();
            app.UseMiddleware<ExceptionHandlingMiddleware>();

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
        }
    }
}
