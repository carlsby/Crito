using Crito.Models;
using Crito.Services;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Website.Controllers;

namespace Crito.Controllers
{
    public class SubscriberController : SurfaceController
    {
        private readonly SubscriberService subscribe;

        public SubscriberController(IUmbracoContextAccessor umbracoContextAccessor, IUmbracoDatabaseFactory databaseFactory, ServiceContext services, AppCaches appCaches, IProfilingLogger profilingLogger, IPublishedUrlProvider publishedUrlProvider, SubscriberService subscribe) : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
        {
            this.subscribe = subscribe;
        }

        public async Task<IActionResult> Index(NewsletterForm form)
        {
            var subscriber = new NewsletterForm()
            {
                Email = form.Email
            };

            await subscribe.AddSubscriberAsync(subscriber);

            return CurrentUmbracoPage();
        }
    }
}
