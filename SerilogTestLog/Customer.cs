using System.ComponentModel.DataAnnotations.Schema;

namespace SerilogTestLog;

[Table("Customer", Schema = "SalesLt")]
public class Customer {
    public int CustomerId { get; set; }
    public string FirstName { get; set; }
}
