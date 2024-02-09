using System.ComponentModel.DataAnnotations.Schema;

namespace ConsumerLoggerTest;

[Table("Customer", Schema = "SalesLt")]
public class Customer {
    public int CustomerId { get; set; }
    public string FirstName { get; set; }
}
