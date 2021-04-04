using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AD.Server.Models;
using AD.Server.Services.Interfaces;
using AD.Shared.DTOs;
using AD.Shared.DTOs.Auth;
using AD.Shared.Entities;
using AD.Shared.Entities.Base;
using AD.Shared.Entities.Base.Enums;
using AD.Shared.Entities.MTM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AD.Server.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        private readonly IUserServices _userServices;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IUserServices userServices)
            : base(options)
        {
            _userServices = userServices;
        }
        public virtual DbSet<UserA_B> UserA_Bs { get; set; }
        public virtual DbSet<UserC> UserCs { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }
        public virtual DbSet<Currency> Currencies { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Store> Stores { get; set; }
        public virtual DbSet<StoreCities> StoreCities { get; set; }
        public virtual DbSet<StoreCountries> StoreCountries { get; set; }
        public virtual DbSet<StoreTags> StoreTags { get; set; }
        public virtual DbSet<StoreUsers> StoreUsers { get; set; }
        public virtual DbSet<Fixer> Fixers { get; set; }
        public virtual DbSet<FixerCurrencies> FixerCurrencies { get; set; }
        public virtual DbSet<Audit> AuditLogs { get; set; }
        public virtual DbSet<WebApiLogs> WebApiLogs { get; set; }
        public virtual DbSet<StoreFollowers> StoreFollowers { get; set; }
        public virtual DbSet<CategoryFollowers> CategoryFollowers { get; set; }
        public virtual DbSet<Picture> Pictures { get; set; }
        public virtual DbSet<MeasruingUnit> MeasruingUnits { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<PaymentMethod> PaymentMethods { get; set; }
        public virtual DbSet<StorePaymentMethods> StorePaymentMethods { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasKey(g => new { g.Id });

            base.OnModelCreating(modelBuilder);
        }
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            string userId = _userServices.GetUserId();

            OnBeforeSaveChanges(userId);

            return await base.SaveChangesAsync();
        }
        private void OnBeforeSaveChanges(string userId)
        {
            ChangeTracker.DetectChanges();
            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedById = userId;
                    entry.Entity.CreatedOnUtc = DateTime.UtcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.LastModifiedById = userId;
                    entry.Entity.LastModifiedOnUtc = DateTime.UtcNow;
                }
            }
            var auditEntries = new List<AuditEntry>();
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is Audit || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                    continue;
                var auditEntry = new AuditEntry(entry);
                auditEntry.TableName = entry.Entity.GetType().Name;
                auditEntry.UserId = userId;
                auditEntries.Add(auditEntry);
                foreach (var property in entry.Properties)
                {
                    string propertyName = property.Metadata.Name;
                    if (property.Metadata.IsPrimaryKey())
                    {
                        auditEntry.KeyValues[propertyName] = property.CurrentValue;
                        continue;
                    }
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            auditEntry.AuditType = AuditType.Create;
                            auditEntry.NewValues[propertyName] = property.CurrentValue;
                            break;
                        case EntityState.Deleted:
                            auditEntry.AuditType = AuditType.Delete;
                            auditEntry.OldValues[propertyName] = property.OriginalValue;
                            break;
                        case EntityState.Modified:
                            if (property.IsModified)
                            {
                                auditEntry.ChangedColumns.Add(propertyName);
                                auditEntry.AuditType = AuditType.Update;
                                auditEntry.OldValues[propertyName] = property.OriginalValue;
                                auditEntry.NewValues[propertyName] = property.CurrentValue;
                            }
                            break;
                    }
                }
            }
            foreach (var auditEntry in auditEntries)
            {
                AuditLogs.Add(auditEntry.ToAudit());
            }
        }
    }
}