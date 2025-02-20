using Microsoft.EntityFrameworkCore;

namespace Homework2;

public class Product
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? ProductType { get; set; }
    public string? Provider { get; set; }
    public int Quantity { get; set; }
    public double Price { get; set; }
    public DateTime DeliveryDate { get; set; }

    public override string ToString()
    {
        return $"{Name} - {Price} - {ProductType} - {Quantity} - {Provider} - {DeliveryDate}";
    }
}