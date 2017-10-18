using Graphbook.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Graphbook.Web.ViewModels
{
    public class MeViewModel
    {
        public IUserProfile MyCard { get; private set; }
        public IEnumerable<IUserProfile> PeopleWhoInvitedMe { get; private set; }
        public IEnumerable<IUserProfile> FriendSuggestions { get; private set; }
        public IEnumerable<IUserProfile> Friends { get; private set; }

        public MeViewModel(IUserProfile myCard, IEnumerable<IUserProfile> peopleWhoInvitedMe, IEnumerable<IUserProfile> friendSuggestions, IEnumerable<IUserProfile> friends)
        {
            MyCard = myCard;
            PeopleWhoInvitedMe = peopleWhoInvitedMe;
            FriendSuggestions = friendSuggestions;
            Friends = friends;
        }
    }
}