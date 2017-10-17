﻿using Graphbook.Contracts;
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
                vertexSelector: v => v.Id)).Select(id => id.ToString());

            return result != null && result.Any();
        }

        public async Task<IEnumerable<IUserProfile>> GetAllUsersCardsAsync()
        {
            return await gremlinClient.Execute(
                gremlinQuery: $"g.V().hasLabel('{UserLabel}').order().by('lastName', incr)",
                vertexSelector: v => new Models.UserCard(v));
        }

        public async Task<long> CountAllUsersAsync()
        {
            var result = await gremlinClient.ExecuteFeedResponse($"g.V().hasLabel('{UserLabel}').count()");

            return (long)result.FirstOrDefault();
        }

        public Task<bool> UserExistsAsync(IUser user) => UserExistsAsync(user.Id);

        public Task PersistUserAsync(IUserProfile userCard)
        {
            return gremlinClient.Execute($"g.addV('{UserLabel}').property('id', '{userCard.Id}').property('name', '{userCard.Name}').property('lastName', '{userCard.LastName}').property('profilePictureUrl', '{userCard.ProfilePictureUrl}')");
        }

        public async Task<IUserProfile> GetCardAsync(string userId)
        {
            var results = await gremlinClient.Execute(gremlinQuery: $"g.V('{userId}')",
                vertexSelector: v => new Models.UserCard(v));
            return results.FirstOrDefault();
        }
    }
}
