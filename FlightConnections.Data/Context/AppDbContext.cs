using FlightConnections.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace FlightConnections.Data.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<FlightRoutes> Connections { get; set; }
    }
}
