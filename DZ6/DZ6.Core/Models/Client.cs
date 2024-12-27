using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DZ6.Core.Models;

public class Client
{
    public int Id { get; set; }  //для збереження в Базі Даних
    public string FirstName { get; set; } //ім'я
    public string LastName { get; set; } //прізвище нашого клента
    public DateTime DateOfBirth { get; set; } //дату народження

    public ICollection<Order> Orders { get; set; } //збереження покупок через кошик
}
