using Lar.Avaliacao.Application.Interfaces;
using Lar.Avaliacao.Domain.Interfaces;
using Lar.Avaliacao.Infra.Percistence.Repositories;
using Lar.Avaliacao.Infra.Percistence;
using Lar.Avaliacao.Infra.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace Lar.Avaliacao.Infra
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? "Data Source=cadastro.db";

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(connectionString));

            // Repositórios
            services.AddScoped<IPessoaRepository, PessoaRepository>();
            services.AddScoped<ITelefoneRepository, TelefoneRepository>();
            services.AddScoped<IEnderecoRepository, EnderecoRepository>();
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));
            services.AddSingleton<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            services.AddSingleton<IClock, SystemClock>();

            return services;
        }
    }
}
