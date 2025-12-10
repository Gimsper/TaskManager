using TaskManagerAPI.Application.Services;
using TaskManagerAPI.Domain.Models;
using TaskManagerAPI.Domain.Context;
using TaskManagerAPI.Utils.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using TaskManagerAPI.Domain.Context;
using TaskManagerAPI.Infrastructure.Repositories.Interfaces;
using TaskManagerAPI.Infrastructure.Repositories;
using TaskManagerAPI.Application.Services.Interfaces;
using ApiGenerico.Application.Services;
var builder = WebApplication.CreateBuilder(args);
IEncryptionService Encrypt = new EncryptionService();
IConfigurationSection seccionConfiguracion = builder.Configuration.GetSection("SectionConfiguration");
IConfigurationSection seccionConnectionStrings = builder.Configuration.GetSection("ConnectionStrings");

builder.Services.Configure<SectionConfiguration>(seccionConfiguracion);
builder.Services.Configure<ConnectionStrings>(seccionConnectionStrings);
var configuracionAppSettings = seccionConfiguracion.Get<SectionConfiguration>();
var configuracionConnectionStrings = seccionConnectionStrings.Get<ConnectionStrings>();

string DecryptConnectionString(string encryptedConnectionString)
{
    return string.IsNullOrEmpty(encryptedConnectionString) ? null : Encrypt.Decrypt(encryptedConnectionString);
}

if (builder.Configuration.GetSection("ConnectionStrings:ConnetionToken").Exists())
{
    string GetConnetionToken = DecryptConnectionString(configuracionConnectionStrings.ConnetionToken);
}

if (builder.Configuration.GetSection("ConnectionStrings:ConnetionGenerico").Exists())
{
    string ConnetionGenerico = DecryptConnectionString(configuracionConnectionStrings.ConnetionGenerico);
    if (!string.IsNullOrEmpty(ConnetionGenerico))
    {
        builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(ConnetionGenerico));
    }
}

builder.Services.AddScoped<IEncryptionService, EncryptionService>();
builder.Services.AddScoped(typeof(IBaseService<>), typeof(BaseService<>));
builder.Services.AddScoped(typeof(BaseRepository<>), typeof(BaseRepository<>));
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<IStateRepository, StateRepository>();

#region Registro dinámico de servicios (Dynamic Services Injection)
var generalServices = typeof(_Service).Assembly.GetTypes()
    .Where(type => !type.Name.StartsWith("_") && type.Name.EndsWith("Service"))
    .ToList();

var serviceInterfaces = generalServices.Where(type => type.IsInterface);
var serviceImplementations = generalServices.Where(type => type.IsClass);

foreach (var implementation in serviceImplementations)
{
    var interfaceName = $"I{implementation.Name}";
    var serviceInterface = serviceInterfaces.FirstOrDefault(i => i.Name == interfaceName);
    if (serviceInterface != null)
    {
        builder.Services.AddScoped(serviceInterface, implementation);
    }
}
#endregion

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
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Authentication:Issuer"],
        ValidAudience = builder.Configuration["Authentication:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Authentication:SecretKey"]))
    };
});

builder.Services.AddAuthorization();

builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("V1", new OpenApiInfo { Title = "TaskManagerAPI", Version = "V1" });
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        In = ParameterLocation.Header,
        Description = "Enter JWT with bearer format like 'Bearer [Token]'"
    });
    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
    opt.CustomSchemaIds(type => type.FullName);
    opt.DocInclusionPredicate((docName, apiDesc) =>
    {
        return apiDesc.GroupName == null || !apiDesc.GroupName.Equals("Hidden", StringComparison.OrdinalIgnoreCase);
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowPolicySecureDomains", policy =>
    {
        policy.WithOrigins(configuracionAppSettings.SecureDomains)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
});

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseCors("AllowPolicySecureDomains");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger(opt => opt.RouteTemplate = "swagger/{documentName}/swagger.json");
    app.UseSwaggerUI(opt => opt.SwaggerEndpoint("V1/swagger.json", "TaskManagerAPI V1"));
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();