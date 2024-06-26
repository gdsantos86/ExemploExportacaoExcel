﻿// <auto-generated />
using ExemploExportacaoExcel.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ExemploExportacaoExcel.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240521174004_inicial")]
    partial class inicial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ExemploExportacaoExcel.Models.Cliente", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Endereco")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Idade")
                        .HasColumnType("int");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Clientes");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Endereco = "Endereço 1.",
                            Idade = 32,
                            Nome = "Antônio Fagundes"
                        },
                        new
                        {
                            Id = 2,
                            Endereco = "Endereço 2",
                            Idade = 29,
                            Nome = "Fernanda Montenegro "
                        },
                        new
                        {
                            Id = 3,
                            Endereco = "Endereço 3",
                            Idade = 22,
                            Nome = "Tony Ramos"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
