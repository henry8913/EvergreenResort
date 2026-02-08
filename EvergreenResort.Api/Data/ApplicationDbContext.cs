using Microsoft.EntityFrameworkCore;
using EvergreenResort.Api.Models;

namespace EvergreenResort.Api.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    
    public DbSet<Camera> Camere { get; set; }
}