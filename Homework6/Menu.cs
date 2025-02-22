using Homework6.Models;
using Microsoft.IdentityModel.Tokens;

namespace Homework6;

internal delegate void CurrentMenu();

public class Menu
{
    private readonly Application _app = new();
    private CurrentMenu? _currentMenu;

    public Menu()
    {
        _currentMenu = MainMenu;

        while (_currentMenu is not null)
        {
            try
            {
                _currentMenu();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _currentMenu = MainMenu;
            }
        }
    }

    private void MainMenu()
    {
        Console.WriteLine("1. Show\n2. Add\n3. Edit\n4. Delete\n5. Exit");
        var choice = Convert.ToInt32(Console.ReadLine());

        switch (choice)
        {
            case 1:
                _currentMenu = ShowMenu;
                return;
            case 2:
                _currentMenu = AddMenu;
                return;
            case 3:
                _currentMenu = EditMenu;
                return;
            case 4:
                _currentMenu = DeleteMenu;
                return;
            case 5:
                _currentMenu = null;
                return;
            default:
                Console.Error.WriteLine("Invalid input");
                return;
        }
    }

    private void ShowMenu()
    {
        Console.Write("Show:\n1. Customers\n2. Emails\n3. Categories\n4. Promotions\n" +
                      "5. Cities\n6. Countries\n7. Customers from specific city\n" +
                      "8. Customers from specific country\n9. Cities in country\n" +
                      "10. Categories from customer\n11. Promotions from category\n" +
                      "12. Promotions for country\n13. Exit\n: ");
        var choice = Convert.ToInt32(Console.ReadLine());

        switch (choice)
        {
            case 1:
                DisplayItems(_app.GetCustomers());
                break;
            case 2:
                DisplayItems(_app.GetCustomersEmails());
                break;
            case 3:
                DisplayItems(_app.GetCategories());
                break;
            case 4:
                DisplayItems(_app.GetPromotions());
                break;
            case 5:
                DisplayItems(_app.GetCities());
                break;
            case 6:
                DisplayItems(_app.GetCountries());
                break;
            case 7:
            {
                string city = Console.ReadLine() ?? string.Empty;
                DisplayItems(_app.GetCustomersFromCity(city));
                break;
            }
            case 8:
            {
                string country = Console.ReadLine() ?? string.Empty;
                DisplayItems(_app.GetCustomersFromCountry(country));
                break;
            }
            case 9:
            {
                string country = Console.ReadLine() ?? string.Empty;
                DisplayItems(_app.GetCitiesIn(country));
                break;
            }
            case 10:
            {
                string customerFullName = Console.ReadLine() ?? string.Empty;
                string[] splitName = customerFullName.Split(' ');
                DisplayItems(_app.GetCategoriesFromCustomer(splitName[0], splitName[1]));
                break;
            }
            case 11:
            {
                string category = Console.ReadLine() ?? string.Empty;
                DisplayItems(_app.GetPromotionsForCategory(category));
                break;
            }
            case 12:
            {
                string country = Console.ReadLine() ?? string.Empty;
                DisplayItems(_app.GetPromotionsForCountry(country));
                break;
            }
            case 13:
                _currentMenu = null;
                return;
            default:
                Console.Error.WriteLine("Invalid choice.");
                return;
        }

        _currentMenu = MainMenu;
    }

    private void AddMenu()
    {
        Console.Write("Add:\n1. Customer\n2. Country\n3. City\n4. Category\n5. Promotion\n:");
        var choice = Convert.ToInt32(Console.ReadLine());

        switch (choice)
        {
            case 1:
            {
                Console.Write("Enter first name: ");
                string firstName = Console.ReadLine() ?? string.Empty;
                Console.Write("Enter last name: ");
                string lastName = Console.ReadLine() ?? string.Empty;
                Console.Write("Enter birth date: ");
                var birthDate = DateTime.Parse(Console.ReadLine() ?? string.Empty);
                Console.Write("Enter gender: ");
                var gender = Convert.ToChar(Console.Read());
                Console.Write("Enter email address: ");
                string emailAddress = Console.ReadLine() ?? string.Empty;
                var chosenCity = ChooseMenu(_app.GetCities());

                var customer = new Customer(0, firstName, lastName, birthDate, gender, emailAddress, chosenCity.CityId);
                _app.InsertCustomer(customer);
                break;
            }
            case 2:
            {
                Console.Write("Enter country name: ");
                string countryName = Console.ReadLine() ?? string.Empty;

                var country = new Country(0, countryName);
                _app.InsertCountry(country);
                break;
            }
            case 3:
            {
                Console.Write("Enter city name: ");
                string cityName = Console.ReadLine() ?? string.Empty;
                var chosenCountry = ChooseMenu(_app.GetCountries());

                var city = new City(0, cityName, chosenCountry.CountryId);
                _app.InsertCity(city);
                break;
            }
            case 4:
            {
                Console.Write("Enter category name: ");
                string categoryName = Console.ReadLine() ?? string.Empty;
                var chosenCategory = ChooseMenu(_app.GetCategories());

                var category = new Category(0, categoryName);
                _app.InsertCategory(category);
                break;
            }
            case 5:
            {
                var chosenCategory = ChooseMenu(_app.GetCategories());
                var chosenCountry = ChooseMenu(_app.GetCountries());
                Console.Write("Enter start date: ");
                var startDate = DateTime.Parse(Console.ReadLine() ?? string.Empty);
                Console.Write("Enter end date: ");
                var endDate = DateTime.Parse(Console.ReadLine() ?? string.Empty);
                Console.Write("Enter the description: ");
                string description = Console.ReadLine() ?? string.Empty;

                var promotion =
                    new Promotion(0, chosenCategory.CategoryId, chosenCountry.CountryId, startDate, endDate,
                        description);
                _app.InsertPromotion(promotion);
                break;
            }
            default:
                Console.Error.WriteLine("Invalid choice.");
                return;
        }

        _currentMenu = MainMenu;
    }

    private void EditMenu()
    {
        Console.WriteLine("Edit:\n1. Customer\n2. Country\n3. City\n4. Category\n5. Promotion");
        var choice = Convert.ToInt32(Console.ReadLine());

        switch (choice)
        {
            case 1:
            {
                var chosenCustomer = ChooseMenu(_app.GetCustomers());
                EditCustomerMenu(chosenCustomer);
                break;
            }
            case 2:
            {
                var chosenCountry = ChooseMenu(_app.GetCountries());
                Console.Write("Enter new country name: ");
                string newCountryName = Console.ReadLine() ?? string.Empty;
                _app.UpdateCountry(chosenCountry.CountryId, newCountryName);
                break;
            }
            case 3:
            {
                var chosenCity = ChooseMenu(_app.GetCities());
                EditCityMenu(chosenCity);
                break;
            }
            case 4:
            {
                var chosenCategory = ChooseMenu(_app.GetCategories());
                Console.Write("Enter new category name: ");
                string newCategoryName = Console.ReadLine() ?? string.Empty;
                _app.UpdateCategory(chosenCategory.CategoryId, newCategoryName);
                break;
            }
            case 5:
            {
                var chosenPromotion = ChooseMenu(_app.GetPromotions());
                EditPromotionMenu(chosenPromotion);
                break;
            }
            default:
                Console.Error.WriteLine("Invalid choice.");
                return;
        }
        
        _currentMenu = MainMenu;
    }

    private void DeleteMenu()
    {
        Console.WriteLine("Delete:\n1. Customer\n2. Country\n3. City\n4. Category\n5. Promotion");
        var choice = Convert.ToInt32(Console.ReadLine());

        switch (choice)
        {
            case 1:
            {
                var chosenCustomer = ChooseMenu(_app.GetCustomers());
                _app.DeleteCustomer(chosenCustomer.CustomerId);
                break;
            }
            case 2:
            {
                var chosenCountry = ChooseMenu(_app.GetCountries());
                _app.DeleteCountry(chosenCountry.CountryId);
                break;
            }
            case 3:
            {
                var chosenCity = ChooseMenu(_app.GetCities());
                _app.DeleteCity(chosenCity.CityId);
                break;
            }
            case 4:
            {
                var chosenCategory = ChooseMenu(_app.GetCategories());
                _app.DeleteCategory(chosenCategory.CategoryId);
                break;
            }
            case 5:
            {
                var chosenPromotion = ChooseMenu(_app.GetPromotions());
                _app.DeletePromotion(chosenPromotion.PromotionId);
                break;
            }
            default:
                Console.Error.WriteLine("Invalid choice.");
                return;
        }
        
        _currentMenu = MainMenu;
    }

    private void EditCustomerMenu(Customer customer)
    {
        Console.Write("1. Name\n2. Birth Date\n3. Gender\n4. Email\n5. City\nWhat do you want to edit: ");
        var choice = Convert.ToInt32(Console.ReadLine());

        switch (choice)
        {
            case 1:
            {
                Console.Write("Enter full name: ");
                string newName = Console.ReadLine() ?? string.Empty;
                string[] splitName = newName.Split(' ');
                customer.FirstName = splitName[0];
                customer.LastName = splitName[1];
                break;
            }
            case 2:
            {
                Console.Write("Enter birth date: ");
                var newBirthDate = DateTime.Parse(Console.ReadLine() ?? string.Empty);
                customer.BirthDate = newBirthDate;
                break;
            }
            case 3:
            {
                Console.Write("Enter gender: ");
                var newGender = Convert.ToChar(Console.Read());
                customer.Gender = newGender;
                break;
            }
            case 4:
            {
                Console.Write("Enter email address: ");
                string newEmailAddress = Console.ReadLine() ?? string.Empty;
                customer.Email = newEmailAddress;
                break;
            }
            case 5:
            {
                Console.Write("Enter city name: ");
                string newCityName = Console.ReadLine() ?? string.Empty;
                customer.CityId = ChooseMenu(_app.GetCities()).CityId;
                break;
            }
            default:
                Console.Error.WriteLine("Invalid choice.");
                return;
        }

        _app.UpdateCustomer(customer.CustomerId, customer);
    }

    private void EditCityMenu(City city)
    {
        Console.Write("1. Name\n2. Country\nWhat do you want to edit: ");
        var choice = Convert.ToInt32(Console.ReadLine());

        switch (choice)
        {
            case 1:
            {
                Console.Write("Enter new name: ");
                string newName = Console.ReadLine() ?? string.Empty;
                city.Name = newName;
                break;
            }
            case 2:
            {
                var chosenCountry = ChooseMenu(_app.GetCountries());
                city.CountryId = chosenCountry.CountryId;
                break;
            }
            default:
                Console.Error.WriteLine("Invalid choice.");
                return;
        }

        _app.UpdateCity(city.CityId, city);
    }

    private void EditPromotionMenu(Promotion promotion)
    {
        Console.Write(
            "1. Category\n2. Country\n3. Start Date\n4. End Date\n5. Description\nWhat do you want to edit: ");
        var choice = Convert.ToInt32(Console.ReadLine());

        switch (choice)
        {
            case 1:
            {
                var chosenCategory = ChooseMenu(_app.GetCategories());
                Console.Write("Enter new category name: ");
                string newCategoryName = Console.ReadLine() ?? string.Empty;
                _app.UpdateCategory(chosenCategory.CategoryId, newCategoryName);
                break;
            }
            case 2:
            {
                var chosenCountry = ChooseMenu(_app.GetCountries());
                Console.Write("Enter new country name: ");
                string newCountryName = Console.ReadLine() ?? string.Empty;
                _app.UpdateCountry(chosenCountry.CountryId, newCountryName);
                break;
            }
            case 3:
            {
                Console.Write("Enter start date: ");
                var newStartDate = DateTime.Parse(Console.ReadLine() ?? string.Empty);
                promotion.StartDate = newStartDate;
                break;
            }
            case 4:
            {
                Console.Write("Enter end date: ");
                var newEndDate = DateTime.Parse(Console.ReadLine() ?? string.Empty);
                promotion.EndDate = newEndDate;
                break;
            }
            case 5:
            {
                Console.Write("Enter text description: ");
                string newDescription = Console.ReadLine() ?? string.Empty;
                promotion.Description = newDescription;
                break;
            }
            default:
                Console.Error.WriteLine("Invalid choice.");
                return;
        }
        
        _app.UpdatePromotion(promotion.CategoryId, promotion);
    }

    private static void DisplayItems<T>(IEnumerable<T> items)
    {
        var arrayedItems = items.ToArray();

        if (arrayedItems.Length == 0) throw new NullReferenceException();

        int i = 0;
        foreach (var item in arrayedItems)
        {
            i++;
            Console.WriteLine($"{i}. {item}");
        }
    }

    private static T ChooseMenu<T>(T[] items)
    {
        DisplayItems(items);
        Console.Write("Choose option: ");
        var choice = Convert.ToInt32(Console.ReadLine());

        return items[choice - 1];
    }
}