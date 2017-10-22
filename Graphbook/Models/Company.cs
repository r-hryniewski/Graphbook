using Graphbook.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphbook.Models
{
    public class Company : ICompany
    {
        public string Id { get; private set; }
        public string Name { get; private set; }

        public Company(string id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
