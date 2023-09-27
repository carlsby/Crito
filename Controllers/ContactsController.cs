using Crito.Models;
using Crito.Services;
using MailKit;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Website.Controllers;

namespace Crito.Controllers;

public class ContactsController : SurfaceController
{
    public ContactsController(IUmbracoContextAccessor umbracoContextAccessor, IUmbracoDatabaseFactory databaseFactory, ServiceContext services, AppCaches appCaches, IProfilingLogger profilingLogger, IPublishedUrlProvider publishedUrlProvider) : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
    {
    }

    [HttpPost]
    public async Task <IActionResult> Index(ContactForm contactform)
    {
        if (!ModelState.IsValid)
        {
            TempData["ErrorMessage"] = "Form was not submitted.";
        }
        else
        {
            TempData["SuccessMessage"] = "Form submitted successfully!";
            using var mail = new EmailService("carlandersson99@outlook.com", "smtp-mail.outlook.com", 587, "carlandersson99@outlook.com", "");

            await mail.SendAsync(contactform.Email, "Your request was received.", "Hi, your request was received and we will be in contact with you as soon as possible.");
            await mail.SendAsync("carlandersson99@outlook.com", $"{contactform.Name} has sent a contact request.", contactform.Message).ConfigureAwait(false);
        }

        return LocalRedirect(contactform.RedirectUrl ?? "/contact");
    }
}
