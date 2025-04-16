using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace RepMed.Data;

public partial class RepMedContext : DbContext
{
    public RepMedContext()
    {
    }

    public RepMedContext(DbContextOptions<RepMedContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Deliverydetail> Deliverydetails { get; set; }

    public virtual DbSet<Deliveryperson> Deliverypersons { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Orderdetail> Orderdetails { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Person> Persons { get; set; }

    public virtual DbSet<Pharmacy> Pharmacies { get; set; }

    public virtual DbSet<Pharmacybankdetail> Pharmacybankdetails { get; set; }

    public virtual DbSet<Pharmacyinventory> Pharmacyinventories { get; set; }

    public virtual DbSet<Pharmacypurchase> Pharmacypurchases { get; set; }

    public virtual DbSet<Prescription> Prescriptions { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Refund> Refunds { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Settlement> Settlements { get; set; }

    public virtual DbSet<State> States { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Useraddress> Useraddresses { get; set; }

    public virtual DbSet<Userjwttokenlog> Userjwttokenlogs { get; set; }

    public virtual DbSet<Userrole> Userroles { get; set; }

    public virtual DbSet<Usertoken> Usertokens { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;port=3306;database=repmed;user=arka;password=Admin@1234", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.37-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("category");

            entity.HasIndex(e => e.CreatedBy, "fk_category_createdby");

            entity.HasIndex(e => e.UpdatedBy, "fk_category_updatedby");

            entity.Property(e => e.CategoryName).HasMaxLength(200);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.IsActive)
                .HasDefaultValueSql("b'1'")
                .HasColumnType("bit(1)");
            entity.Property(e => e.IsDeleted).HasDefaultValueSql("'0'");
            entity.Property(e => e.UpdatedDate)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.Url)
                .HasMaxLength(1000)
                .HasColumnName("url");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.CategoryCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_category_createdby");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.CategoryUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_category_updatedby");
        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("cities");

            entity.HasIndex(e => e.StateId, "StateId");

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.HasOne(d => d.State).WithMany(p => p.Cities)
                .HasForeignKey(d => d.StateId)
                .HasConstraintName("cities_ibfk_1");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("countries");

            entity.HasIndex(e => e.Name, "Name").IsUnique();

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);
        });

        modelBuilder.Entity<Deliverydetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("deliverydetails");

            entity.HasIndex(e => e.DeliveryPersonId, "DeliveryPersonId");

            entity.HasIndex(e => e.OrderId, "OrderId");

            entity.Property(e => e.ActualDeliveryTime).HasColumnType("datetime");
            entity.Property(e => e.Codcollected)
                .HasDefaultValueSql("'0'")
                .HasColumnName("CODCollected");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.DeliveredAt).HasColumnType("datetime");
            entity.Property(e => e.DeliveryStatus)
                .HasDefaultValueSql("'Pending'")
                .HasColumnType("enum('Pending','Out for Delivery','Delivered')");
            entity.Property(e => e.EstimatedDelivery).HasColumnType("datetime");
            entity.Property(e => e.IsCod)
                .HasDefaultValueSql("'0'")
                .HasColumnName("IsCOD");
            entity.Property(e => e.UpdatedDate)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");

            entity.HasOne(d => d.DeliveryPerson).WithMany(p => p.Deliverydetails)
                .HasForeignKey(d => d.DeliveryPersonId)
                .HasConstraintName("deliverydetails_ibfk_3");

            entity.HasOne(d => d.Order).WithMany(p => p.Deliverydetails)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("deliverydetails_ibfk_1");
        });

        modelBuilder.Entity<Deliveryperson>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("deliverypersons");

            entity.HasIndex(e => e.AssignedPharmacyId, "AssignedPharmacyId");

            entity.HasIndex(e => e.GovernmentIdNumber, "GovernmentIdNumber").IsUnique();

            entity.HasIndex(e => e.UserId, "UserId");

            entity.Property(e => e.AccountHolderName).HasMaxLength(100);
            entity.Property(e => e.BankAccountNumber).HasMaxLength(50);
            entity.Property(e => e.BankName).HasMaxLength(100);
            entity.Property(e => e.BranchName).HasMaxLength(100);
            entity.Property(e => e.GovernmentIdNumber)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.GovernmentIdType)
                .IsRequired()
                .HasColumnType("enum('Aadhar','Driver’s License','Passport')");
            entity.Property(e => e.Ifsccode)
                .HasMaxLength(20)
                .HasColumnName("IFSCCode");
            entity.Property(e => e.IsActive).HasDefaultValueSql("'1'");
            entity.Property(e => e.UpiId)
                .HasMaxLength(100)
                .HasColumnName("UPI_ID");
            entity.Property(e => e.VehicleDetails).HasMaxLength(255);

            entity.HasOne(d => d.AssignedPharmacy).WithMany(p => p.Deliverypeople)
                .HasForeignKey(d => d.AssignedPharmacyId)
                .HasConstraintName("deliverypersons_ibfk_2");

            entity.HasOne(d => d.User).WithMany(p => p.Deliverypeople)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("deliverypersons_ibfk_1");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("orders");

            entity.HasIndex(e => e.CityId, "CityId");

            entity.HasIndex(e => e.CountryId, "CountryId");

            entity.HasIndex(e => e.CustomerId, "CustomerId");

            entity.HasIndex(e => e.PharmacyId, "PharmacyId");

            entity.HasIndex(e => e.StateId, "StateId");

            entity.Property(e => e.Address1).HasColumnType("text");
            entity.Property(e => e.Address2).HasColumnType("text");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.IsDiffAddress).HasDefaultValueSql("'0'");
            entity.Property(e => e.Latitude).HasPrecision(9, 6);
            entity.Property(e => e.Longitude).HasPrecision(9, 6);
            entity.Property(e => e.OrderStatus)
                .HasDefaultValueSql("'Pending'")
                .HasColumnType("enum('Pending','Confirmed','Shipped','Delivered','Cancelled','Returned')");
            entity.Property(e => e.Pincode).HasMaxLength(10);
            entity.Property(e => e.TotalAmount).HasPrecision(10, 2);
            entity.Property(e => e.UpdatedDate)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");

            entity.HasOne(d => d.City).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CityId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("orders_ibfk_3");

            entity.HasOne(d => d.Country).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("orders_ibfk_5");

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("orders_ibfk_1");

            entity.HasOne(d => d.Pharmacy).WithMany(p => p.Orders)
                .HasForeignKey(d => d.PharmacyId)
                .HasConstraintName("orders_ibfk_2");

            entity.HasOne(d => d.State).WithMany(p => p.Orders)
                .HasForeignKey(d => d.StateId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("orders_ibfk_4");
        });

        modelBuilder.Entity<Orderdetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("orderdetails");

            entity.HasIndex(e => e.OrderId, "OrderId");

            entity.HasIndex(e => e.PharmacyInventoryId, "PharmacyInventoryId");

            entity.Property(e => e.Discount)
                .HasPrecision(10, 2)
                .HasDefaultValueSql("'0.00'");
            entity.Property(e => e.Price).HasPrecision(10, 2);
            entity.Property(e => e.TotalPrice)
                .HasPrecision(10, 2)
                .HasComputedColumnSql("`Quantity` * `Price`", true);

            entity.HasOne(d => d.Order).WithMany(p => p.Orderdetails)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("orderdetails_ibfk_1");

            entity.HasOne(d => d.PharmacyInventory).WithMany(p => p.Orderdetails)
                .HasForeignKey(d => d.PharmacyInventoryId)
                .HasConstraintName("orderdetails_ibfk_2");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("payments");

            entity.HasIndex(e => e.CustomerId, "CustomerId");

            entity.HasIndex(e => e.OrderId, "OrderId");

            entity.HasIndex(e => e.TransactionId, "TransactionId").IsUnique();

            entity.Property(e => e.AmountPaid).HasPrecision(10, 2);
            entity.Property(e => e.FailedReason).HasMaxLength(500);
            entity.Property(e => e.PaymentDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.PaymentMethod)
                .IsRequired()
                .HasColumnType("enum('Credit Card','Debit Card','UPI','Net Banking','Cash on Delivery')");
            entity.Property(e => e.PaymentStatus)
                .HasDefaultValueSql("'Pending'")
                .HasColumnType("enum('Pending','Completed','Failed')");

            entity.HasOne(d => d.Customer).WithMany(p => p.Payments)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("payments_ibfk_2");

            entity.HasOne(d => d.Order).WithMany(p => p.Payments)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("payments_ibfk_1");
        });

        modelBuilder.Entity<Person>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("persons");

            entity.HasIndex(e => e.Email, "Email").IsUnique();

            entity.HasIndex(e => e.Mobile, "Mobile").IsUnique();

            entity.Property(e => e.BloodGroup).HasMaxLength(10);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).IsRequired();
            entity.Property(e => e.EmailVerified).HasDefaultValueSql("'0'");
            entity.Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.Gender).HasColumnType("enum('Male','Female','Other')");
            entity.Property(e => e.LastName)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.Mobile)
                .IsRequired()
                .HasMaxLength(20);
            entity.Property(e => e.MobileVerified).HasDefaultValueSql("'0'");
            entity.Property(e => e.MotherName).HasMaxLength(100);
            entity.Property(e => e.Picture).HasMaxLength(255);
            entity.Property(e => e.Qualification).HasMaxLength(255);
            entity.Property(e => e.Signature).HasMaxLength(255);
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<Pharmacy>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("pharmacies");

            entity.HasIndex(e => e.CityId, "CityId");

            entity.HasIndex(e => e.CountryId, "CountryId");

            entity.HasIndex(e => e.Gstnumber, "GSTNumber").IsUnique();

            entity.HasIndex(e => e.LicenseNumber, "LicenseNumber").IsUnique();

            entity.HasIndex(e => e.StateId, "StateId");

            entity.HasIndex(e => e.UserId, "UserId");

            entity.Property(e => e.Address1).HasColumnType("text");
            entity.Property(e => e.Address2).HasColumnType("text");
            entity.Property(e => e.BusinessName).HasMaxLength(255);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.EmailVerified).HasDefaultValueSql("'0'");
            entity.Property(e => e.Gstnumber)
                .HasMaxLength(50)
                .HasColumnName("GSTNumber");
            entity.Property(e => e.Latitude).HasPrecision(10, 7);
            entity.Property(e => e.LicenseNumber)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.Longitude).HasPrecision(10, 7);
            entity.Property(e => e.MobileVerified).HasDefaultValueSql("'0'");
            entity.Property(e => e.OfficialEmail).HasMaxLength(255);
            entity.Property(e => e.Otpverified)
                .HasDefaultValueSql("'0'")
                .HasColumnName("OTPVerified");
            entity.Property(e => e.OwnerName).HasMaxLength(100);
            entity.Property(e => e.RegisteredMobile).HasMaxLength(20);
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'Pending'")
                .HasColumnType("enum('Active','Inactive','Suspended','Pending','Rejected')");
            entity.Property(e => e.StoreEmail1).HasMaxLength(255);
            entity.Property(e => e.StoreEmail2).HasMaxLength(255);
            entity.Property(e => e.StoreMobile1).HasMaxLength(20);
            entity.Property(e => e.StoreName)
                .IsRequired()
                .HasMaxLength(255);

            entity.HasOne(d => d.City).WithMany(p => p.Pharmacies)
                .HasForeignKey(d => d.CityId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("pharmacies_ibfk_2");

            entity.HasOne(d => d.Country).WithMany(p => p.Pharmacies)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("pharmacies_ibfk_4");

            entity.HasOne(d => d.State).WithMany(p => p.Pharmacies)
                .HasForeignKey(d => d.StateId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("pharmacies_ibfk_3");

            entity.HasOne(d => d.User).WithMany(p => p.Pharmacies)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("pharmacies_ibfk_1");
        });

        modelBuilder.Entity<Pharmacybankdetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("pharmacybankdetails");

            entity.HasIndex(e => e.AccountNumber, "AccountNumber").IsUnique();

            entity.HasIndex(e => e.PharmacyId, "PharmacyId");

            entity.Property(e => e.AccountHolderName)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.AccountNumber)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.BankName)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.BranchName).HasMaxLength(255);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.Ifsccode)
                .IsRequired()
                .HasMaxLength(20)
                .HasColumnName("IFSCCode");
            entity.Property(e => e.UpdatedDate)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.UpiId).HasMaxLength(100);

            entity.HasOne(d => d.Pharmacy).WithMany(p => p.Pharmacybankdetails)
                .HasForeignKey(d => d.PharmacyId)
                .HasConstraintName("pharmacybankdetails_ibfk_1");
        });

        modelBuilder.Entity<Pharmacyinventory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("pharmacyinventory");

            entity.HasIndex(e => e.PharmacyId, "PharmacyId");

            entity.HasIndex(e => e.PharmacyPurchaseId, "PharmacyPurchaseId");

            entity.HasIndex(e => e.ProductId, "ProductId");

            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.ExpiryDate).HasColumnType("datetime");
            entity.Property(e => e.LastRestocked)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.LastUpdated)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.MinimumStockThreshold).HasDefaultValueSql("'10'");
            entity.Property(e => e.SellingPrice).HasPrecision(10, 2);

            entity.HasOne(d => d.Pharmacy).WithMany(p => p.Pharmacyinventories)
                .HasForeignKey(d => d.PharmacyId)
                .HasConstraintName("pharmacyinventory_ibfk_1");

            entity.HasOne(d => d.PharmacyPurchase).WithMany(p => p.Pharmacyinventories)
                .HasForeignKey(d => d.PharmacyPurchaseId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("pharmacyinventory_ibfk_3");

            entity.HasOne(d => d.Product).WithMany(p => p.Pharmacyinventories)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("pharmacyinventory_ibfk_2");
        });

        modelBuilder.Entity<Pharmacypurchase>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("pharmacypurchases");

            entity.HasIndex(e => e.InvoiceNumber, "InvoiceNumber").IsUnique();

            entity.HasIndex(e => e.PharmacyId, "PharmacyId");

            entity.HasIndex(e => e.ProductId, "ProductId");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.ExpiryDate).HasColumnType("datetime");
            entity.Property(e => e.Gstamount)
                .HasPrecision(10, 2)
                .HasComputedColumnSql("case when (`GSTIncluded` = true) then 0 else ((`ProductPrice` * `GSTPercentage`) / 100) end", true)
                .HasColumnName("GSTAmount");
            entity.Property(e => e.Gstincluded)
                .HasDefaultValueSql("'0'")
                .HasColumnName("GSTIncluded");
            entity.Property(e => e.Gstpercentage)
                .HasPrecision(5, 2)
                .HasDefaultValueSql("'0.00'")
                .HasColumnName("GSTPercentage");
            entity.Property(e => e.InvoiceNumber)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.Manufacturer).HasMaxLength(255);
            entity.Property(e => e.ProductPrice).HasPrecision(10, 2);
            entity.Property(e => e.PurchaseDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'Pending'")
                .HasColumnType("enum('Pending','Completed','Cancelled')");
            entity.Property(e => e.SupplierName)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.TotalCost)
                .HasPrecision(10, 2)
                .HasComputedColumnSql("`QuantityPurchased` * (`ProductPrice` + `GSTAmount`)", true);
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Pharmacy).WithMany(p => p.Pharmacypurchases)
                .HasForeignKey(d => d.PharmacyId)
                .HasConstraintName("pharmacypurchases_ibfk_1");

            entity.HasOne(d => d.Product).WithMany(p => p.Pharmacypurchases)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("pharmacypurchases_ibfk_2");
        });

        modelBuilder.Entity<Prescription>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("prescription");

            entity.HasIndex(e => e.CustomerId, "CustomerId");

            entity.HasIndex(e => e.OrderId, "OrderId");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.Note).HasColumnType("text");
            entity.Property(e => e.UploadedFile)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.Verified).HasDefaultValueSql("'0'");
            entity.Property(e => e.VerifiedByDoctor).HasMaxLength(255);
            entity.Property(e => e.VerifiedDate).HasColumnType("datetime");

            entity.HasOne(d => d.Customer).WithMany(p => p.Prescriptions)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("prescription_ibfk_1");

            entity.HasOne(d => d.Order).WithMany(p => p.Prescriptions)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("prescription_ibfk_2");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("product");

            entity.HasIndex(e => e.CreatedBy, "fk_product_createdby");

            entity.HasIndex(e => e.UpdatedBy, "fk_product_updatedby");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.Discount).HasPrecision(18, 2);
            entity.Property(e => e.Image).HasMaxLength(100);
            entity.Property(e => e.IsDeleted).HasDefaultValueSql("'0'");
            entity.Property(e => e.Mrp)
                .HasPrecision(18, 2)
                .HasColumnName("MRP");
            entity.Property(e => e.Name).HasMaxLength(300);
            entity.Property(e => e.Price).HasPrecision(18, 2);
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ProductCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_product_createdby");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.ProductUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_product_updatedby");
        });

        modelBuilder.Entity<Refund>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("refunds");

            entity.HasIndex(e => e.OrderId, "OrderId");

            entity.HasIndex(e => e.PaymentId, "PaymentId");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.ProcessedDate).HasColumnType("datetime");
            entity.Property(e => e.Reason).HasColumnType("text");
            entity.Property(e => e.RefundAmount).HasPrecision(10, 2);
            entity.Property(e => e.RefundStatus)
                .HasDefaultValueSql("'Pending'")
                .HasColumnType("enum('Pending','Processed','Rejected')");

            entity.HasOne(d => d.Order).WithMany(p => p.Refunds)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("refunds_ibfk_1");

            entity.HasOne(d => d.Payment).WithMany(p => p.Refunds)
                .HasForeignKey(d => d.PaymentId)
                .HasConstraintName("refunds_ibfk_2");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("roles");

            entity.HasIndex(e => e.RoleName, "RoleName").IsUnique();

            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.RoleName)
                .IsRequired()
                .HasMaxLength(50);
        });

        modelBuilder.Entity<Settlement>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("settlement");

            entity.HasIndex(e => e.PharmacyId, "PharmacyId");

            entity.HasIndex(e => e.TransactionId, "TransactionID").IsUnique();

            entity.Property(e => e.AmountPaid).HasPrecision(10, 2);
            entity.Property(e => e.CommissionAmount)
                .HasPrecision(10, 2)
                .HasDefaultValueSql("'0.00'");
            entity.Property(e => e.Notes).HasColumnType("text");
            entity.Property(e => e.PaymentDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.PaymentStatus)
                .HasDefaultValueSql("'Pending'")
                .HasColumnType("enum('Pending','Processed','Failed')");
            entity.Property(e => e.SettlementPeriod)
                .IsRequired()
                .HasColumnType("enum('Weekly','Monthly')");
            entity.Property(e => e.TotalAmountDue).HasPrecision(10, 2);
            entity.Property(e => e.TransactionId)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("TransactionID");

            entity.HasOne(d => d.Pharmacy).WithMany(p => p.Settlements)
                .HasForeignKey(d => d.PharmacyId)
                .HasConstraintName("admintopharmacysettlements_ibfk_1");
        });

        modelBuilder.Entity<State>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("states");

            entity.HasIndex(e => e.CountryId, "CountryId");

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.HasOne(d => d.Country).WithMany(p => p.States)
                .HasForeignKey(d => d.CountryId)
                .HasConstraintName("states_ibfk_1");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("users");

            entity.HasIndex(e => e.PersonId, "PersonId");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.IsDeleted).HasDefaultValueSql("'0'");
            entity.Property(e => e.IsLocked).HasDefaultValueSql("'0'");
            entity.Property(e => e.LastLoginDate).HasColumnType("datetime");
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.PasswordHash)
                .IsRequired()
                .HasColumnType("blob");
            entity.Property(e => e.PasswordSalt)
                .IsRequired()
                .HasColumnType("blob");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'Pending'")
                .HasColumnType("enum('Active','Inactive','Suspended','Pending')");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Person).WithMany(p => p.Users)
                .HasForeignKey(d => d.PersonId)
                .HasConstraintName("users_ibfk_1");
        });

        modelBuilder.Entity<Useraddress>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("useraddresses");

            entity.HasIndex(e => e.CityId, "CityId");

            entity.HasIndex(e => e.CountryId, "CountryId");

            entity.HasIndex(e => e.PersonId, "PersonId");

            entity.HasIndex(e => e.StateId, "StateId");

            entity.Property(e => e.AddressLine).HasColumnType("text");
            entity.Property(e => e.Latitude).HasPrecision(9, 6);
            entity.Property(e => e.Longitude).HasPrecision(9, 6);
            entity.Property(e => e.Pincode).HasMaxLength(10);

            entity.HasOne(d => d.City).WithMany(p => p.Useraddresses)
                .HasForeignKey(d => d.CityId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("useraddresses_ibfk_2");

            entity.HasOne(d => d.Country).WithMany(p => p.Useraddresses)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("useraddresses_ibfk_5");

            entity.HasOne(d => d.Person).WithMany(p => p.Useraddresses)
                .HasForeignKey(d => d.PersonId)
                .HasConstraintName("useraddresses_ibfk_3");

            entity.HasOne(d => d.State).WithMany(p => p.Useraddresses)
                .HasForeignKey(d => d.StateId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("useraddresses_ibfk_4");
        });

        modelBuilder.Entity<Userjwttokenlog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("userjwttokenlog");

            entity.HasIndex(e => e.UserId, "UserId");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Token)
                .IsRequired()
                .HasColumnType("text");
            entity.Property(e => e.TokenValidTill).HasColumnType("datetime");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.User).WithMany(p => p.Userjwttokenlogs)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("userjwttokenlog_ibfk_1");
        });

        modelBuilder.Entity<Userrole>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("userroles");

            entity.HasIndex(e => e.RoleId, "RoleId");

            entity.HasIndex(e => e.UserId, "UserId");

            entity.HasOne(d => d.Role).WithMany()
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("userroles_ibfk_2");

            entity.HasOne(d => d.User).WithMany()
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("userroles_ibfk_1");
        });

        modelBuilder.Entity<Usertoken>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("usertokens");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.ExpiresAt).HasColumnType("datetime");
            entity.Property(e => e.IsUsed).HasDefaultValueSql("'0'");
            entity.Property(e => e.Token)
                .IsRequired()
                .HasMaxLength(255);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
