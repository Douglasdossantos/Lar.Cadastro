
using FluentValidation;
using Lar.Avaliacao.Application.Interfaces;
using Lar.Avaliacao.Application.Services;
using Lar.Avaliacao.Infra.Percistence;
using Lar.Avaliacao.Infra.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text.Json.Serialization;
using System.Text;
using Lar.Avaliacao.Api.Filters;
using Lar.Avaliacao.Application.Validators;
using Lar.Avaliacao.Api.Middlewares;
using Microsoft.EntityFrameworkCore;
using Lar.Avaliacao.Infra;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft.AspNetCore", Serilog.Events.LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore", Serilog.Events.LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/cadastro-api-.txt", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 14)
    .CreateLogger();



try
{
    Log.Information("Iniciando a Cadastro API...");

    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog();


    builder.Services.AddControllers(options =>
    {
        options.Filters.Add<ValidationFilter>();
    })
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

    builder.Services.Configure<Microsoft.AspNetCore.Mvc.ApiBehaviorOptions>(options =>
    {
        options.SuppressModelStateInvalidFilter = true;
    });

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
        {
            Title = "Cadastro API",
            Version = "v1",
            Description = "CRUD de Pessoa, Telefone e Endereco, com autenticação JWT e validação via FluentValidation."
        });
        
        options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = Microsoft.OpenApi.Models.ParameterLocation.Header,
            Description = "Informe:{seu token}"
        });

        options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
        {
            {
                new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Reference = new Microsoft.OpenApi.Models.OpenApiReference
                    {
                        Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },   Array.Empty<string>()
            }
        });
    });

    // FluentValidation: registra todos os validadores do assembly da Application
    builder.Services.AddValidatorsFromAssemblyContaining<CriarPessoaRequestValidator>();

    // Application + Infrastructure (SQLite, Unit of Work, JWT, hashing, clock)
    builder.Services.AddScoped<IPessoaService, PessoaService>();
    builder.Services.AddScoped<ITelefoneService, TelefoneService>();
    builder.Services.AddScoped<IEnderecoService, EnderecoService>();
    builder.Services.AddScoped<IAutenticacaoService, AutenticacaoService>();
    builder.Services.AddInfrastructure(builder.Configuration);

    // ---------- Autenticação JWT ----------
    var jwtSection = builder.Configuration.GetSection(JwtSettings.SectionName);
    var jwtSettings = jwtSection.Get<JwtSettings>() ?? new JwtSettings();

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
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
                ClockSkew = TimeSpan.FromMinutes(1)
            };
            });

        builder.Services.AddAuthorization();

        var app = builder.Build();

        // Loga cada requisição (método, rota, status, tempo) automaticamente.
        app.UseSerilogRequestLogging();

        // ---------- Migrations automáticas em desenvolvimento ----------
        // Em produção, prefira rodar "dotnet ef database update" manualmente/via pipeline.
        if (app.Environment.IsDevelopment())
        {
            using var scope = app.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            db.Database.Migrate();
        }

        // ---------- Pipeline HTTP ----------

        app.UseMiddleware<ExceptionMiddleware>();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // UseHttpsRedirection é pulado no ambiente "Testing": o TestServer usado pelo
        // WebApplicationFactory não expõe um endpoint HTTPS real, e o redirect 307
        // quebraria as chamadas dos testes de integração.
        if (!app.Environment.IsEnvironment("Testing"))
        {
            app.UseHttpsRedirection();
        }

        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "A aplicação falhou ao iniciar.");
}
finally
{
    Log.CloseAndFlush();
}

public partial class Program { }


//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.

//builder.Services.AddControllers();
//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();

//app.UseAuthorization();

//app.MapControllers();

//app.Run();
