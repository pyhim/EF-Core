namespace Homework6.Models;

public class Promotion(
    int promotionId,
    int categoryId,
    int countryId,
    DateTime startDate,
    DateTime endDate,
    string description
)
{
    public int PromotionId { get; set; } = promotionId;
    public int CategoryId { get; set; } = categoryId;
    public int CountryId { get; set; } = countryId;
    public DateTime StartDate { get; set; } = startDate;
    public DateTime EndDate { get; set; } = endDate;
    public string Description { get; set; } = description;
    
    public override string ToString()
    {
        return $"{CategoryId} - {CountryId} - {StartDate} - {EndDate} - {Description}";
    }
}