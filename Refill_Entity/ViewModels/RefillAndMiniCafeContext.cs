using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Refill_Entity.Models;

namespace Refill_Entity.ViewModels
{
    public partial class RefillAndMiniCafeContext : DbContext
    {
        public RefillAndMiniCafeContext()
        {
        }

        public RefillAndMiniCafeContext(DbContextOptions<RefillAndMiniCafeContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Product> Products { get; set; }

        public virtual DbSet<Refill> Refills { get; set; }

        public virtual DbSet<Sale> Sales { get; set; }

        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)

            => optionsBuilder.UseSqlServer("Data Source=DESKTOP-0BAGMG4\\SQLEXPRESS;Initial Catalog=RefillAndMiniCafe;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Product__3214EC070B179E5F");

                entity.ToTable("Product");

                entity.Property(e => e.Category).HasMaxLength(50);
                entity.Property(e=> e.ProductCount).HasColumnType("double");
                entity.Property(e => e.Price).HasColumnType("money");
                entity.Property(e => e.Title).HasMaxLength(50);
            });

            modelBuilder.Entity<Refill>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Refills__3214EC078C0FE61F");
                entity.Property(e => e.ProductCount).HasColumnType("double");
                entity.Property(e => e.Price).HasColumnType("money");
                entity.Property(e => e.Title).HasMaxLength(10);
            });

            modelBuilder.Entity<Sale>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Sales__3214EC0773F13FDD");

                entity.Property(e => e.Amount).HasColumnType("money");
                entity.Property(e => e.Date).HasColumnType("date");
                entity.Property(e => e.NameUsers).HasMaxLength(50);
                entity.Property(e => e.ProductName).HasMaxLength(50);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Users__3214EC07146AA85C");

                entity.HasIndex(e => e.Passwd, "UQ__Users__C6778312C56610E0").IsUnique();

                entity.Property(e => e.Name).HasMaxLength(50);
                entity.Property(e => e.Passwd)
                    .HasMaxLength(25)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}




