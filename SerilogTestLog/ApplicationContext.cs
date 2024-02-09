using Microsoft.EntityFrameworkCore;

using System.Globalization;

namespace SerilogTestLog;

public class ApplicationContext : DbContext {

    public ApplicationContext(DbContextOptions<ApplicationContext> options)
    : base(options) {
        CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
        CultureInfo.CurrentUICulture = CultureInfo.InvariantCulture;
        System.Diagnostics.Debug.WriteLine("OrderingContext::ctor ->" + this.GetHashCode());
    }
    public DbSet<Customer> Customer { get; set; }
}
