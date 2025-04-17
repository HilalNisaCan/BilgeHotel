using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Entities.Interfaces
{
    public interface IIdentifiablePerson
    {
        string FirstName { get; set; }
        string LastName { get; set; }
        string IdentityNumber { get; set; }
        string PhoneNumber { get; set; }
    }
}
