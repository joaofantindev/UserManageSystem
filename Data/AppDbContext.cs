using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CompleteSystem.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

        public DbSet<Usuario> Usuarios => Set<Usuario>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>(entidade =>
            {
                entidade.ToTable("Usuarios");
                entidade.HasKey(u => u.Id);

                entidade.Property(u => u.Nome).HasMaxLength(100).IsRequired();

                entidade.HasIndex(u => u.Email).IsUnique();

                entidade.Property(u => u.SenhaHash).HasMaxLength(255).IsRequired();
                
            });
        }
    }
}