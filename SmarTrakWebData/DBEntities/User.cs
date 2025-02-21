using System;
using System.Collections.Generic;

namespace SmarTrakWebData.DBEntities;

public partial class User
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public int OrganizationId { get; set; }

    public virtual Organization Organization { get; set; } = null!;
}
