using Microsoft.EntityFrameworkCore;
using WeatherApp.API.Models;

namespace WeatherApp.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }= null!;
    
    public DbSet<SearchHistory> SearchHistories { get; set; } = null!;
}
