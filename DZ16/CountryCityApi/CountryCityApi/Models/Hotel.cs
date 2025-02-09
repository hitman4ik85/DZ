using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CountryCityApi.Models;

public class Hotel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Stars { get; set; }
    public decimal PriceForOneNight { get; set; }

    [ForeignKey(nameof(City))]
    public int CityId { get; set; }
    public City City { get; set; } // Навігаційна властивість
}
