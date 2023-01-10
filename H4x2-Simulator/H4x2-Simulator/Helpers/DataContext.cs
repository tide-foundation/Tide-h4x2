namespace H4x2_Simulator.Helpers;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using H4x2_Simulator.Entities;
using System.Numerics;

public class DataContext : DbContext
{
    protected readonly IConfiguration Configuration;

    public DataContext(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlServer(Configuration.GetConnectionString("WebApiDatabase"));
    }

     protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .Property(e => e.OrkUrls)
            .HasConversion(
                v => string.Join(',', v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries));

        // var converter = new ValueConverter<BigInteger, long>(    
        //     model => (long)model,
        //     provider => new BigInteger(provider));

        // modelBuilder.Entity<User>()
        //     .Property(e => e.UserId)
        //     .HasConversion(converter);

        //  modelBuilder.Entity<User>()
        //     .Property(e => e.UserId)
        //     .HasConversion(
        //         v => v.ToByteArray(true, true),
        //         v => new BigInteger(v, true, true));

        // modelBuilder.Entity<User>()
        // .Property(e => e.UserId)
        // .HasConversion<string>();
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Ork> Orks { get; set; }
}