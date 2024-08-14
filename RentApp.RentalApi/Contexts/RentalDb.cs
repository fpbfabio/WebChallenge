using Microsoft.EntityFrameworkCore;
using RentApp.RentalApi.Models;

namespace RentApp.RentalApi.Contexts;

public class RentalDb(DbContextOptions<RentalDb> options) : DbContext(options)
{
    public DbSet<RentalDatabaseModel> Items { get; set; } = null!;
}