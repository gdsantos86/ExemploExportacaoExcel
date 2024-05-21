using ExemploExportacaoExcel.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace ExemploExportacaoExcel.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Cliente>()
                .HasData(new List<Cliente> {
                new Cliente{Id = 1, Nome = "Antônio Fagundes", Idade = 32, Endereco = "Endereço 1." },
                new Cliente {Id = 2, Nome = "Fernanda Montenegro ", Idade = 29, Endereco = "Endereço 2" },
                new Cliente{Id = 3,  Nome = "Tony Ramos", Idade = 22, Endereco = "Endereço 3" }
                });
        }

        public DbSet<Cliente> Clientes { get; set; }

    }
}
