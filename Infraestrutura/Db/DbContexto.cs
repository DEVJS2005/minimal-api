/* Todo o DbContexto é uma extenção da conexão com mysql e o entity framework, nele nós pegamos as classe prontas da pasta "Entidades e setamos elas na base de dados
Através desse arquivo. */

using Microsoft.EntityFrameworkCore;
using minimal_api.Dominio.Entidades;

namespace minimal_api.Infraestrutura.Db;

    public class DbContexto : DbContext
    {
        
        private readonly IConfiguration _configuracaoAppSettings;
        public DbContexto( IConfiguration configuracaoAppSettings) 
        { 
            _configuracaoAppSettings = configuracaoAppSettings;
        }
        
        // Classe para gerar a tabela "Administradores dentro do banco de dados.
        public DbSet<Administrador> Administradores { get; set; } = default!;
        // Override para criar um objeto no banco de dados com os parametros apresentados.
        protected override void OnModelCreating(ModelBuilder modelBuilder){
            modelBuilder.Entity<Administrador>().HasData(
                new Administrador{
                    Id = 1,
                    Email = "administrador@teste.com",
                    Senha = "123456",
                    Perfil = "Adm",
                }
            );
        }
        // classe para gerar a tabela Veiculos.
        public DbSet<Veiculo> Veiculos { get; set; } = default!;
        
        // Função que faz a configuração da string do mysql dentro da aplicação
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var stringConexao = _configuracaoAppSettings.GetConnectionString("mysql")?.ToString();
                if (!string.IsNullOrEmpty(stringConexao))
                {
                    optionsBuilder.UseMySql(stringConexao,
                    ServerVersion.AutoDetect(stringConexao));
                }
            }
        }
    }

