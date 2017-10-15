using Graphbook.Contracts;
using Graphbook.DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Graphbook.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly static HashSet<string> naiveUserIdsCache = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        private readonly UserRepository repo;
        private readonly IUser currentUser;

        public HomeController(UserRepository repo, IUser currentUser)
        {
            this.repo = repo;
            this.currentUser = currentUser;
        }

        public async Task<ActionResult> Index()
        {
            //TODO: Persist user data if it's not in database already 
            if (Request.IsAuthenticated && !naiveUserIdsCache.Contains(currentUser.Id) && !(await repo.UserExistsAsync(currentUser)))
            {
                var profilePictureUrl = "https://via.placeholder.com/200x200";

                //Getting profile photo from fb
                try
                {
                    var fbAccessToken = Request.Headers["X-MS-TOKEN-FACEBOOK-ACCESS-TOKEN"];

                    var client = new Facebook.FacebookClient(fbAccessToken);

                    var me = await client.GetTaskAsync<dynamic>("me");
                    var response = await client.GetTaskAsync<dynamic>("me?fields=picture.width(200).height(200)");
                    profilePictureUrl = response.picture.data.url;
                }
                catch (Exception)
                {
                    //Something fucked up - do nothing
                }

                var userCard = new Models.UserCard(id: currentUser.Id,
                    name: currentUser.Name,
                    lastName: currentUser.LastName,
                    profilePictureUrl: profilePictureUrl);

                //Persist in db
                try
                {
                    await repo.PersistUserAsync(userCard);
                    naiveUserIdsCache.Add(currentUser.Id);
                }
                catch (Exception)
                {
                    //Something fucked up - do nothing
                }

            }

            return View();
            //return RedirectToAction("Me", "Users");
        }
    }
}