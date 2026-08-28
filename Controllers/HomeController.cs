using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using KM_Digital_Solutions.Models;

namespace KM_Digital_Solutions.Controllers;

public class HomeController : Controller
{
    private static readonly object LeadFileLock = new();

    private readonly ILogger<HomeController> _logger;
    private readonly string _leadStorePath;

    public HomeController(ILogger<HomeController> logger, IWebHostEnvironment environment)
    {
        _logger = logger;
        _leadStorePath = Path.Combine(environment.ContentRootPath, "App_Data", "leads.json");
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

        SaveLead(LeadViewModel.FromContactForm(model));

        TempData["ContactSuccess"] = "Thanks. Your project request was received. KM Digital Solutions will follow up with the next step.";
        return RedirectToAction(nameof(Contact));
    }

    public IActionResult Leads()
    {
        return View(GetSavedLeads());
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

    private IReadOnlyList<LeadViewModel> GetSavedLeads()
    {
        lock (LeadFileLock)
        {
            if (!System.IO.File.Exists(_leadStorePath))
            {
                return [];
            }

            var json = System.IO.File.ReadAllText(_leadStorePath);

            if (string.IsNullOrWhiteSpace(json))
            {
                return [];
            }

            try
            {
                return JsonSerializer.Deserialize<List<LeadViewModel>>(json) ?? [];
            }
            catch (JsonException exception)
            {
                _logger.LogError(exception, "Could not read saved KM Digital leads from {LeadStorePath}", _leadStorePath);
                return [];
            }
        }
    }

    private void SaveLead(LeadViewModel lead)
    {
        lock (LeadFileLock)
        {
            var directory = Path.GetDirectoryName(_leadStorePath);

            if (!string.IsNullOrWhiteSpace(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var leads = GetSavedLeads().ToList();
            leads.Insert(0, lead);

            var json = JsonSerializer.Serialize(leads, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            System.IO.File.WriteAllText(_leadStorePath, json);
        }
    }
}
