namespace OnlineShopping.DTOs.Inputs;

public class AddProductDTO
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int Count { get; set; }
    public double OriginalPrice { get; set; }
    public double Discount { get; set; }
}
