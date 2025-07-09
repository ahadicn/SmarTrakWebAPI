using System;
using System.Collections.Generic;

namespace SmarTrakWebAPI.DBEntities;

public partial class User
{
    public Guid Id { get; set; }

    public Guid TenantUserId { get; set; }

    public int? RoleId { get; set; }

    public string? Name { get; set; }

    public string Email { get; set; } = null!;

    public string? Sub { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Role? Role { get; set; }
}
