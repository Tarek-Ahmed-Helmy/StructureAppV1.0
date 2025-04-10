using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models;

public class Department
{
    public int Id { get; set; }

    [Required]
    public required string DepartmentName { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    [ValidateNever]
    public ICollection<Employee> Employees { get; set; } = new HashSet<Employee>();
}
