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
        private const string Label_User = "user";
        private const string Edge_Invite = "invite";
        private const string Edge_Friend = "friend";

        private readonly GremlinClient gremlinClient;

        public UserRepository(GremlinClient gremlinClient)
        {
            this.gremlinClient = gremlinClient;
        }

        public async Task<bool> UserExistsAsync(string userId)
        {
            var result = (await gremlinClient.Execute(
                gremlinQuery: $"g.V('{userId}')",
                vertexSelector: v => v.Id)).Select(id => id.ToString());

            return result != null && result.Any();
        }

        public async Task<IEnumerable<IUserProfile>> GetMyFriendsAsync(string id)
        {
            return await gremlinClient.Execute(
                //Since 'friend' edge is one way edge - we need to use 'both()' traversal step to traverse both incoming and outcoming edges
                gremlinQuery: $"g.V('{id}').both('{Edge_Friend}').dedup()",
                vertexSelector: v => new Models.UserCard(v));
        }

        public async Task<IEnumerable<IUserProfile>> GetMyFriendsSuggestions(string id)
        {
            return await gremlinClient.Execute(
                gremlinQuery: $"g.V('{id}').both('{Edge_Friend}').both('{Edge_Friend}').dedup()",

                //UH! Above query suggest me as my own friend! Something is seriously wrong here, lets use 'as()' and 'where()' steps
                //gremlinQuery: $"g.V('{id}').as('me').both('{Edge_Friend}').both('{Edge_Friend}').where(neq('me')).dedup()",

                //Let's order results by mutual friends count
                //gremlinQuery: $"g.V('{id}').as('me').both('{Edge_Friend}').both('{Edge_Friend}')." + //Old query fragment
                //$"order().by(both('{Edge_Friend}').both('{Edge_Friend}').where(is('me')).count(), decr)" + //Ordering by mutual friends count
                //$".where(values('id').is(neq('{id}')))" + //Another way to filter people that are not me
                //$".dedup()",

                vertexSelector: v => new Models.UserCard(v));
        }

        public async Task<IEnumerable<IUserProfile>> GetAllUsersCardsAsync()
        {
            return await gremlinClient.Execute(
                gremlinQuery: $"g.V().hasLabel('{Label_User}').order().by('lastName', incr)",
                vertexSelector: v => new Models.UserCard(v));
        }

        public async Task<IEnumerable<IUserProfile>> GetMyPendingInvitesAsync(string myId)
        {
            return await gremlinClient.Execute(
                gremlinQuery: $"g.V('{myId}').in('{Edge_Invite}').hasLabel('{Label_User}')",
                vertexSelector: v => new Models.UserCard(v));
        }
        

        public async Task<long> CountAllUsersAsync()
        {
            var result = await gremlinClient.ExecuteFeedResponse($"g.V().hasLabel('{Label_User}').count()");

            return (long)result.FirstOrDefault();
        }

        public async Task InviteFriendAsync(string myId, string invitedId)
        {
            await gremlinClient.Execute($"g.V('{myId}').addE('{Edge_Invite}').to(g.V('{invitedId}').where(inE('invite').outV().has('id', '{myId}').count().is(0)))");
        }

        public Task<bool> UserExistsAsync(IUser user) => UserExistsAsync(user.Id);

        public Task PersistUserAsync(IUserProfile userCard)
        {
            return gremlinClient.Execute(
                gremlinQuery: $"g.addV('{Label_User}').property('id', '{userCard.Id}').property('name', '{userCard.Name}').property('lastName', '{userCard.LastName}').property('profilePictureUrl', '{userCard.ProfilePictureUrl}')");
        }

        public async Task AcceptFriendInvitationAsync(string myId, string invitingUserId)
        {
            //Remove invitation first
            await DeclineFriendInvitationAsync(myId, invitingUserId);

            await gremlinClient.Execute($"g.V('{myId}').addE('{Edge_Friend}').to(g.V('{invitingUserId}'))");
        }

        public async Task DeclineFriendInvitationAsync(string myId, string invitingUserId)
        {
            await gremlinClient.Execute(
                gremlinQuery: $"g.V('{myId}').inE('{Edge_Invite}').where(outV().has('id', '{invitingUserId}')).drop()");
        }

        public async Task<IUserProfile> GetCardAsync(string userId)
        {
            var results = await gremlinClient.Execute(gremlinQuery: $"g.V('{userId}')",
                vertexSelector: v => new Models.UserCard(v));
            return results.FirstOrDefault();
        }
    }
}
