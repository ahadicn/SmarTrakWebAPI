using System;
using System.Collections.Generic;

namespace SmarTrakWebData.DBEntities;

public partial class UserDetail
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Gender { get; set; }

    public string? Designation { get; set; }

    public string? Email { get; set; }
}
