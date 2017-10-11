using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Graphbook.Web.ViewModels
{
    public class NavbarVM
    {
        public string UserFullName { get; private set; }
        public bool IsAuthenticated => !string.IsNullOrWhiteSpace(UserFullName);

        public NavbarVM(string userFullName)
        {
            UserFullName = userFullName;
        }

    }
}