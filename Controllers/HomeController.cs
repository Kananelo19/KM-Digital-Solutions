using System.Diagnostics;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Mvc;
using KM_Digital_Solutions.Models;

namespace KM_Digital_Solutions.Controllers;

public class HomeController : Controller
{
    private static readonly ConcurrentQueue<LeadViewModel> ProjectLeads = new();

    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Portfolio()
    {
        return View();
    }

    public IActionResult About()
    {
        return View();
    }

    public IActionResult Contact()
    {
        return View(new ContactFormViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Contact(ContactFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        _logger.LogInformation(
            "New KM Digital lead: {Name} from {BusinessName} interested in {ProjectType}. Phone: {Phone}. Email: {Email}",
            model.Name,
            model.BusinessName,
            model.ProjectType,
            model.Phone,
            model.Email);

        ProjectLeads.Enqueue(LeadViewModel.FromContactForm(model));

        TempData["ContactSuccess"] = "Thanks. Your project request was received. KM Digital Solutions will follow up with the next step.";
        return RedirectToAction(nameof(Contact));
    }

    public IActionResult Leads()
    {
        var leads = ProjectLeads
            .OrderByDescending(lead => lead.SubmittedAt)
            .ToList();

        return View(leads);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
        });
    }
}
