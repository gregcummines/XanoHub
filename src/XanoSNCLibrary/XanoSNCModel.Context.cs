﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace XanoSNCLibrary
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class XanoSNCEntities : DbContext
    {
        public XanoSNCEntities()
            : base("name=XanoSNCEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<xNotification> xNotifications { get; set; }
        public virtual DbSet<xNotificationEvent> xNotificationEvents { get; set; }
        public virtual DbSet<xPublisher> xPublishers { get; set; }
        public virtual DbSet<xSubscriber> xSubscribers { get; set; }
        public virtual DbSet<xSubscriptionNotification> xSubscriptionNotifications { get; set; }
        public virtual DbSet<xSubscription> xSubscriptions { get; set; }
    }
}
