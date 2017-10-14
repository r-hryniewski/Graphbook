using Graphbook.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Graphbook.Web.Controllers
{
    public class NavbarController : Controller
    {
        private readonly IUser currentUser;

        public NavbarController(IUser currentUser)
        {
            this.currentUser = currentUser;
        }

        public ActionResult Index()
        {
            return View(currentUser);
        }
    }
}