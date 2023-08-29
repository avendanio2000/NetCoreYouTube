using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WhatsappAPI.Entidades;

namespace WhatsappAPI.Data
{
    public class ApplicationDBContext : IdentityDbContext
    {
        public ApplicationDBContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //construye el modelo para el contexto BD
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<AutorLibro>().HasKey(al => new {al.AutorId, al.LibroId});  

        }

        public DbSet<LogEnvio> LogEnvios { get; set; } //se define a que tabla futura a crear va a mapear la entidad existente
    }
}
