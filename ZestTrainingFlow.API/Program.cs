//using AutoMapper;
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
//using ZestTrainingFlow.API.Controllers;
using ZestTrainingFlow.Infrastructure.Data;
using Microsoft.Extensions.Configuration;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

//builder.Services.AddMediatR(cfg =>
//{
//    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
//});
//builder.Services.AddMediatR(typeof(Program));

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
});//Automapper



//builder.Services.AddSwaggerGen(options =>
//{
//    options.SwaggerDoc("v1", new OpenApiInfo
//    {
//        Title = "ZestTrainingFlow API",
//        Version = "v1",
//        Description = "Zest Training Task API"
//    });

//options.AddSecurityDefinition("Bearer",
//        new OpenApiSecurityScheme
//        {
//            Name = "Authorization",
//            Type = SecuritySchemeType.Http,
//            Scheme = "bearer",
//            BearerFormat = "JWT",
//            In = ParameterLocation.Header,
//            Description = "Enter JWT Token"
//        });

//    options.AddSecurityRequirement(
//        new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
//        {
//            {
//                new Microsoft.OpenApi.Models.OpenApiSecurityScheme
//                {
//                    Reference = new Microsoft.OpenApi.Models.OpenApiReference
//                    { 
//                        Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
//                            Id = "Bearer"
//                    }
//                },
//            Array.Empty<string>()

//                //new string[] {}
//            }
//        });

//});
//builder.Services.AddSwaggerGen(options =>
//{
//    options.AddSecurityDefinition("Bearer",
//        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
//        {
//            Name = "Authorization",
//            Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
//            Scheme = "bearer",
//            BearerFormat = "JWT",
//            In = Microsoft.OpenApi.Models.ParameterLocation.Header,
//            Description = "Enter JWT Token"
//        });

//    options.AddSecurityRequirement(
//        new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
//        {
//            {
//                new Microsoft.OpenApi.Models.OpenApiSecurityScheme
//                {
//                    Reference =
//                        new Microsoft.OpenApi.Models.OpenApiReference
//                        {
//                            Type =
//                                Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
//                            Id = "Bearer"
//                        }
//                },
//                new string[] {}
//            }
//        });
//});
//builder.Services.AddSwaggerGen(options =>
//{
//    options.SwaggerDoc("v1", new OpenApiInfo
//    {
//        Title = "ZestTrainingFlow API",
//        Version = "v1",
//        Description = "Zest Training Task API"
//    });
//});

builder.Services.AddDbContext<MyDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DBCS"), b => b.MigrationsAssembly("ZestTrainingFlow.API")));
//Console.WriteLine("JWT KEY = " + builder.Configuration["Jwt:Key"]);//add this line to check swagger will open or not
//var jwtSettings = builder.Configuration.GetSection("Jwt");

//var key = jwtSettings["Key"];

//if (string.IsNullOrEmpty(key))
//{
//    throw new Exception("JWT Key is NULL");
//}
//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme =
//        JwtBearerDefaults.AuthenticationScheme;

//    options.DefaultChallengeScheme =
//        JwtBearerDefaults.AuthenticationScheme;

//}).AddJwtBearer(options =>
//{


//    options.TokenValidationParameters =
//        new TokenValidationParameters
//        {
//            ValidateIssuer = true,

//            ValidateAudience = true,

//            ValidateLifetime = true,

//            ValidateIssuerSigningKey = true,

//            ValidIssuer =
//                builder.Configuration["Jwt:Issuer"],

//            ValidAudience =
//                builder.Configuration["Jwt:Audience"],

//            IssuerSigningKey = 
//                new SymmetricSecurityKey(
//                    Encoding.UTF8.GetBytes((key)))
//                        //builder.Configuration["Jwt:Key"]))
//        };
//});
//builder.Services.AddMediatR(cfg =>
//{
//    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
//}); //automapper

//var builderserilog = WebApplication.CreateBuilder(args); //serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("Logs/log.txt",
        rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

//Log.Logger = new LoggerConfiguration()
//    .MinimumLevel.Debug()
//    .WriteTo.Console()
//    .WriteTo.File("Logs/log.txt",
//rollingInterval: RollingInterval.Day
//).CreateLogger();
//Log.Information("Application Started");
//builder.Host.UseSerilog();


var app = builder.Build();
app.UseSerilogRequestLogging();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseSwagger();
//app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseSerilogRequestLogging();

app.UseCors("MyPolicy");
app.UseAuthorization();
//app.UseMiddleware<CustomMiddleware>();

app.MapControllers();

app.Run();
