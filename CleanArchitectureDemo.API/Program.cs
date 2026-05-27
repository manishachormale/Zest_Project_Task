using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;
using Microsoft.Extensions.Configuration;


using AutoMapper;
using CleanArchitectureDemo.API.Middleware;

using CleanArchitectureDemo.Infrastructure.Data;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();



builder.Services.AddCors(options =>
{
    options.AddPolicy("MyPolicy",
        policy =>
        {
            policy.WithOrigins("https://localhost:3003")
            .AllowAnyHeader()
            .AllowAnyMethod();
        });
});



builder.Services.AddMediatR(typeof(Program));




builder.Services.AddSwaggerGen(
    options =>
    {
        options.AddSecurityDefinition("Bearer",
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                Description = "Enter JWT Token"
            });

    });
        builder.Services.AddDbContext<MyDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DBCS")));

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme =
                JwtBearerDefaults.AuthenticationScheme;

            options.DefaultChallengeScheme =
                JwtBearerDefaults.AuthenticationScheme;

        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters =
                new TokenValidationParameters
                {
                    ValidateIssuer = true,

                    ValidateAudience = true,

                    ValidateLifetime = true,

                    ValidateIssuerSigningKey = true,

                    ValidIssuer =
                        builder.Configuration["Jwt:Issuer"],

                    ValidAudience =
                        builder.Configuration["Jwt:Audience"],

                    IssuerSigningKey = //this is signature
                        new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(
                                builder.Configuration["Jwt:Key"]))
                };
        });

        builder.Services.AddAutoMapper(typeof(Program)); //automapper


        var builderserilog = WebApplication.CreateBuilder(args); //serilog



        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .WriteTo.File("Logs/log.txt",
        rollingInterval: RollingInterval.Day
        ).CreateLogger();
        Log.Information("Application Started");
        builder.Host.UseSerilog();




        var app = builder.Build();


        app.UseSerilogRequestLogging();
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseCors("MyPolicy");
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseMiddleware<CustomMiddleware>();
app.UseMiddleware<ExceptionMiddleware>();
        app.MapControllers();
        app.Run();
    
    
    
