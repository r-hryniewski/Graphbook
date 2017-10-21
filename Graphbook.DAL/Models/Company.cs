using Graphbook.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Graphs.Elements;

namespace Graphbook.DAL.Models
{
    public class Company : ICompany
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public Company(Vertex vertex)
        {
            Id = vertex.Id.ToString();
            var properties = vertex.GetVertexProperties().ToDictionary(vp => vp.Key, vp => vp.Value);

            Name = properties.ContainsKey("name") ? properties["name"].ToString() : string.Empty;
        }
    }
}
