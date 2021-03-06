﻿using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Data
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Credit> Credits { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerAddress> CustomerAddresses { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<Correspondence> Correspondences { get; set; }
        public DbSet<CorrespondenceType> CorrespondenceTypes { get; set; }
        public DbSet<CorrespondenceStatusType> CorrespondenceStatusTypes { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<ItemImage> ItemImages { get; set; }
        public DbSet<ItemType> ItemTypes { get; set; }
        public DbSet<LineItem> LineItems { get; set; }
        public DbSet<Merchant> Merchants { get; set; }
        public DbSet<MerchantImage> MerchantImages { get; set; }
        public DbSet<MerchantLocation> MerchantLocations { get; set; }
        public DbSet<MerchantType> MerchantTypes { get; set; }
        public DbSet<MerchantUser> MerchantUsers { get; set; }
        public DbSet<MerchantWorkflow> MerchantWorkflows { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderStatusType> OrderStatusTypes { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<PaymentStatusType> PaymentStatusTypes { get; set; }
        public DbSet<PaymentType> PaymentTypes { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<PriceType> PriceTypes { get; set; }
        public DbSet<Refund> Refunds { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<ServiceRequest> ServiceRequests { get; set; }
        public DbSet<ServiceRequestStatusType> ServiceRequestStatusTypes { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<TaxRate> TaxRates { get; set; }
        public DbSet<TaxType> TaxTypes { get; set; }
        public DbSet<UnitType> UnitTypes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserStatusType> UserStatusTypes { get; set; }
        public DbSet<VoidReasonType> VoidReasonTypes { get; set; }
        public DbSet<WorkflowStep> WorkflowSteps { get; set; }
        public DbSet<Workflow> Workflows { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Correspondence>();
            modelBuilder.Entity<CorrespondenceType>();
            modelBuilder.Entity<CorrespondenceStatusType>();
            modelBuilder.Entity<Credit>();
            modelBuilder.Entity<Customer>();
            modelBuilder.Entity<CustomerAddress>();
            modelBuilder.Entity<Discount>();
            modelBuilder.Entity<Item>();
            modelBuilder.Entity<ItemImage>();
            modelBuilder.Entity<ItemType>();
            modelBuilder.Entity<LineItem>();
            modelBuilder.Entity<Merchant>();
            modelBuilder.Entity<MerchantLocation>();
            modelBuilder.Entity<MerchantType>();
            modelBuilder.Entity<MerchantUser>();
            modelBuilder.Entity<MerchantWorkflow>();
            modelBuilder.Entity<Order>();
            modelBuilder.Entity<OrderStatusType>();
            modelBuilder.Entity<Payment>();
            modelBuilder.Entity<PaymentMethod>();
            modelBuilder.Entity<PaymentStatusType>();
            modelBuilder.Entity<PaymentType>();
            modelBuilder.Entity<Permission>();
            modelBuilder.Entity<PriceType>();
            modelBuilder.Entity<Refund>();
            modelBuilder.Entity<Role>();
            modelBuilder.Entity<RolePermission>();
            modelBuilder.Entity<ServiceRequest>();
            modelBuilder.Entity<ServiceRequestStatusType>();
            modelBuilder.Entity<State>();
            modelBuilder.Entity<TaxRate>();
            modelBuilder.Entity<TaxType>();
            modelBuilder.Entity<UnitType>();
            modelBuilder.Entity<User>();
            modelBuilder.Entity<UserStatusType>();
            modelBuilder.Entity<VoidReasonType>();
            modelBuilder.Entity<WorkflowStep>();
            modelBuilder.Entity<Workflow>();

            modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys())
                .ToList()
                .ForEach(r => r.DeleteBehavior = DeleteBehavior.Restrict);
        }
        public bool EnsureCreated()
        {
            return Database.EnsureCreated();
        }
    }
}
