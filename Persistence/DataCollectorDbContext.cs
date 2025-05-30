using Microsoft.EntityFrameworkCore;
using Sundroid.Homework.Persistence.Entities;

namespace Sundroid.Homework.Persistence;

public sealed class DataCollectorDbContext(DbContextOptions<DataCollectorDbContext> options) : DbContext(options)
{
    public DbSet<DataLogger> DataLoggers { get; set; }
    public DbSet<Inverter> Inverters { get; set; }
    public DbSet<LogItem> LogItems { get; set; }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<decimal>().HavePrecision(18, 4);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DataLogger>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.SerialNumber).HasMaxLength(100);
            entity.HasIndex(e => e.SerialNumber).IsUnique();
        });

        modelBuilder.Entity<Inverter>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne<DataLogger>().WithMany().HasForeignKey(i => i.DataLoggerId);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.SerialNumber).HasMaxLength(100);
            entity.HasIndex(e => e.SerialNumber).IsUnique();
        });

        modelBuilder.Entity<LogItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne<Inverter>().WithMany().HasForeignKey(i => i.InverterId);
            entity.OwnsOne(i => i.Values, i => { i.MapOwnedWithoutPrefix(); });
            entity.HasIndex(e => new { e.InverterId, e.Time }).IsUnique();
        });
    }
}