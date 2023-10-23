using CatalogoAPI.Logging;
using CatalogoAPI.Context;
using CatalogoAPI.Extensions;
using CatalogoAPI.Filters;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using CatalogoAPI.Repository;
using CatalogoAPI.DTOs.Mappings;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
    .AddControllers()
    .AddJsonOptions
    (
        options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles
    );
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ApiLoggingFilter>();

#region Registrando o serviço AutoMapper

var mappingConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new MappingProfile());
});
IMapper mapper = mappingConfig.CreateMapper();

builder.Services.AddSingleton(mapper);

#endregion

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>(); // Registrando o padrão Unit Of Work

// Obtém a string de conexão
string mySqlConnection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options => options.UseMySql(mySqlConnection, ServerVersion.AutoDetect(mySqlConnection)));

builder.Services
    .AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Logging.AddProvider(new CustomLoggerProvider(new CustomLoggerProviderConfiguration
{
    LogLevel = LogLevel.Information
}));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


// Adiciona o middleware de tratamento de erros
app.ConfigureExceptionHandler();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
