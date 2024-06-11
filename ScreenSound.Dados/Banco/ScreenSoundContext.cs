
using Microsoft.EntityFrameworkCore;

using ScreenSound.Modelos.Modelos;

namespace ScreenSound.Banco
{
    public class ScreenSoundContext : DbContext
    {
        public DbSet<Artista> Artistas { get; set; } //identifica a tabela
        public DbSet<Musica> Musicas { get; set; }
        public DbSet<Genero> Generos { get; set; }

		private string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=SreenSoundV0;Integrated Security=True;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) //->EntityFramework
        {
            optionsBuilder.UseSqlServer(connectionString).UseLazyLoadingProxies();
        }
        //public SqlConnection ObterConexao() ->ADO.Net
        //{
        //    return new SqlConnection(connectionString);
        //}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Musica>()
                .HasMany(c => c.Generos)
                .WithMany(c => c.Musicas);
        }

    }
}
