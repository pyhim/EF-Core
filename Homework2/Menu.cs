using System.Runtime.InteropServices;

namespace Homework2;

public enum AggregateFunction
{
    Min,
    Max
}

public class Menu
{
    private readonly ApplicationContext _db = new();

    public Menu()
    {
        while (true)
        {
            Console.WriteLine("\n1. Show Products\n2. Show Product Types\n3. Show Product Providers" +
                              "\n4. Show Product Count\n5. Show Product Price\n6. Show Products By Type Category" +
                              "\n7. Show Products By Provider\n8. Show The Oldest Product" +
                              "\n9. ShowAvgCountByTypeCategory\n10. Exit\n: ");

            var choice = Convert.ToInt32(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    ShowProducts();
                    break;
                case 2:
                    ShowProductTypes();
                    break;
                case 3:
                    ShowProductProviders();
                    break;
                case 4:
                {
                    Console.WriteLine("MIN or MAX: ");
                    string option = Console.ReadLine() ?? string.Empty;
                    ShowProductCount(
                        option.ToLower().Equals("min")
                            ? AggregateFunction.Min
                            : AggregateFunction.Max);
                    break;
                }
                case 5:
                {
                    Console.WriteLine("MIN or MAX: ");
                    string option = Console.ReadLine() ?? string.Empty;
                    ShowProductPrice(
                        option.ToLower().Equals("min")
                            ? AggregateFunction.Min
                            : AggregateFunction.Max);
                    break;
                }
                case 6:
                    ShowProductsByTypeCategory();
                    break;
                case 7:
                    ShowProductsByProvider();
                    break;
                case 8:
                    ShowTheOldestProduct();
                    break;
                case 9:
                    ShowAvgCountByTypeCategory();
                    break;
                case 10:
                    return;
            }
        }
    }

    private void ShowProducts()
    {
        int i = 0;
        foreach (var p in _db.Products)
        {
            i++;
            Console.WriteLine($"{i}. {p}");
        }
    }

    private void ShowProductTypes()
    {
        int i = 0;
        foreach (var p in _db.Products)
        {
            i++;
            Console.WriteLine($"{i}. {p.ProductType}");
        }
    }

    private void ShowProductProviders()
    {
        int i = 0;
        foreach (var p in _db.Products)
        {
            i++;
            Console.WriteLine($"{i}. {p.Provider}");
        }
    }

    private void ShowProductCount(AggregateFunction function)
    {
        switch (function)
        {
            case AggregateFunction.Min:
            {
                var product = _db.Products.OrderBy(p => p.Quantity).First();
                Console.WriteLine($"Min product count: {product}");
                break;
            }
            case AggregateFunction.Max:
            {
                var product = _db.Products.OrderByDescending(p => p.Quantity).First();
                Console.WriteLine($"Max product count: {product}");
                break;
            }
            default:
                return;
        }
    }

    private void ShowProductPrice(AggregateFunction function)
    {
        switch (function)
        {
            case AggregateFunction.Min:
            {
                var product = _db.Products.OrderBy(p => p.Price).First();
                Console.WriteLine($"Min product price: {product}");
                return;
            }
            case AggregateFunction.Max:
            {
                var product = _db.Products.OrderByDescending(p => p.Price).First();
                Console.WriteLine($"Max product price: {product}");
                return;
            }
            default:
                return;
        }
    }

    private void ShowProductsByTypeCategory()
    {
        Console.WriteLine("Enter product type category: ");
        string inputCategory = Console.ReadLine()!;

        var categorizedProducts = _db.Products.Where(p => p.ProductType == inputCategory);
        int i = 0;
        foreach (var p in categorizedProducts)
        {
            i++;
            Console.WriteLine($"{i}. {p}");
        }
    }

    private void ShowProductsByProvider()
    {
        Console.WriteLine("Enter product provider: ");
        string inputProvider = Console.ReadLine()!;

        var categorizedProducts = _db.Products.Where(p => p.Provider == inputProvider);
        int i = 0;
        foreach (var p in categorizedProducts)
        {
            i++;
            Console.WriteLine($"{i}. {p}");
        }
    }

    private void ShowTheOldestProduct()
    {
        var oldestProduct = _db.Products.OrderBy(p => p.DeliveryDate).First();
        Console.WriteLine(oldestProduct);
    }

    private void ShowAvgCountByTypeCategory()
    {
        string[] categories = _db.Products.Select(p => p.ProductType).Distinct().ToArray()!;
        var productsQuantityByCategory = categories.ToDictionary(category => category, dummy => 0M);
        var productsCountByCategory = categories.ToDictionary(category => category, dummy => 0M);

        foreach (var p in _db.Products)
        {
            productsQuantityByCategory[p.ProductType!] += p.Quantity;
            productsCountByCategory[p.ProductType!] += 1;
        }

        foreach (string c in categories)
        {
            decimal avg = productsQuantityByCategory[c] / productsCountByCategory[c];
            Console.WriteLine($"{c}: {avg}");
        }
    }

    ~Menu()
    {
        _db.Dispose();
    }
}