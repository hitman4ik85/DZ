using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DZ6.Core.Models;

public class Order
{
    public int Id { get; set; } //id замовлення
    public Client Client { get; set; } //клієнта
    public ICollection<Product> Products { get; set; } //колекцію продуктів
}
