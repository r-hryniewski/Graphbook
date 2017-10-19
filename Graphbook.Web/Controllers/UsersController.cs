using Graphbook.Contracts;
using Graphbook.DAL;
using Graphbook.Web.ViewModels;
using System.Linq;
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
            var vm = new MeViewModel(
                myCard: await repo.GetCardAsync(currentuser.Id),
                peopleWhoInvitedMe: await repo.GetMyPendingInvitesAsync(currentuser.Id),
                //peopleWhoInvitedMe: Enumerable.Empty<IUserProfile>(),
                friends: await repo.GetMyFriendsAsync(currentuser.Id),
                //friends: Enumerable.Empty<IUserProfile>(),
                friendSuggestions: await repo.GetMyFriendsSuggestions(currentuser.Id));
                //friendSuggestions: Enumerable.Empty<IUserProfile>());

            return View(vm);
        }

        public async Task<ActionResult> List()
        {
            var usersCards = await repo.GetAllUsersCardsAsync();

            return View(usersCards);
        }

        [Authorize]
        public async Task<ActionResult> Invite(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                TempData.Add("Message", "Id of person to invite can not be empty");

            else
            {
                var userCard = await repo.GetCardAsync(id);
                if (userCard == null)
                    TempData.Add("Message", $"Cannot find user with id {id}");
                else
                {
                    await repo.InviteFriendAsync(
                        myId: currentuser.Id,
                        invitedId: id);
                    TempData.Add("Message", $"Invited {userCard.Name} {userCard.LastName} to friends");
                }
            }

            return Redirect(HttpContext.Request.UrlReferrer.ToString());
        }

        [Authorize]
        public async Task<ActionResult> AcceptInvitation(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                TempData.Add("Message", "Id of person to invite can not be empty");

            else
            {
                await repo.AcceptFriendInvitationAsync(currentuser.Id, id);
                TempData.Add("Message", "Friend invitation accepted");
            }

            return Redirect(HttpContext.Request.UrlReferrer.ToString());
        }

        [Authorize]
        public async Task<ActionResult> DeclineInvitation(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                TempData.Add("Message", "Id of person to invite can not be empty");

            else
            {
                await repo.DeclineFriendInvitationAsync(currentuser.Id, id);
                TempData.Add("Message", "Friend invitation declined");
            }

            return Redirect(HttpContext.Request.UrlReferrer.ToString());
        }
    }
}