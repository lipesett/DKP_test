using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using teste_api.Models;
using teste_api.Models.Entites;

namespace teste_api.Data
{
    public class AppDbContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public AppDbContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(Configuration.GetConnectionString("WebApiDatabase"));

        }

        public DbSet<Aluno> Alunos { get; set; }
        public DbSet<Materia> Materias { get; set; }
        public DbSet<AlunoXMaterias> AlunoXMaterias { get; set; }
    }
}
