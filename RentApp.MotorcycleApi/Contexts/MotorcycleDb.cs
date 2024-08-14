using Microsoft.EntityFrameworkCore;
using RentApp.MotorcycleApi.Models;

namespace RentApp.MotorcycleApi.Contexts;

public class MotorcycleDb(DbContextOptions<MotorcycleDb> options) : DbContext(options)
{
    public DbSet<MotorcycleDatabaseModel> Items { get; set; } = null!;
}