using Microsoft.EntityFrameworkCore;
using MetricsApp.Models;

namespace MetricsApp.Data;

public class CustomerContext : DbContext
{
    public CustomerContext(DbContextOptions<CustomerContext> options) : base(options) { }

    public DbSet<Customer> Customer { get; set; } = default!;
}
