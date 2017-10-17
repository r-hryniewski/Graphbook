using Graphbook.Contracts;
using Graphbook.DAL;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Graphbook.Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUser currentuser;
        private readonly UserRepository repo;

        public UsersController(IUser currentuser, UserRepository repo)
        {
            this.currentuser = currentuser;
            this.repo = repo;
        }

        [Authorize]
        public async Task<ActionResult> Me()
        {
            var myCard = await repo.GetCardAsync(currentuser.Id);            
            return View(myCard);
        }

        public async Task<ActionResult> List()
        {
            var usersCards = await repo.GetAllUsersCardsAsync();

            return View(usersCards);
        }
    }
}