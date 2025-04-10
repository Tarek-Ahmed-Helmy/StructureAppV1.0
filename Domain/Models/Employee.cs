using System.ComponentModel.DataAnnotations;

namespace Domain.Models;

public class Employee
{
    public int Id { get; set; }

    [Required, MaxLength(20)]
    public required string FirstName { get; set; }

    [Required, MaxLength(20)]
    public required string LastName { get; set; }

    [Range(18, 65, ErrorMessage = "Age must be between 18 and 65.")]
    public int? Age { get; set; }

    [DataType(DataType.Currency)]
    public decimal? Salary {get; set;}

    [Required]
    public required string Title { get; set; }
    public DateTime? HireDate { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    [Display(Name = "Department")]
    public int? DepartmentId { get; set; }
    public Department? Department { get; set; }
}
