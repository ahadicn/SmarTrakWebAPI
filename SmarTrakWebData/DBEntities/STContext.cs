using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SmarTrakWebAPI.DBEntities;

public partial class STContext : DbContext
{
    public STContext()
    {
    }

    public STContext(DbContextOptions<STContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Subscription> Subscriptions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=tcp:smatrakdemo.database.windows.net;Database=SmarTrakDemo;User ID=Smartrak;Password=Vofox@1234;TrustServerCertificate=True;");
        //=> optionsBuilder.UseSqlServer("Server=.\\sqlexpress;Database=AzureSmartrak;user id=sa;password=vfx@123;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Customer__3214EC078DD2A886");

            entity.ToTable("Customer");

            entity.HasIndex(e => e.CustomerId, "UQ__Customer__A4AE64D9992BB43D").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.City).HasMaxLength(100);
            entity.Property(e => e.Country).HasMaxLength(100);
            entity.Property(e => e.CreatedBy).HasMaxLength(255);
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Culture).HasMaxLength(20);
            entity.Property(e => e.Domain).HasMaxLength(255);
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.Language).HasMaxLength(20);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.PostalCode).HasMaxLength(20);
            entity.Property(e => e.RelationshipToPartner).HasMaxLength(50);
            entity.Property(e => e.State).HasMaxLength(100);
            entity.Property(e => e.UpdatedBy).HasMaxLength(255);
            entity.Property(e => e.UpdatedDate).HasDefaultValueSql("(sysutcdatetime())");
        });

        modelBuilder.Entity<Subscription>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Subscrip__3214EC0745903989");

            entity.ToTable("Subscription");

            entity.HasIndex(e => e.SubscriptionId, "UQ__Subscrip__9A2B249C536EEC89").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.BillingCycle).HasMaxLength(50);
            entity.Property(e => e.BillingType).HasMaxLength(50);
            entity.Property(e => e.ContractType).HasMaxLength(50);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.CreatedBy).HasMaxLength(255);
            entity.Property(e => e.FriendlyName).HasMaxLength(255);
            entity.Property(e => e.OfferId)
                .HasMaxLength(100)
                .HasDefaultValue("");
            entity.Property(e => e.OfferName).HasMaxLength(255);
            entity.Property(e => e.OrderId).HasMaxLength(100);
            entity.Property(e => e.ProductId).HasMaxLength(100);
            entity.Property(e => e.ProductTypeId).HasMaxLength(100);
            entity.Property(e => e.PublisherName).HasMaxLength(200);
            entity.Property(e => e.RenewalTermDuration).HasMaxLength(20);
            entity.Property(e => e.SkuId).HasMaxLength(100);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.TermDuration).HasMaxLength(50);
            entity.Property(e => e.TermLifeCycleState).HasMaxLength(50);
            entity.Property(e => e.UnitType).HasMaxLength(50);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.UpdatedBy).HasMaxLength(255);

            entity.HasOne(d => d.Customer).WithMany(p => p.Subscriptions)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Subscription_Customer_New");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
