using System;
using System.Collections.Generic;

namespace SmarTrakWebAPI.DBEntities;

public partial class Subscription
{
    public Guid Id { get; set; }

    public Guid SubscriptionId { get; set; }

    public Guid CustomerId { get; set; }

    public Guid? CustomerRefId { get; set; }

    public string OfferName { get; set; } = null!;

    public string Status { get; set; } = null!;

    public int Quantity { get; set; }

    public string? UnitType { get; set; }

    public string BillingCycle { get; set; } = null!;

    public string BillingType { get; set; } = null!;

    public DateTime? CreatedDate { get; set; }

    public DateTime? StartedDate { get; set; }

    public string? FriendlyName { get; set; }

    public bool? AutoRenewEnabled { get; set; }

    public string? TermDuration { get; set; }

    public DateTime? CommitmentEndDate { get; set; }

    public DateTime? EffectiveStartDate { get; set; }

    public bool? IsTrial { get; set; }

    public string? ProductId { get; set; }

    public string? SkuId { get; set; }

    public Guid? EntitlementId { get; set; }

    public int? QuantityProvisioned { get; set; }

    public string? OfferId { get; set; }

    public string? ProductTypeId { get; set; }

    public bool? HasPurchasableAddons { get; set; }

    public DateTimeOffset? BillingCycleEndDateTime { get; set; }

    public DateTimeOffset? CancellationAllowedUntilDate { get; set; }

    public string? TermLifeCycleState { get; set; }

    public string? RenewalTermDuration { get; set; }

    public string? ContractType { get; set; }

    public string? PublisherName { get; set; }

    public bool? IsMicrosoftProduct { get; set; }

    public bool? AttentionNeeded { get; set; }

    public bool? ActionTaken { get; set; }

    public string? OrderId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public string? UpdatedBy { get; set; }

    public virtual Customer Customer { get; set; } = null!;
}
