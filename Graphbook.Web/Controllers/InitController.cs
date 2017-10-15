using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Graphbook.Web.Controllers
{
    /// <summary>
    /// Controller for initializing stuff for demo
    /// </summary>
    public class InitController : Controller
    {
        public async Task<ActionResult> Users()
        {
            return Content("Users initialized");
        }

        public async Task<ActionResult> Schools()
        {
            return Content("Companies initialized");
        }

        public async Task<ActionResult> Companies()
        {
            return Content("Companies initialized");
        }
    }
}