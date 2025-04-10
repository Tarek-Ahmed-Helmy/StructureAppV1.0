using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Presentation.ViewModels;

public class EmployeeVM
{
    public int Id { get; set; }

    [Required(ErrorMessage = "First Name is Required!")]
    [DisplayName("First Name")]
    [MaxLength(20, ErrorMessage = "The first name must be less than 20 characters.")]
    [MinLength(2, ErrorMessage = "The minemum length of first name is 2 characters.")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Last Name is Required!")]
    [DisplayName("Last Name")]
    [MaxLength(20, ErrorMessage = "The last name must be less than 20 characters.")]
    [MinLength(2, ErrorMessage = "The minemum length of last name is 2 characters.")]
    public string LastName { get; set; }

    [Range(20, 60, ErrorMessage = "Age Must Be Between 20 and 60")]
    public int? Age { get; set; }

    [DataType(DataType.Currency)]
    [Range(1000, 100000, ErrorMessage = "Salary Must Be Between 1000 and 100000")]
    public int? Salary { get; set; }

    [Required(ErrorMessage = "Title is Required!")]
    public string Title { get; set; }

    public DateTime? HireDate { get; set; }

    [DisplayName("Department")]
    public int? DepartmentId { get; set; }

    [ValidateNever]
    public string? DepartmentName { get; set; }

    [ValidateNever]
    public IEnumerable<SelectListItem>? DepartmentList { get; set; }
}
