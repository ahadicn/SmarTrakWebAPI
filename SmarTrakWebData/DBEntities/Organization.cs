using System;
using System.Collections.Generic;

namespace SmarTrakWebData.DBEntities;

public partial class Organization
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Address { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
