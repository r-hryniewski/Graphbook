using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphbook.Contracts
{
    public interface IUser
    {
        string Id { get; }
        string Name { get; }
        string LastName { get; }
    }
}
