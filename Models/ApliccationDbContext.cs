using Microsoft.EntityFrameworkCore;

namespace SistemaAcademia.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Exercicio> Exercicios { get; set; } = null!;
        public DbSet<Treino> Treinos { get; set; } = null!;
        public DbSet<ItemTreino> ItensTreino { get; set; } = null!;

        // NOVAS LINHAS ADICIONADAS ABAIXO:
        public DbSet<RegistroTreino> RegistrosTreino { get; set; } = null!;
        public DbSet<RegistroItem> RegistrosItens { get; set; } = null!;
    }
}