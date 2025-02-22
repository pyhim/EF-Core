using Dapper;
using Homework6.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration.Json;

namespace Homework6;

public class Application
{
    private SqlConnection _db;

    public Application()
    {
        var cfgSource = new JsonConfigurationSource { Path = "appsettings.json" };
        var config = new ConfigurationBuilder().Add(cfgSource).Build();

        string connectionString = config.GetConnectionString("DefaultConnection")!;

        _db = new SqlConnection(connectionString);
        _db.Open();
    }

    public int GetId(string idField, string fromTable, string where)
    {
        return _db.ExecuteScalar<int>($"SELECT {idField} FROM {fromTable} WHERE {where}");
    }

    public Customer[] GetCustomers()
    {
        return _db.Query<Customer>("SELECT * FROM Customers").ToArray();
    }

    public string[] GetCustomersEmails()
    {
        return _db.Query<string>("SELECT Email FROM Customers").ToArray();
    }

    public Category[] GetCategories()
    {
        return _db.Query<Category>("SELECT * FROM Categories").ToArray();
    }

    public Category[] GetCategoriesFromCustomer(string firstName, string lastName)
    {
        string query = "SELECT Categories.CategoryID, Categories.Name FROM Categories " +
                       "JOIN CustomersCategories CC on Categories.CategoryID = CC.CategoryID " +
                       "JOIN Customers C on CC.CustomerID = C.CustomerID " +
                       $"WHERE C.FirstName = '{firstName}' AND C.LastName = '{lastName}'";

        return _db.Query<Category>(query).ToArray();
    }

    public Promotion[] GetPromotions()
    {
        return _db.Query<Promotion>("SELECT * FROM Promotions").ToArray();
    }

    public Promotion[] GetPromotionsForCategory(string categoryName)
    {
        string query =
            "SELECT PromotionID, P.CategoryID, P.CountryID, " +
            "P.StartDate, P.EndDate, P.Description FROM Promotions AS P " +
            "JOIN Categories C on C.CategoryID = P.CategoryID " +
            $"WHERE C.Name = '{categoryName}'";

        return _db.Query<Promotion>(query).ToArray();
    }

    public Promotion[] GetPromotionsForCountry(string countryName)
    {
        string query =
            "SELECT PromotionID, P.CategoryID, P.CountryID, " +
            "P.StartDate, P.EndDate, P.Description FROM Promotions AS P " +
            "JOIN dbo.Countries C on C.CountryID = P.CountryID " +
            $"WHERE C.Name = '{countryName}'";

        return _db.Query<Promotion>(query).ToArray();
    }

    public City[] GetCities()
    {
        return _db.Query<City>("SELECT * FROM Cities").ToArray();
    }

    public City[] GetCitiesIn(string countryName)
    {
        string query = "SELECT * FROM Cities " +
                       "JOIN dbo.Countries C on C.CountryID = Cities.CountryID " +
                       $"WHERE C.Name = '{countryName}'";

        return _db.Query<City>(query).ToArray();
    }

    public Country[] GetCountries()
    {
        return _db.Query<Country>("SELECT * FROM Countries").ToArray();
    }

    public Customer[] GetCustomersFromCity(string cityName)
    {
        var cityId = _db.Query<int>($"SELECT CityID FROM Cities WHERE Name = '{cityName}'");

        return _db.Query<Customer>($"SELECT * FROM Customers WHERE CityID = {cityId}").ToArray();
    }

    public Customer[] GetCustomersFromCountry(string countryName)
    {
        var countryId = _db.Query<int>($"SELECT CountryID FROM Countries WHERE Name = '{countryName}'");

        return _db.Query<Customer>($"SELECT * FROM Customers WHERE CountryID = {countryId}").ToArray();
    }

    public void InsertCustomer(Customer customer)
    {
        string query =
            "INSERT INTO Customers(FirstName, LastName, BirthDate, Gender, Email, CityID) " +
            $"VALUES ('{customer.FirstName}', '{customer.LastName}', '{customer.BirthDate.ToShortDateString()}', " +
            $"'{customer.Gender}', '{customer.Email}', {customer.CityId})";

        _db.Execute(query);
    }

    public void InsertCountry(Country country)
    {
        string query = $"INSERT INTO Countries(Name) VALUES ('{country.Name}')";

        _db.Execute(query);
    }

    public void InsertCity(City city)
    {
        string query = $"INSERT INTO Cities(Name) VALUES ('{city.Name}')";

        _db.Execute(query);
    }

    public void InsertCategory(Category category)
    {
        string query = $"INSERT INTO Categories(Name) VALUES ('{category.Name}')";

        _db.Execute(query);
    }

    public void InsertPromotion(Promotion promotion)
    {
        string query =
            "INSERT INTO Promotions(CategoryID, CountryID, StartDate, EndDate, Description) " +
            $"VALUES ({promotion.CategoryId}, {promotion.CountryId}, '{promotion.StartDate.ToShortDateString()}', " +
            $"{promotion.EndDate.ToShortDateString()}, {promotion.Description})";

        _db.Execute(query);
    }

    public void UpdateCustomer(int oldCustomerId, Customer newCustomer)
    {
        string query =
            "UPDATE Customers SET " +
            $"FirstName = '{newCustomer.FirstName}', LastName = '{newCustomer.LastName}', " +
            $"BirthDate = '{newCustomer.BirthDate.ToShortDateString()}', Gender = '{newCustomer.Gender}', " +
            $"Email = '{newCustomer.Email}'" +
            $"WHERE CustomerID = {oldCustomerId}";

        _db.Execute(query);
    }

    public void UpdateCountry(int oldCountryId, string newCountryName)
    {
        string query = "UPDATE Countries SET " +
                       $"Name = '{newCountryName}' " +
                       $"WHERE CountryID = {oldCountryId}";

        _db.Execute(query);
    }

    public void UpdateCity(int oldCityId, City newCity)
    {
        string query = "UPDATE Cities SET " +
                       $"Name = '{newCity.Name}', " +
                       $"CountryID = {newCity.CountryId}" +
                       $"WHERE CityID = {oldCityId}";

        _db.Execute(query);
    }

    public void UpdateCategory(int oldCategoryId, string newCategoryName)
    {
        string query = "UPDATE Categories SET " +
                       $"Name = '{newCategoryName}' " +
                       $"WHERE CategoryID = {oldCategoryId}";

        _db.Execute(query);
    }

    public void UpdatePromotion(int oldPromotionId, Promotion newPromotion)
    {
        string query = "UPDATE Promotions SET " +
                       $"CategoryID = {newPromotion.CategoryId}, " +
                       $"CountryID = {newPromotion.CountryId}, " +
                       $"StartDate = '{newPromotion.StartDate}', " +
                       $"EndDate = '{newPromotion.EndDate}', " +
                       $"Description = {newPromotion.Description} " +
                       $"WHERE PromotionID = {oldPromotionId}";

        _db.Execute(query);
    }

    public void DeleteCustomer(int customerId)
    {
        string query = $"DELETE FROM Customers WHERE CustomerID = {customerId}";

        _db.Execute(query);
    }

    public void DeleteCountry(int countryId)
    {
        string query = $"DELETE FROM Countries WHERE CountryID = {countryId}";

        _db.Execute(query);
    }

    public void DeleteCity(int cityId)
    {
        string query = $"DELETE FROM Cities WHERE CityID = {cityId}";

        _db.Execute(query);
    }

    public void DeleteCategory(int categoryId)
    {
        string query = $"DELETE FROM Categories WHERE CategoryID = {categoryId}";

        _db.Execute(query);
    }

    public void DeletePromotion(int promotionId)
    {
        string query = $"DELETE FROM Promotions WHERE PromotionID = {promotionId}";

        _db.Execute(query);
    }

    ~Application()
    {
        _db.Close();
    }
}