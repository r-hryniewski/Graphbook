using Graphbook.Contracts;
using Graphbook.DAL.Graphs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Graphbook.DAL.Constants;

namespace Graphbook.DAL
{
    public class SchoolRepository
    {

        private readonly GremlinClient gremlinClient;

        public SchoolRepository(GremlinClient gremlinClient)
        {
            this.gremlinClient = gremlinClient;
        }

        public async Task<bool> SchoolExistsAsync(string schoolId)
        {
            var result = (await gremlinClient.Execute(
                gremlinQuery: $"g.V('{schoolId}')",
                vertexSelector: v => v.Id)).Select(id => id.ToString());

            return result != null && result.Any();
        }

        public Task<bool> SchoolExistsAsync(ISchool school) => SchoolExistsAsync(school.Id);

        public async Task<IEnumerable<ISchool>> GetAllCompaniesAsync()
        {
            return await gremlinClient.Execute(
                gremlinQuery: $"g.V().hasLabel('{Label_School}').order().by('name', incr)",
                vertexSelector: v => new Models.School(v));
        }
        
        public async Task<long> CountAllUsersAsync()
        {
            var result = await gremlinClient.ExecuteFeedResponse($"g.V().hasLabel('{Label_School}').count()");

            return (long)result.FirstOrDefault();
        }

        public Task PersistSchoolAsync(ISchool school)
        {
            return gremlinClient.Execute(
                gremlinQuery: $"g.addV('{Label_School}').property('id', '{school.Id}').property('name', '{school.Name}')");
        }

        public async Task<ISchool> GetSchoolAsync(string schoolId)
        {
            var results = await gremlinClient.Execute(gremlinQuery: $"g.V('{schoolId}').hasLabel('{Label_School}')",
                vertexSelector: v => new Models.School(v));
            return results.FirstOrDefault();
        }
    }
}
