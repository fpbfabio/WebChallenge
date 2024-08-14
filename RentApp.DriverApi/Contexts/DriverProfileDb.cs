using Microsoft.EntityFrameworkCore;
using RentApp.DriverApi.Models;

namespace RentApp.DriverApi.Contexts;

public class DriverProfileDb(DbContextOptions<DriverProfileDb> options) : DbContext(options)
{
    public DbSet<DriverProfile> Items { get; set; } = null!;
}