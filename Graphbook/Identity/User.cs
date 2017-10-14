using Graphbook.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;

namespace Graphbook.Identity
{
    public class User : IUserIdentity
    {
        public string Id { get; private set; }
        public string Name { get; private set; }
        public string LastName { get; private set; }
        public bool IsAuthenticated { get; private set; }

        public User(ClaimsPrincipal claimsPrincipal)
        {
            IsAuthenticated = claimsPrincipal.Identity.IsAuthenticated;

            if (!IsAuthenticated)
                return;

            Id = claimsPrincipal.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
            Name = claimsPrincipal.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname")?.Value;
            LastName = claimsPrincipal.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname")?.Value;
        }
    }
}
