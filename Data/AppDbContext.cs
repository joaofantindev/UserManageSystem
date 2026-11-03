using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CompleteSystem.Models;
using Microsoft.EntityFrameworkCore;
using UserManageSystem.Models;

namespace CompleteSystem.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

        public DbSet<Usuario> Usuarios => Set<Usuario>();
        public DbSet<Produto> Produtos => Set<Produto>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //ENTIDADE USUARIO
            modelBuilder.Entity<Usuario>(entidade =>
            {
                entidade.ToTable("Usuarios");
                entidade.HasKey(u => u.Id);

                entidade.Property(u => u.Nome).HasMaxLength(100).IsRequired();

                entidade.HasIndex(u => u.Email).IsUnique();

                entidade.Property(u => u.SenhaHash).HasMaxLength(255).IsRequired();
                
            });
            

            //ENTIDADE PRODUTO
            modelBuilder.Entity<Produto>(entidade =>
            {
                entidade.ToTable("Produtos");
                entidade.HasKey(p => p.Id);

                entidade.Property(p => p.Nome)
                .HasMaxLength(100)
                .IsRequired();

                entidade.Property(p => p.Marca)
                .HasMaxLength(100)
                .IsRequired();

                entidade.Property(p => p.Preco)
                .HasPrecision(11, 2)
                .IsRequired();

                entidade.Property(p => p.CompradoEm)
                .IsRequired(); 
            });
        }
    }
}