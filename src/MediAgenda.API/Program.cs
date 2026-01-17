
using FluentValidation;
using MediAgenda.API.MaperConfig;
using MediAgenda.API.Middleware;
using MediAgenda.Application.DTOs;
using MediAgenda.Application.Interfaces;
using MediAgenda.Application.Services;
using MediAgenda.Application.Validations;
using MediAgenda.Domain.Entities;
using MediAgenda.Infraestructure.Context;
using MediAgenda.Infraestructure.Interfaces;
using MediAgenda.Infraestructure.Models;
using MediAgenda.Infraestructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Newtonsoft.Json.Serialization;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using System.Reflection;
using System.Text;

namespace MediAgenda.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

			MappingConfig.RegisterMappings();

			builder.Services.AddDbContext<MediContext>(
                opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            var origins = builder.Configuration.GetSection("origins").Get<string[]>();
           

            builder.Services.AddCors(opciones =>
            {
                opciones.AddDefaultPolicy(optionsCORS =>
                {
                    optionsCORS.WithOrigins(origins)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
                });
            });           


            builder.Services.AddControllers().AddNewtonsoftJson(options =>
            {
                // Esto hace que JsonPatch respete el [JsonIgnore] para que no cambien los Id en los patch
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });

            builder.Services.AddValidatorsFromAssemblyContaining<AnalysisCreateValidation>();
            builder.Services.AddValidatorsFromAssemblyContaining<AnalysisUpdateValidation>();
            builder.Services.AddFluentValidationAutoValidation();



            builder.Services.AddScoped<IAnalysisRepository, AnalysisRepository>();
            builder.Services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();
            builder.Services.AddScoped<IClinicRepository, ClinicRepository>();
            builder.Services.AddScoped<IConsultationRepository, ConsultationRepository>();
            builder.Services.AddScoped<IDayAvailableRepository, DayAvailableRepository>();
            builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
            builder.Services.AddScoped<IInsuranceRepository, InsuranceRepository>();
            builder.Services.AddScoped<IMedicalDocumentRepository, MedicalDocumentRepository>();
            builder.Services.AddScoped<IMedicineRepository, MedicineRepository>();
            builder.Services.AddScoped<INoteConsultationRepository, NoteConsultationRepository>();
            builder.Services.AddScoped<INotePatientRepository, NotePatientRepository>();
            builder.Services.AddScoped<IPatientRepository, PatientRepository>();
            builder.Services.AddScoped<IPermissionRepository, PermissionRepository>();
            builder.Services.AddScoped<IPrescriptionRepository, PrescriptionRepository>();
            builder.Services.AddScoped<IReasonRepository, ReasonRepository>();

            builder.Services.AddScoped<IAnalysesService, AnalysesService>();
            builder.Services.AddScoped<IReasonsService, ReasonsService>();
            builder.Services.AddScoped<IApplicationUsersService, ApplicationUsersService>();
            builder.Services.AddScoped<IClinicsService, ClinicsService>();
            builder.Services.AddScoped<IConsultationsService, ConsultationsService>();
            builder.Services.AddScoped<IDayAvailablesService, DayAvailablesService>();
            builder.Services.AddScoped<IDoctorsService, DoctorsService>();
            builder.Services.AddScoped<IInsurancesService, InsurancesService>();
            builder.Services.AddScoped<IMedicalDocumentsService, MedicalDocumentsService>();
            builder.Services.AddScoped<IMedicinesService, MedicinesService>();
            builder.Services.AddScoped<INotesConsultationService, NotesConsultationService>();
            builder.Services.AddScoped<INotesPatientsService, NotesPatientsService>();
            builder.Services.AddScoped<IPatientsService, PatientsService>();
            builder.Services.AddScoped<IPermissionsService, PermissionsService>();
            builder.Services.AddScoped<IPrescriptionsService, PrescriptionsService>();
            builder.Services.AddScoped<IAuthService, AuthService>();

            builder.Services.AddScoped<IValidationService, ValidationService>();

            builder.Services.AddIdentity<ApplicationUserModel, IdentityRole>()
                .AddEntityFrameworkStores<MediContext>()
                .AddDefaultTokenProviders();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true, // datos a validar
                        ValidateAudience = true,
                        ValidateLifetime = true, // el tiempo de vida
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])), //contraseña
                        ClockSkew = TimeSpan.Zero 
                    };
                });

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(opt =>
            {
                opt.EnableAnnotations();
                opt.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "MediAgenda API de Gestíon Medica",
                    Version = "v1",
                    Description = "Esta es una API encargada de gestionar consultas medicas, ademas de documentos, analisis y medicamentos.",
                    Contact = new OpenApiContact
                    {
                        Name = "Jhan Crhistian Terrero Ramirez",
                        Email = "j.crhistiantr@gmail.com"
                    }
                });

            }
            );

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();

            //Revisa el context de la peticion
            app.Use(async (context, next) =>
            {
                //Busca una cookie con el nombre jwt
                var token = context.Request.Cookies["jwt"];
                if (!string.IsNullOrEmpty(token))
                {
                    context.Request.Headers.Add("Authorization", $"Bearer {token}");
                }
                await next();
            });


            
            app.UseLogMiddleware();

            app.UseHttpsRedirection();

            app.UseCors();

            app.UseAuthentication();

            app.UseAuthorization();           

            app.MapControllers();

            app.Run();
        }
    }
}
