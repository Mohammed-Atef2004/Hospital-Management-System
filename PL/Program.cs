using Application;
using Domain.Repositories;
using FluentValidation;
using Infastructure.Presistence.Database;
using Infastructure.Repositories;
using Infrastructure.Repositories;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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

            // ================== MediatR ==================
            builder.Services.AddMediatR(typeof(ApplicationAssembly));

            // ================== AutoMapper ==================
            builder.Services.AddAutoMapper(typeof(ApplicationAssembly));


            // ================== FluentValidation (Global) ==================
            builder.Services.AddValidatorsFromAssembly(typeof(ApplicationAssembly).Assembly);
            builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));

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

            // ================== Middleware ==================
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication(); // IMPORTANT
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
