using Graphbook.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Graphs.Elements;

namespace Graphbook.DAL.Models
{
    public class UserCard : IUserCard
    {
        public string Id { get; private set; }
        public string Name { get; private set; }
        public string LastName { get; private set; }
        public string ProfilePictureUrl { get; private set; }

        public UserCard(Vertex vertex)
        {
            Id = vertex.Id.ToString();
            var properties = vertex.GetVertexProperties().ToDictionary(vp => vp.Key, vp => vp.Value);

            Name = properties.ContainsKey("name") ? properties["name"].ToString() : string.Empty;
            LastName = properties.ContainsKey("lastName") ? properties["lastName"].ToString() : string.Empty;
            ProfilePictureUrl = properties.ContainsKey("profilePictureUrl") ? properties["profilePictureUrl"].ToString() : string.Empty;
        }
    }
}
