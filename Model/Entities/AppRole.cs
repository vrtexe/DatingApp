using Microsoft.AspNetCore.Identity;

namespace Model.Entities;

public class AppRole : IdentityRole<int>
{
    public virtual ICollection<AppUserRole> UserRoles { get; set; }

    public AppRole() : base() { }
    public AppRole(string roleName) : base(roleName) { }
}