using Microsoft.EntityFrameworkCore;

using System.Globalization;

namespace ConsumerLoggerTest;

public class ApplicationContext : DbContext {

    public ApplicationContext(DbContextOptions<ApplicationContext> options)
    : base(options) {
        CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
        CultureInfo.CurrentUICulture = CultureInfo.InvariantCulture;
        System.Diagnostics.Debug.WriteLine("OrderingContext::ctor ->" + GetHashCode());
    }
    public DbSet<Customer> Customer { get; set; }
}
