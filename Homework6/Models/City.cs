namespace Homework6.Models;

public class City(int cityId, string name, int countryId)
{
    public int CityId { get; set; } = cityId;
    public string Name { get; set; } = name;
    public int CountryId { get; set; } = countryId;

    public override string ToString()
    {
        return $"{Name} - {CountryId}";
    }
}