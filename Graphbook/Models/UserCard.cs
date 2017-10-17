using Graphbook.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphbook.Models
{
    public class UserCard : IUserProfile
    {
        public string Id { get; private set; }
        public string Name { get; private set; }
        public string LastName { get; private set; }
        public string ProfilePictureUrl { get; private set; }

        public UserCard(string id, string name, string lastName, string profilePictureUrl)
        {
            Id = id;
            Name = name;
            LastName = lastName;
            ProfilePictureUrl = profilePictureUrl;
        }

    }
}
