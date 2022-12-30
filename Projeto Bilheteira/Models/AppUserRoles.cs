namespace Utad_Proj_.Models
{
    using System.Collections.Generic;

    public class AppUserRoles
    {
        public string Email { get; set; }
        public IEnumerable<string> Roles { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
    }
}