using EventCatalogAPI.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventCatalogAPI.Data
{
    public class EventContext: DbContext
    {
        //Injection only obsorbed through constructor
        public EventContext(DbContextOptions options) : base(options)
        { }

        public DbSet<EventCatagory> EventCatagories { get; set; }
        public DbSet<EventType> EventTypes { get; set; }
        public DbSet<EventItem> EventItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EventCatagory>(ConfigureEventCatagory);
            modelBuilder.Entity<EventType>(ConfigureEventType);
            modelBuilder.Entity<EventItem>(ConfigureEventItem);
        }

        private void ConfigureEventItem(EntityTypeBuilder<EventItem> builder)
        {
            builder.ToTable("Event");
            builder.Property(e => e.Id)
                .IsRequired()
                .ForSqlServerUseSequenceHiLo("event_hilo");
            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(e => e.Price)
                .IsRequired();

            //ForeignKey, event item has one eventType but the eventtype can have mulitple event items , 1:M
            builder.HasOne(e => e.EventType)
                .WithMany()
                .HasForeignKey(e => e.EventTypeId);
            builder.HasOne(e => e.EventCatagory)
                .WithMany()
                .HasForeignKey(e => e.EventCatagoryId);
        }

        private void ConfigureEventType(EntityTypeBuilder<EventType> builder)
        {
            builder.ToTable("EventTypes");
            //primary key should autopopulate using hilo
            builder.Property(t => t.Id)
                .IsRequired()
                .ForSqlServerUseSequenceHiLo("event_type_hilo");

            builder.Property(t => t.TypeName)
                .IsRequired()
                .HasMaxLength(200);
        }

        //Action Delegate which doesn't return anything
        private void ConfigureEventCatagory(EntityTypeBuilder<EventCatagory> builder)
        {
            builder.ToTable("EventCatagories");
            //primary key should autopopulate using hilo
            builder.Property(c => c.Id)
                .IsRequired()
                .ForSqlServerUseSequenceHiLo("event_catagory_hilo");

            builder.Property(c => c.CatagoryName)
                .IsRequired()
                .HasMaxLength(200);
        }
    }
}
