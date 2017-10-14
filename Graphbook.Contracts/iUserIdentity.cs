using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphbook.Contracts
{
    public interface IUserIdentity : IUser
    {
        bool IsAuthenticated { get; }
    }
}
