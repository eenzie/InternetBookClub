﻿using InternetBookClub.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Website.Controllers;

namespace InternetBookClub.Controllers
{
    public class ContactFormSurfaceController : SurfaceController
    {
        public ContactFormSurfaceController(IUmbracoContextAccessor umbracoContextAccessor,
                                            IUmbracoDatabaseFactory databaseFactory,
                                            ServiceContext services,
                                            AppCaches appCaches,
                                            IProfilingLogger profilingLogger,
                                            IPublishedUrlProvider publishedUrlProvider)
                                            : base(umbracoContextAccessor,
                                                  databaseFactory,
                                                  services,
                                                  appCaches,
                                                  profilingLogger,
                                                  publishedUrlProvider)
        {
        }

        // GET Contact Form instance
        public IActionResult Index()
        {
            return PartialView("ContactForm", new ContactForm());
        }

        [HttpPost]
        public IActionResult Submit(ContactForm model)
        {
            if (!ModelState.IsValid)
            {
                // returns the form with the data entries intact
                return CurrentUmbracoPage();
            }
            else
            {
                // Adding feedback to the users
                TempData["success"] = true;

                // Creates the comment content
                var cs = Services.ContentService;

                if (cs != null)
                {
                    IContent comment = cs.Create(model.Subject, CurrentPage.Id, "Comment");

                    comment.SetValue("username", model.Name);
                    comment.SetValue("email", model.Email);
                    comment.SetValue("subject", model.Subject);
                    comment.SetValue("message", model.Message);

                    cs.Save(comment);
                }

                // Returns an empty form as entries are no longer needed
                return RedirectToCurrentUmbracoPage();
            }
        }
    }
}
