using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Graphbook.Web.Controllers
{
    public class UsersController : Controller
    {
        public ActionResult Me()
        {
            return Content("'Me' NYI");
        }

        public ActionResult List()
        {
            return Content("'List' NYI");
        }
    }
}