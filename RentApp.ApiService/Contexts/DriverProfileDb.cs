using Microsoft.EntityFrameworkCore;
using RentApp.ApiService.Models;

namespace RentApp.ApiService.Contexts;

public class DriverProfileDb(DbContextOptions<DriverProfileDb> options) : DbContext(options)
{
    public DbSet<DriverProfile> Items { get; set; } = null!;
}