using System.ComponentModel.DataAnnotations.Schema;

namespace CountryCityApi.Models;

public class City
{
    public int Id { get; set; }
    public string Name { get; set; }
    public long Population { get; set; }
    public int CountryId { get; set; }
    public bool IsCapitalCity { get; set; }  // Вказувати столицю

    public ICollection<Hotel> Hotels { get; set; } = new List<Hotel>();
}
