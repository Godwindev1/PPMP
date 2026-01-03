using Microsoft.AspNetCore.Identity;
using System.Collections.ObjectModel;

namespace PPMP.Data
{
    public class User : IdentityUser
    {
        public List<Client> Clients { get; set; }
        public Guid GuestID { get; set; }

        //NAVIGATIONS
        public List<Project>? projects {get; set; }
        public List<Comment>? Comments {get; set; }
        public List<SessionPage>? Sessions {get; set; }

    }
}
