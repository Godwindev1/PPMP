using Microsoft.AspNetCore.Identity;
using System.Collections.ObjectModel;

namespace PPMP.Data
{
    public class Role : IdentityRole
    {
        public ICollection<ClientRole> clientRoles {get; set; }
    }
}