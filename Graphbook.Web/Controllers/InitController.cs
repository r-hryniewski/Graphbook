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
        private readonly UserRepository userRepo;
        private readonly CompanyRepository companyRepo;
        private readonly SchoolRepository schoolRepo;
        private readonly IUser currentUser;

        public InitController(UserRepository userRepo, CompanyRepository companyRepo, SchoolRepository schoolRepo, IUser currentUser)
        {
            this.userRepo = userRepo;
            this.companyRepo = companyRepo;
            this.schoolRepo = schoolRepo;
            this.currentUser = currentUser;
        }

        public async Task<ActionResult> Users()
        {
            var usersCount = await userRepo.CountAllUsersAsync();
            //Init to 25 (or so) users
            var sampleUsersToAdd = (int)(25 - usersCount);
            if (sampleUsersToAdd > 0)
            {
                using (var fileStream = System.IO.File.OpenRead(HostingEnvironment.MapPath("/Resources/SampleUsers.json")))
                {
                    if (fileStream.Length == 0)
                        throw new ArgumentException($"File at path '/Resources/SampleUsers.json' has length equal {fileStream.Length}");

                    using (var sr = new StreamReader(fileStream))
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
                                await userRepo.PersistUserAsync(user);
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
            var schoolModels = new Models.School[0];
            using (var fileStream = System.IO.File.OpenRead(HostingEnvironment.MapPath("/Resources/SampleSchools.json")))
            {
                if (fileStream.Length == 0)
                    throw new ArgumentException($"File at path '/Resources/SampleSchools.json' has length equal {fileStream.Length}");

                using (var sr = new StreamReader(fileStream))
                {
                    schoolModels = JsonConvert.DeserializeObject<IEnumerable<dynamic>>(value: sr.ReadToEnd())
                        .Select(uc => new Models.School(id: uc.Id.ToString(),
                            name: uc.Name.ToString())).ToArray();

                    foreach (var school in schoolModels)
                    {
                        try
                        {
                            await schoolRepo.PersistSchoolAsync(school);
                        }
                        catch (Exception)
                        {
                            //Exists or stuff - ignore
                        }
                    }
                }
            }

            //Attach random schools to users
            var random = new Random();
            var allUsersIds = (await userRepo.GetAllUsersCardsAsync()).Select(u => u.Id);

            var tasks = new List<Task>();
            foreach (var userId in allUsersIds)
            {
                var n = random.Next(-1, schoolModels.Length);
                if (n >= 0)
                    tasks.Add(userRepo.AddSchoolThatUserAttendedToAsync(userId, schoolModels[n].Id));
            }


            return Content("Schools initialized");
        }

        public async Task<ActionResult> Companies()
        {
            var companyModels = new Models.Company[0];
            using (var fileStream = System.IO.File.OpenRead(HostingEnvironment.MapPath("/Resources/SampleCompanies.json")))
            {
                if (fileStream.Length == 0)
                    throw new ArgumentException($"File at path '/Resources/SampleCompanies.json' has length equal {fileStream.Length}");

                using (var sr = new StreamReader(fileStream))
                {
                    companyModels = JsonConvert.DeserializeObject<IEnumerable<dynamic>>(value: sr.ReadToEnd())
                        .Select(uc => new Models.Company(id: uc.Id.ToString(),
                            name: uc.Name.ToString())).ToArray();

                    foreach (var company in companyModels)
                    {
                        try
                        {
                            await companyRepo.PersistCompanyAsync(company);
                        }
                        catch (Exception)
                        {
                            //Exists or stuff - ignore
                        }
                    }
                }
            }
            //Attach random schools to users
            var random = new Random();
            var allUsersIds = (await userRepo.GetAllUsersCardsAsync()).Select(u => u.Id);

            var tasks = new List<Task>();
            foreach (var userId in allUsersIds)
            {
                var n = random.Next(-1, companyModels.Length);
                if (n >= 0)
                    tasks.Add(userRepo.AddCompanyThatUserWorksAtAsync(userId, companyModels[n].Id));
            }

            return Content("Companies initialized");
        }
    }
}