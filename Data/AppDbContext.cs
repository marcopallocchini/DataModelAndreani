using DbClienti.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;


namespace DbClienti.Data
{
  public class AppDbContext : DbContext
  {
    public DbSet<Cliente> Clienti => Set<Cliente>();
    public DbSet<Ente> Enti => Set<Ente>();
    public DbSet<Commessa> Commesse => Set<Commessa>();

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=DbClienti;Trusted_Connection=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<Cliente>()
          .HasIndex(c => c.CLIEN_Codice)
          .IsUnique();

      modelBuilder.Entity<Commessa>()
          .HasIndex(c => c.COMME_Codice)
          .IsUnique();

      // Altre configurazioni...
    }
  }
}
