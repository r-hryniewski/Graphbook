using Graphbook.Contracts;
using Graphbook.DAL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Web.Hosting;

namespace Graphbook.Web.Controllers
{
    /// <summary>
    /// Controller for initializing stuff for demo
    /// </summary>
    public class InitController : Controller
    {
        private readonly UserRepository repo;
        private readonly IUser currentUser;

        public InitController(UserRepository repo, IUser currentUser)
        {
            this.repo = repo;
            this.currentUser = currentUser;
        }

        public async Task<ActionResult> Users()
        {
            var usersCount = await repo.CountAllUsersAsync();
            //Init to 25 (or so) users
            var sampleUsersToAdd = (int)(25 - usersCount);
            if (sampleUsersToAdd > 0)
            {
                using (var fileStream = System.IO.File.OpenRead(HostingEnvironment.MapPath("/Resources/SampleUsers.json")))
                {
                    if (fileStream.Length == 0)
                        throw new ArgumentException($"File at path '/Resources/SampleUsers.json' has length equal {fileStream.Length}");

                    using (var sr = new StreamReader(fileStream, true))
                    {
                        var userCards = JsonConvert.DeserializeObject<IEnumerable<dynamic>>(value: sr.ReadToEnd())
                            .Take(sampleUsersToAdd)
                            .Select(uc => new Models.UserCard(id: uc.Id.ToString(),
                                name: uc.Name.ToString(),
                                lastName: uc.LastName.ToString(),
                                profilePictureUrl: uc.ProfilePictureUrl.ToString()));

                        foreach (var user in userCards)
                        {
                            try
                            {
                                await repo.PersistUserAsync(user);
                            }
                            catch (Exception)
                            {
                                //Exists or stuff - ignore
                            }
                        }
                    }
                }
            }

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