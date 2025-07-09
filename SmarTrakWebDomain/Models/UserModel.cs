using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmarTrakWebDomain.Models
{
    public class UserModel
    {
        public Guid Id { get; set; }
        public Guid TenantUserId { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }

        public string Name { get; set; }
        public string Email { get; set; }
        public string Sub { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
