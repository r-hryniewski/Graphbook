using Graphbook.Contracts;
using Graphbook.DAL.Graphs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Graphbook.DAL
{
    public class UserRepository
    {
        private const string UserLabel = "user";
        private readonly GremlinClient gremlinClient;

        public UserRepository(GremlinClient gremlinClient)
        {
            this.gremlinClient = gremlinClient;
        }

        public async Task<bool> UserExistsAsync(string userId)
        {
            var result = (await gremlinClient.Execute(
                gremlinQuery: $"g.V('{userId}')",
                selector: v => v.Id)).Select(id => id.ToString());

            return result != null && result.Any();
        }

        public Task<bool> UserExistsAsync(IUser user) => UserExistsAsync(user.Id);

        public Task PersistUserAsync(IUserCard userCard)
        {
            return gremlinClient.Execute($"g.addV('{UserLabel}').property('id', '{userCard.Id}').property('name', '{userCard.Name}').property('lastName', '{userCard.LastName}').property('profilePictureUrl', '{userCard.ProfilePictureUrl}')");
        }
    }
}
