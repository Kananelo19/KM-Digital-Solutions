using System.ComponentModel.DataAnnotations;

namespace KM_Digital_Solutions.Models;

public class ContactFormViewModel
{
    [Required]
    [StringLength(80)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(120)]
    [Display(Name = "Business name")]
    public string BusinessName { get; set; } = string.Empty;

    [Required]
    [Phone]
    [StringLength(30)]
    public string Phone { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(120)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(60)]
    [Display(Name = "Project type")]
    public string ProjectType { get; set; } = string.Empty;

    [Required]
    [StringLength(1000)]
    [Display(Name = "Project details")]
    public string Message { get; set; } = string.Empty;
}
