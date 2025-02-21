using System;
using System.Collections.Generic;

namespace SmarTrakWebData.DBEntities;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public string? Name { get; set; }

    public string? Address { get; set; }

    public string? Gender { get; set; }

    public string? Department { get; set; }
}
