﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphbook.Contracts
{
    public interface IUserCard : IUser
    {
        string ProfilePictureUrl { get; }
    }
}
