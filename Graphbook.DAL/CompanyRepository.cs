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
    public class CompanyRepository
    {

        private readonly GremlinClient gremlinClient;

        public CompanyRepository(GremlinClient gremlinClient)
        {
            this.gremlinClient = gremlinClient;
        }

        public async Task<bool> CompanyExistsAsync(string companyId)
        {
            var result = (await gremlinClient.Execute(
                gremlinQuery: $"g.V('{companyId}')",
                vertexSelector: v => v.Id)).Select(id => id.ToString());

            return result != null && result.Any();
        }

        public Task<bool> CompanyExistsAsync(ICompany company) => CompanyExistsAsync(company.Id);

        public async Task<IEnumerable<ICompany>> GetAllCompaniesAsync()
        {
            return await gremlinClient.Execute(
                gremlinQuery: $"g.V().hasLabel('{Label_Company}').order().by('name', incr)",
                vertexSelector: v => new Models.Company(v));
        }
        
        public async Task<long> CountAllUsersAsync()
        {
            var result = await gremlinClient.ExecuteFeedResponse($"g.V().hasLabel('{Label_Company}').count()");

            return (long)result.FirstOrDefault();
        }

        public Task PersistCompanyAsync(ICompany company)
        {
            return gremlinClient.Execute(
                gremlinQuery: $"g.addV('{Label_Company}').property('id', '{company.Id}').property('name', '{company.Name}')");
        }

        public async Task<ICompany> GetCompanyAsync(string companyId)
        {
            var results = await gremlinClient.Execute(gremlinQuery: $"g.V('{companyId}').hasLabel('{Label_Company}')",
                vertexSelector: v => new Models.Company(v));
            return results.FirstOrDefault();
        }
    }
}
