using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace DAL.Models
{
    public partial class LoichDBContext : DbContext
    {
        public LoichDBContext()
        {
        }

        public LoichDBContext(DbContextOptions<LoichDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Apartment> Apartments { get; set; }
        public virtual DbSet<Collection> Collections { get; set; }
        public virtual DbSet<CollectionMapping> CollectionMappings { get; set; }
        public virtual DbSet<Menu> Menus { get; set; }
        public virtual DbSet<MerchantStore> MerchantStores { get; set; }
        public virtual DbSet<News> News { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<PaymentMethod> PaymentMethods { get; set; }
        public virtual DbSet<Poi> Pois { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductCategory> ProductCategories { get; set; }
        public virtual DbSet<ProductCombination> ProductCombinations { get; set; }
        public virtual DbSet<ProductInMenu> ProductInMenus { get; set; }
        public virtual DbSet<Resident> Residents { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<StoreMenuDetail> StoreMenuDetails { get; set; }
        public virtual DbSet<SystemCategory> SystemCategories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=localhost;Database=LoichDB;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("Account");

                entity.Property(e => e.AccountId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("AccountID");

                entity.Property(e => e.AvatarImage).IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ProfileImage).IsUnicode(false);

                entity.Property(e => e.RoleId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("RoleID");

                entity.Property(e => e.Token).IsUnicode(false);

                entity.Property(e => e.Username)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Accounts)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_Account_Role");
            });

            modelBuilder.Entity<Apartment>(entity =>
            {
                entity.ToTable("Apartment");

                entity.Property(e => e.ApartmentId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ApartmentID");

                entity.Property(e => e.Address).HasMaxLength(250);
            });

            modelBuilder.Entity<Collection>(entity =>
            {
                entity.ToTable("Collection");

                entity.Property(e => e.CollectionId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("CollectionID");

                entity.Property(e => e.ResidentId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ResidentID");

                entity.HasOne(d => d.Resident)
                    .WithMany(p => p.Collections)
                    .HasForeignKey(d => d.ResidentId)
                    .HasConstraintName("FK_Collection_Resident");
            });

            modelBuilder.Entity<CollectionMapping>(entity =>
            {
                entity.HasKey(e => new { e.CollectionId, e.ProductId })
                    .HasName("PK_tblCollectionMapping");

                entity.ToTable("CollectionMapping");

                entity.Property(e => e.CollectionId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("CollectionID");

                entity.Property(e => e.ProductId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ProductID");

                entity.HasOne(d => d.Collection)
                    .WithMany(p => p.CollectionMappings)
                    .HasForeignKey(d => d.CollectionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblCollectionMapping_tblCollection");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.CollectionMappings)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblCollectionMapping_tblProduct");
            });

            modelBuilder.Entity<Menu>(entity =>
            {
                entity.ToTable("Menu");

                entity.Property(e => e.MenuId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("MenuID");

                entity.Property(e => e.MenuName).HasMaxLength(250);

                entity.Property(e => e.ResidentId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ResidentID");

                entity.HasOne(d => d.Resident)
                    .WithMany(p => p.Menus)
                    .HasForeignKey(d => d.ResidentId)
                    .HasConstraintName("FK_Menu_Resident");
            });

            modelBuilder.Entity<MerchantStore>(entity =>
            {
                entity.ToTable("MerchantStore");

                entity.Property(e => e.MerchantStoreId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("MerchantStoreID");

                entity.Property(e => e.ApartmentId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ApartmentID");

                entity.Property(e => e.ResidentId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ResidentID");

                entity.Property(e => e.StoreName).HasMaxLength(250);

                entity.HasOne(d => d.Apartment)
                    .WithMany(p => p.MerchantStores)
                    .HasForeignKey(d => d.ApartmentId)
                    .HasConstraintName("FK_MerchantStore_Aparments");

                entity.HasOne(d => d.Resident)
                    .WithMany(p => p.MerchantStores)
                    .HasForeignKey(d => d.ResidentId)
                    .HasConstraintName("FK_MerchantStore_Resident");
            });

            modelBuilder.Entity<News>(entity =>
            {
                entity.Property(e => e.NewsId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("NewsID");

                entity.Property(e => e.ApartmentId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ApartmentID");

                entity.Property(e => e.ResidentId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ResidentID");

                entity.Property(e => e.Title).HasMaxLength(250);

                entity.HasOne(d => d.Apartment)
                    .WithMany(p => p.News)
                    .HasForeignKey(d => d.ApartmentId)
                    .HasConstraintName("FK_News_Apartment");

                entity.HasOne(d => d.Resident)
                    .WithMany(p => p.News)
                    .HasForeignKey(d => d.ResidentId)
                    .HasConstraintName("FK_News_Resident");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Order");

                entity.Property(e => e.OrderId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("OrderID");

                entity.Property(e => e.MerchantStoreId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("MerchantStoreID");

                entity.Property(e => e.ResidentId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ResidentID");

                entity.HasOne(d => d.MerchantStore)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.MerchantStoreId)
                    .HasConstraintName("FK_tblOrder_tblMerchantStore");

                entity.HasOne(d => d.Resident)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.ResidentId)
                    .HasConstraintName("FK_Order_Resident");
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.ToTable("OrderDetail");

                entity.Property(e => e.OrderDetailId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("OrderDetailID");

                entity.Property(e => e.OrderId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("OrderID");

                entity.Property(e => e.ProductInMenuId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ProductInMenuID");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK_tblOrderDetail_tblOrder");

                entity.HasOne(d => d.ProductInMenu)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.ProductInMenuId)
                    .HasConstraintName("FK_OrderDetail_ProductInMenu");
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.ToTable("Payment");

                entity.Property(e => e.PaymentId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("PaymentID");

                entity.Property(e => e.OrderId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("OrderID");

                entity.Property(e => e.PaymentMethodId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("PaymentMethodID");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK_tblPayment_tblOrder");

                entity.HasOne(d => d.PaymentMethod)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.PaymentMethodId)
                    .HasConstraintName("FK_tblPayment_tblPaymentMethod");
            });

            modelBuilder.Entity<PaymentMethod>(entity =>
            {
                entity.ToTable("PaymentMethod");

                entity.Property(e => e.PaymentMethodId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("PaymentMethodID");

                entity.Property(e => e.PaymentName).HasMaxLength(250);
            });

            modelBuilder.Entity<Poi>(entity =>
            {
                entity.ToTable("POI");

                entity.Property(e => e.PoiId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("PoiID");

                entity.Property(e => e.ApartmentId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ApartmentID");

                entity.Property(e => e.ResidentId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ResidentID");

                entity.Property(e => e.Title).HasMaxLength(250);

                entity.HasOne(d => d.Apartment)
                    .WithMany(p => p.Pois)
                    .HasForeignKey(d => d.ApartmentId)
                    .HasConstraintName("FK_POI_Apartment");

                entity.HasOne(d => d.Resident)
                    .WithMany(p => p.Pois)
                    .HasForeignKey(d => d.ResidentId)
                    .HasConstraintName("FK_POI_Resident");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");

                entity.Property(e => e.ProductId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ProductID");

                entity.Property(e => e.ApproveBy).HasMaxLength(250);

                entity.Property(e => e.BelongTo)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Color).HasMaxLength(250);

                entity.Property(e => e.Image).IsUnicode(false);

                entity.Property(e => e.ProductCode)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.ProductName).HasMaxLength(250);

                entity.Property(e => e.ProductType).HasMaxLength(250);

                entity.Property(e => e.Size).HasMaxLength(250);

                entity.HasOne(d => d.BelongToNavigation)
                    .WithMany(p => p.InverseBelongToNavigation)
                    .HasForeignKey(d => d.BelongTo)
                    .HasConstraintName("FK_Product_Product1");
            });

            modelBuilder.Entity<ProductCategory>(entity =>
            {
                entity.ToTable("ProductCategory");

                entity.Property(e => e.ProductCategoryId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ProductCategoryID");

                entity.Property(e => e.CategoryName)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.ProductId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ProductID");

                entity.Property(e => e.ResidentId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ResidentID");

                entity.Property(e => e.SystemCategoryId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("SystemCategoryID");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductCategories)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_tblProductCategory_tblProduct");

                entity.HasOne(d => d.Resident)
                    .WithMany(p => p.ProductCategories)
                    .HasForeignKey(d => d.ResidentId)
                    .HasConstraintName("FK_ProductCategory_Resident");

                entity.HasOne(d => d.SystemCategory)
                    .WithMany(p => p.ProductCategories)
                    .HasForeignKey(d => d.SystemCategoryId)
                    .HasConstraintName("FK_tblProductCategory_tblSystemCategory");
            });

            modelBuilder.Entity<ProductCombination>(entity =>
            {
                entity.HasKey(e => new { e.BaseProductId, e.ProductId })
                    .HasName("PK_tblProductCombination");

                entity.ToTable("ProductCombination");

                entity.Property(e => e.BaseProductId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("BaseProductID");

                entity.Property(e => e.ProductId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ProductID");

                entity.Property(e => e.DefaultMax)
                    .HasMaxLength(10)
                    .IsFixedLength(true);

                entity.HasOne(d => d.BaseProduct)
                    .WithMany(p => p.ProductCombinationBaseProducts)
                    .HasForeignKey(d => d.BaseProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblProductCombination_tblProduct1");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductCombinationProducts)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblProductCombination_tblProduct");
            });

            modelBuilder.Entity<ProductInMenu>(entity =>
            {
                entity.ToTable("ProductInMenu");

                entity.Property(e => e.ProductInMenuId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ProductInMenuID");

                entity.Property(e => e.MenuId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("MenuID");

                entity.Property(e => e.ProductId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ProductID");

                entity.HasOne(d => d.Menu)
                    .WithMany(p => p.ProductInMenus)
                    .HasForeignKey(d => d.MenuId)
                    .HasConstraintName("FK_tblProductInMenu_tblMenu");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductInMenus)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_tblProductInMenu_tblProduct");
            });

            modelBuilder.Entity<Resident>(entity =>
            {
                entity.ToTable("Resident");

                entity.Property(e => e.ResidentId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ResidentID");

                entity.Property(e => e.AccountId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("AccountID");

                entity.Property(e => e.ApartmentId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ApartmentID");

                entity.Property(e => e.ApproveBy)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.DateOfBirth).HasColumnType("date");

                entity.Property(e => e.Gender).HasMaxLength(50);

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ResidentName).HasMaxLength(250);

                entity.Property(e => e.Type)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Residents)
                    .HasForeignKey(d => d.AccountId)
                    .HasConstraintName("FK_Resident_Account");

                entity.HasOne(d => d.AccountNavigation)
                    .WithMany(p => p.Residents)
                    .HasForeignKey(d => d.AccountId)
                    .HasConstraintName("FK_Resident_Apartment");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");

                entity.Property(e => e.RoleId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("RoleID");

                entity.Property(e => e.RoleName)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<StoreMenuDetail>(entity =>
            {
                entity.ToTable("StoreMenuDetail");

                entity.Property(e => e.StoreMenuDetailId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("StoreMenuDetailID");

                entity.Property(e => e.MenuId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("MenuID");

                entity.Property(e => e.MerchantStoreId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("MerchantStoreID");

                entity.HasOne(d => d.Menu)
                    .WithMany(p => p.StoreMenuDetails)
                    .HasForeignKey(d => d.MenuId)
                    .HasConstraintName("FK_tblStoreMenuDetail_tblMenu");

                entity.HasOne(d => d.MerchantStore)
                    .WithMany(p => p.StoreMenuDetails)
                    .HasForeignKey(d => d.MerchantStoreId)
                    .HasConstraintName("FK_tblStoreMenuDetail_tblMerchantStore");
            });

            modelBuilder.Entity<SystemCategory>(entity =>
            {
                entity.ToTable("SystemCategory");

                entity.Property(e => e.SystemCategoryId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("SystemCategoryID");

                entity.Property(e => e.ApproveBy).HasMaxLength(250);

                entity.Property(e => e.BelongTo)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.SysCategoryName).HasMaxLength(250);

                entity.HasOne(d => d.BelongToNavigation)
                    .WithMany(p => p.InverseBelongToNavigation)
                    .HasForeignKey(d => d.BelongTo)
                    .HasConstraintName("FK_SystemCategory_SystemCategory");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
