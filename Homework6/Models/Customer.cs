namespace Homework6.Models;

public class Customer(
    int customerId,
    string firstName,
    string lastName,
    DateTime birthDate,
    char? gender,
    string email,
    int cityId
)
{
    public int CustomerId { get; set; } = customerId;
    public string FirstName { get; set; } = firstName;
    public string LastName { get; set; } = lastName;
    public DateTime BirthDate { get; set; } = birthDate;
    public char? Gender { get; set; } = gender;
    public string Email { get; set; } = email;
    public int CityId { get; set; } = cityId;

    public override string ToString()
    {
        return $"{FirstName} {LastName} - {BirthDate.ToShortDateString()} - {Gender} - {Email}";
    }
}