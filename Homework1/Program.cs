namespace EF_Core;

internal class Program
{
    private static void Main(string[] args)
    {
        using var db = new ApplicationContext();

        while (true)
        {
            Console.WriteLine("1. Add books.\n2. Show books.");
            var choice = Convert.ToInt32(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    string? author, name, category, publisher;
                    int publishDate, pages;
                    Console.WriteLine("Enter author: ");
                    author = Console.ReadLine();
                    Console.WriteLine("Enter name: ");
                    name = Console.ReadLine();
                    Console.WriteLine("Enter category: ");
                    category = Console.ReadLine();
                    Console.WriteLine("Enter publisher: ");
                    publisher = Console.ReadLine();
                    Console.WriteLine("Enter pages: ");
                    pages = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine("Enter publish date year: ");
                    publishDate = Convert.ToInt32(Console.ReadLine());

                    db.Books.Add(new Book
                    {
                        Author = author,
                        Name = name,
                        Category = category,
                        Publisher = publisher,
                        Pages = pages,
                        PublishDate = publishDate
                    });
                    db.SaveChanges();
                    break;
                case 2:
                    var books = db.Books.ToList();

                    foreach (var book in books)
                    {
                        Console.WriteLine(book.ToString());
                    }
                    break;
                default:
                    Console.Error.WriteLine("Invalid input.");
                    break;
            }
        }
    }
}