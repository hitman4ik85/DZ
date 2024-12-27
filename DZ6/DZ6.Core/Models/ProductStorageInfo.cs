using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DZ6.Core.Models;

public class ProductStorageInfo
{
    public int Id { get; set; }

    [Range(0, int.MaxValue)] //мінімум 0 на складі
    public int Count { get; set; } //скільки в нас таких продуктів на складі
}
