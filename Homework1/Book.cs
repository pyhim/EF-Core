namespace EF_Core;

public class Book
{
    public int Id { get; set; }
    public string? Author { get; set; }
    public string? Name { get; set; }
    public string? Category { get; set; }
    public string? Publisher { get; set; }
    public int PublishDate { get; set; }
    public int Pages { get; set; }

    public override string ToString()
    {
        return $"Author: {Author}, Name: {Name}, Category: {Category},\n" +
               $"Publisher: {Publisher}, Pages: {Pages}, Publish Date: {PublishDate}";
    }
}