using Microsoft.EntityFrameworkCore;
using server.Models;

namespace server.Data;

public class ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : DbContext(options)
{
    public DbSet<WeatherForecast> WeatherForecasts {get; set;}
}
