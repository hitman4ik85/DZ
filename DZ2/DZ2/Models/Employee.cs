using System.ComponentModel.DataAnnotations;

namespace DZ2.Models;

public class Employee  //клас робітника
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Поле 'Ім'я' є обов'язковим.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Поле 'Прізвище' є обов'язковим.")]
    public string Surname { get; set; }

    [Required(ErrorMessage = "Поле 'Дата народження' є обов'язковим.")]
    [DataType(DataType.Date)]
    public DateTime? DateOfBirth { get; set; }

    [Required(ErrorMessage = "Поле 'Стать' є обов'язковим.")]
    public string Gender { get; set; }

    [Required(ErrorMessage = "Поле 'Посада' є обов'язковим.")]
    public string Position { get; set; }

    [Required(ErrorMessage = "Поле 'Зарплата' є обов'язковим.")]
    [Range(1, double.MaxValue, ErrorMessage = "Зарплата повинна бути більшою за 0.")]
    public double? Salary { get; set; }
}
