using System;
using System.Collections.Generic;

namespace SmarTrakWebAPI.DBEntities;

public partial class Customer
{
    public Guid Id { get; set; }

    public Guid CustomerId { get; set; }

    public string Name { get; set; } = null!;

    public string? Domain { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public string? CreatedBy { get; set; }

    public string? UpdatedBy { get; set; }

    public Guid? TenantId { get; set; }

    public string? RelationshipToPartner { get; set; }

    public bool? AllowDelegatedAccess { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Email { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }

    public string? PostalCode { get; set; }

    public string? Country { get; set; }

    public string? Culture { get; set; }

    public string? Language { get; set; }

    public virtual ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
}
