using Microsoft.AspNetCore.Identity;
using System.Collections.ObjectModel;

namespace PPMP.Data
{
    public class User : IdentityUser
    {
        public List<Client> Clients { get; set; }
        public Guid GuestID { get; set; }
        
    }
}
