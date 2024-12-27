using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DZ6.Core.Models;

public class Product
{
    public int Id { get; set; } //id
    public string Name { get; set; } //назва
    public decimal Price { get; set; } //ціна
    public string Description { get; set; } //опис продукту

    public ProductStorageInfo ProductStorageInfo { get; set; }
}
