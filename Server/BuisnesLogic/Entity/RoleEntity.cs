using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entity
{
    public class RoleEntity : IdentityRole<long>
    {
        public virtual ICollection<UserRoleEntity> UserRoles { get; set; }
    }
}
