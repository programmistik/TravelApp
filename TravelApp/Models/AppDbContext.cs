﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelApp.Models
{
    class AppDbContext : DbContext
    {
        public AppDbContext() : base("DefaultConnection")
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Trip> Trips { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<CityList> CityLists { get; set; }
        public DbSet<CheckItem> CheckLists { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CityList>()
               .HasOptional(j => j.Trip)
               .WithMany(x => x.CityList)
               .WillCascadeOnDelete(true);

            modelBuilder.Entity<Ticket>()
               .HasOptional(j => j.Trip)
               .WithMany(x => x.Tickets)
               .WillCascadeOnDelete(true);

            modelBuilder.Entity<CheckItem>()
               .HasOptional(j => j.Trip)
               .WithMany(x => x.CheckItems)
               .WillCascadeOnDelete(true);

            base.OnModelCreating(modelBuilder);
        }
    }
}