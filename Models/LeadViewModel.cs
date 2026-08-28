namespace KM_Digital_Solutions.Models;

public class LeadViewModel
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;

    public string Name { get; set; } = string.Empty;

    public string BusinessName { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string ProjectType { get; set; } = string.Empty;

    public string Message { get; set; } = string.Empty;

    public static LeadViewModel FromContactForm(ContactFormViewModel form)
    {
        return new LeadViewModel
        {
            Id = Guid.NewGuid(),
            SubmittedAt = DateTime.UtcNow,
            Name = form.Name.Trim(),
            BusinessName = form.BusinessName.Trim(),
            Phone = form.Phone.Trim(),
            Email = form.Email.Trim(),
            ProjectType = form.ProjectType.Trim(),
            Message = form.Message.Trim()
        };
    }
}
