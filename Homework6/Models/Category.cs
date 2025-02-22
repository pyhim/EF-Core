namespace Homework6.Models;

public class Category(int categoryId, string name)
{
    public int CategoryId { get; set; } = categoryId;
    public string Name { get; set; } = name;
    
    public override string ToString()
    {
        return $"{CategoryId}, {Name}";
    }
}