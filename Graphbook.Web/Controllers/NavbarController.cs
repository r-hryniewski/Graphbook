using Graphbook.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Graphbook.Web.Controllers
{
    public class NavbarController : Controller
    {
        public ActionResult Index()
        {
            //TODO: Navbar vm

            return View(new NavbarVM(userFullName: System.Security.Claims.ClaimsPrincipal.Current.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")?.Value));
        }
    }
}