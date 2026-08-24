namespace KM_Digital_Solutions.Models;

public class LeadViewModel
{
    public DateTime SubmittedAt { get; set; } = DateTime.Now;

    public string Name { get; set; } = string.Empty;

    public string BusinessName { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string ProjectType { get; set; } = string.Empty;

    public string Message { get; set; } = string.Empty;

    public static LeadViewModel FromContactForm(ContactFormViewModel model)
    {
        return new LeadViewModel
        {
            SubmittedAt = DateTime.Now,
            Name = model.Name,
            BusinessName = model.BusinessName,
            Phone = model.Phone,
            Email = model.Email,
            ProjectType = model.ProjectType,
            Message = model.Message
        };
    }
}
