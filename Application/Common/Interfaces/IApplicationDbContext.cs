using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Correspondence> Correspondences { get; set; }
        DbSet<CorrespondenceStatusType> CorrespondenceStatusTypes { get; set; }
        DbSet<CorrespondenceType> CorrespondenceTypes { get; set; }
        DbSet<Credit> Credits { get; set; }
        DbSet<Customer> Customers { get; set; }
        DbSet<CustomerAddress> CustomerAddresses { get; set; }
        DbSet<Discount> Discounts { get; set; }
        DbSet<Item> Items { get; set; }
        DbSet<ItemImage> ItemImages { get; set; }
        DbSet<ItemType> ItemTypes { get; set; }
        DbSet<LineItem> LineItems { get; set; }
        DbSet<Merchant> Merchants { get; set; }
        DbSet<MerchantImage> MerchantImages { get; set; }
        DbSet<MerchantLocation> MerchantLocations { get; set; }
        DbSet<MerchantType> MerchantTypes { get; set; }
        DbSet<MerchantUser> MerchantUsers { get; set; }
        DbSet<MerchantWorkflow> MerchantWorkflows { get; set; }
        DbSet<Order> Orders { get; set; }
        DbSet<OrderStatusType> OrderStatusTypes { get; set; }
        DbSet<Payment> Payments { get; set; }
        DbSet<PaymentMethod> PaymentMethods { get; set; }
        DbSet<PaymentStatusType> PaymentStatusTypes { get; set; }
        DbSet<PaymentType> PaymentTypes { get; set; }
        DbSet<Permission> Permissions { get; set; }
        DbSet<PriceType> PriceTypes { get; set; }
        DbSet<Refund> Refunds { get; set; }
        DbSet<Role> Roles { get; set; }
        DbSet<RolePermission> RolePermissions { get; set; }
        DbSet<ServiceRequest> ServiceRequests { get; set; }
        DbSet<ServiceRequestStatusType> ServiceRequestStatusTypes { get; set; }
        DbSet<State> States { get; set; }
        DbSet<TaxRate> TaxRates { get; set; }
        DbSet<TaxType> TaxTypes { get; set; }
        DbSet<UnitType> UnitTypes { get; set; }
        DbSet<User> Users { get; set; }
        DbSet<UserStatusType> UserStatusTypes { get; set; }
        DbSet<VoidReasonType> VoidReasonTypes { get; set; }
        DbSet<WorkflowStep> WorkflowSteps { get; set; }
        DbSet<Workflow> Workflows { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        int SaveChanges();
        bool EnsureCreated();
    }
}
