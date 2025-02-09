namespace CountryCityApi.Models;

public class Country
{
    public int Id { get; set; }
    public string Name { get; set; }
    public long Population { get; set; }
    public string MonetaryUnit { get; set; } // Наприклад: "USD", "EUR", "UAH"
    public string Iso { get; set; } // Код країни (ISO)

    public ICollection<City> Cities { get; set; } = new List<City>();
}
