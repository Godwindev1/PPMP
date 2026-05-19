using Microsoft.AspNetCore.Identity;

namespace PPMP.API.Data
{
    public class ClientRole
    {
        public Guid ClientID;
        public string RoleID;

        public Client client {get; set;}
        public Role Role { get; set; }

    }
}