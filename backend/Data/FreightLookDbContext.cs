using FreightLook.Api.Entities;
using Microsoft.EntityFrameworkCore;
namespace FreightLook.Api.Data;
public sealed class FreightLookDbContext(DbContextOptions<FreightLookDbContext> options) : DbContext(options) {
 public DbSet<Airline> Airlines=>Set<Airline>(); public DbSet<Airport> Airports=>Set<Airport>(); public DbSet<RateSheet> RateSheets=>Set<RateSheet>(); public DbSet<Rate> Rates=>Set<Rate>(); public DbSet<RateStaging> RateStaging=>Set<RateStaging>();
 protected override void OnModelCreating(ModelBuilder modelBuilder) {
  modelBuilder.Entity<Airline>().HasIndex(x=>x.Name).IsUnique(); modelBuilder.Entity<Airport>().HasIndex(x=>x.Code).IsUnique();
  foreach(var p in new[]{"MinimumCharge","NormalRate","Rate45","Rate100","Rate250","Rate300","Rate500","Rate1000","AmsAwb","AmsHawb","AmsEawb","AmsEhawb"}) modelBuilder.Entity<Rate>().Property<decimal?>(p).HasPrecision(12,3);
  modelBuilder.Entity<Rate>().HasOne(x=>x.OriginAirport).WithMany(x=>x.OriginRates).HasForeignKey(x=>x.OriginAirportId).OnDelete(DeleteBehavior.Restrict);
  modelBuilder.Entity<Rate>().HasOne(x=>x.DestinationAirport).WithMany(x=>x.DestinationRates).HasForeignKey(x=>x.DestinationAirportId).OnDelete(DeleteBehavior.Restrict);
  modelBuilder.Entity<Rate>().HasOne(x=>x.Airline).WithMany(x=>x.Rates).HasForeignKey(x=>x.AirlineId);
  modelBuilder.Entity<Rate>().HasOne(x=>x.RateSheet).WithMany(x=>x.Rates).HasForeignKey(x=>x.RateSheetId);
  modelBuilder.Entity<RateStaging>().HasOne(x=>x.RateSheet).WithMany(x=>x.StagingRows).HasForeignKey(x=>x.RateSheetId);
  modelBuilder.Entity<RateStaging>().HasIndex(x=>new{x.RateSheetId,x.RowNumber}).IsUnique();
 }
}
