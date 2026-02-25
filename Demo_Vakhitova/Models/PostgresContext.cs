using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Demo_Vakhitova.Models;

public partial class PostgresContext : DbContext
{
    public PostgresContext()
    {
    }

    public PostgresContext(DbContextOptions<PostgresContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Listtovar> Listtovars { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Pickuppoint> Pickuppoints { get; set; }

    public virtual DbSet<Postavschik> Postavschiks { get; set; }

    public virtual DbSet<Proizv> Proizvs { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Tovar> Tovars { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost; Port=5432; Username=postgres; Database=postgres; Password=admin ");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("category_pkey");

            entity.ToTable("category");

            entity.Property(e => e.CategoryId)
                .ValueGeneratedNever()
                .HasColumnName("category_id");
            entity.Property(e => e.CategoryName)
                .HasColumnType("character varying")
                .HasColumnName("category_name");
        });

        modelBuilder.Entity<Listtovar>(entity =>
        {
            entity.HasKey(e => e.ListtovarId).HasName("listtovar_pkey");

            entity.ToTable("listtovar");

            entity.Property(e => e.ListtovarId)
                .HasDefaultValueSql("nextval('listtovar_listtovar_seq'::regclass)")
                .HasColumnName("listtovar_id");
            entity.Property(e => e.Art)
                .HasColumnType("character varying")
                .HasColumnName("art");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.Count).HasColumnName("count");
            entity.Property(e => e.Description)
                .HasColumnType("character varying")
                .HasColumnName("description");
            entity.Property(e => e.Discountnow).HasColumnName("discountnow");
            entity.Property(e => e.Kolvo).HasColumnName("kolvo");
            entity.Property(e => e.Photo)
                .HasColumnType("character varying")
                .HasColumnName("photo");
            entity.Property(e => e.PostavschikId).HasColumnName("postavschik_id");
            entity.Property(e => e.ProizvId).HasColumnName("proizv_id");
            entity.Property(e => e.TovarId).HasColumnName("tovar_id");
            entity.Property(e => e.Unity)
                .HasColumnType("character varying")
                .HasColumnName("unity");

            entity.HasOne(d => d.Category).WithMany(p => p.Listtovars)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("listtovar_category_id_fkey");

            entity.HasOne(d => d.Postavschik).WithMany(p => p.Listtovars)
                .HasForeignKey(d => d.PostavschikId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("listtovar_postavschik_id_fkey");

            entity.HasOne(d => d.Proizv).WithMany(p => p.Listtovars)
                .HasForeignKey(d => d.ProizvId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("listtovar_proizv_id_fkey");

            entity.HasOne(d => d.Tovar).WithMany(p => p.Listtovars)
                .HasForeignKey(d => d.TovarId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("listtovar_tovar_id_fkey");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("orders_pkey");

            entity.ToTable("orders");

            entity.Property(e => e.OrderId)
                .ValueGeneratedNever()
                .HasColumnName("order_id");
            entity.Property(e => e.Code).HasColumnName("code");
            entity.Property(e => e.DateDelivery).HasColumnName("date_delivery");
            entity.Property(e => e.DateOrder).HasColumnName("date_order");
            entity.Property(e => e.Listtovar).HasColumnName("listtovar");
            entity.Property(e => e.PickuppointId).HasColumnName("pickuppoint_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.Status)
                .HasColumnType("character varying")
                .HasColumnName("status");

            entity.HasOne(d => d.ListtovarNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.Listtovar)
                .HasConstraintName("orders_listtovar_fkey");

            entity.HasOne(d => d.Pickuppoint).WithMany(p => p.Orders)
                .HasForeignKey(d => d.PickuppointId)
                .HasConstraintName("orders_pickuppoint_id_fkey");

            entity.HasOne(d => d.Role).WithMany(p => p.Orders)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("orders_role_id_fkey");
        });

        modelBuilder.Entity<Pickuppoint>(entity =>
        {
            entity.HasKey(e => e.PickuppointId).HasName("pickuppoint_pkey");

            entity.ToTable("pickuppoint");

            entity.Property(e => e.PickuppointId).HasColumnName("pickuppoint_id");
            entity.Property(e => e.Address)
                .HasColumnType("character varying")
                .HasColumnName("address");
        });

        modelBuilder.Entity<Postavschik>(entity =>
        {
            entity.HasKey(e => e.PostavschikId).HasName("postavschik_pkey");

            entity.ToTable("postavschik");

            entity.Property(e => e.PostavschikId)
                .ValueGeneratedNever()
                .HasColumnName("postavschik_id");
            entity.Property(e => e.PostavschikName)
                .HasColumnType("character varying")
                .HasColumnName("postavschik_name");
        });

        modelBuilder.Entity<Proizv>(entity =>
        {
            entity.HasKey(e => e.ProizvId).HasName("proizv_pkey");

            entity.ToTable("proizv");

            entity.Property(e => e.ProizvId)
                .ValueGeneratedNever()
                .HasColumnName("proizv_id");
            entity.Property(e => e.ProizvName)
                .HasColumnType("character varying")
                .HasColumnName("proizv_name");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("roles_pkey");

            entity.ToTable("roles");

            entity.Property(e => e.RoleId)
                .ValueGeneratedNever()
                .HasColumnName("role_id");
            entity.Property(e => e.RoleName)
                .HasColumnType("character varying")
                .HasColumnName("role_name");
        });

        modelBuilder.Entity<Tovar>(entity =>
        {
            entity.HasKey(e => e.TovarId).HasName("tovar_pkey");

            entity.ToTable("tovar");

            entity.Property(e => e.TovarId)
                .ValueGeneratedNever()
                .HasColumnName("tovar_id");
            entity.Property(e => e.TovarName)
                .HasColumnType("character varying")
                .HasColumnName("tovar_name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("users_pkey");

            entity.ToTable("users");

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Fio)
                .HasColumnType("character varying")
                .HasColumnName("fio");
            entity.Property(e => e.Login)
                .HasColumnType("character varying")
                .HasColumnName("login");
            entity.Property(e => e.Password)
                .HasColumnType("character varying")
                .HasColumnName("password");
            entity.Property(e => e.RoleId).HasColumnName("role_id");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("users_role_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
