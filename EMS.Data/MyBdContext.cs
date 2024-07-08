using EMS.Data.Entities;
using Microsoft.EntityFrameworkCore;
//thêm user, quipment
namespace EMS.Data
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options) { }

        public DbSet<Equipment> Equipments { get; set; }
        public DbSet<Model> Models { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<Location> Locations { get;set; }
        public DbSet<EquipmentTransfer> EquipmentTransfers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Token> Tokens { get; set; }
        public DbSet<UsageHistory> UsageHistories { get; set; }
        public DbSet<MaintenanceSchedule> MaintenanceSchedules { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<InventoryOrder> InventoryOrders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<ReplacementRecord> ReplacementRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Model>(entity =>
            {
                entity.ToTable("Models");
                entity.HasKey(m => m.Id);
                entity.Property(m => m.Name).IsRequired();

            });

            modelBuilder.Entity<Manufacturer>(entity =>
            {
                entity.ToTable("Manufacturers");
                entity.HasKey(m => m.Id);
                entity.Property(m => m.Name).IsRequired();
                entity.Property(m => m.Address).IsRequired().HasMaxLength(100);
                entity.Property(m => m.Phone).IsRequired().HasMaxLength(10);


            });


            modelBuilder.Entity<Equipment>(entity =>
            {
                entity.ToTable("Equipments");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Purchase_Date).HasDefaultValueSql("NOW()");

                //Equipment_Model
                entity.HasOne(e => e.Model)
                    .WithMany(m => m.Equipments)
                    .HasForeignKey(e => e.Model_Id)
                    .HasConstraintName("FK_Equipment_Model")
                    .OnDelete(DeleteBehavior.Restrict);
                //Equipment_Manufacturer
                entity.HasOne(e => e.Manufacturer)
                    .WithMany(m => m.Equipments)
                    .HasForeignKey(e => e.Manufacturer_Id)
                    .HasConstraintName("FK_Equipment_Manufacturer")
                    .OnDelete(DeleteBehavior.Restrict);
                //Equipment_Status
                entity.HasOne(e => e.Status)
                    .WithMany(m => m.Equipments)
                    .HasForeignKey(e => e.Status_Id)
                    .HasConstraintName("FK_Equipment_Status")
                    .OnDelete(DeleteBehavior.Restrict);
                //Equipment_Location
                entity.HasOne(e => e.Location)
                    .WithMany(m => m.Equipments)
                    .HasForeignKey(e => e.Location_Id)
                    .HasConstraintName("FK_Equipment_Location")
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Status>(entity =>
            {
                entity.ToTable("Status");
                entity.HasKey(s => s.Id);
                entity.Property(s => s.Name).IsRequired();

            });

            modelBuilder.Entity<Location>(entity =>
            {
                entity.ToTable("Locations");
                entity.HasKey(s => s.Id);
                entity.Property(s => s.Name).IsRequired();
                entity.Property(s => s.Floor).IsRequired();
                entity.Property(s => s.RoomNumber).IsRequired();

            });

            modelBuilder.Entity<EquipmentTransfer>(entity =>
            {
                entity.ToTable("EquipmentTransfers");
                entity.HasKey(s => s.Id);
                entity.Property(s => s.ReceivedLocationId).IsRequired();
                entity.Property(s => s.SentLocationId).IsRequired();
                entity.Property(s => s.Id).IsRequired();
                entity.Property(s => s.StartDate).HasDefaultValueSql("NOW()");
                entity.Property(s => s.EndDate).IsRequired(false);
                //Relationship

                entity.HasOne(et => et.ReceivedLocation)
                    .WithMany(l => l.ReceivedTransfers)
                    .HasForeignKey(et => et.ReceivedLocationId)
                    .HasConstraintName("FK_EquipmentTransfer_ReceivedLocation")
                    .OnDelete(DeleteBehavior.Restrict);


                entity.HasOne(et => et.SentLocation)
                    .WithMany(l => l.SentTransfers)
                    .HasForeignKey(et => et.SentLocationId)
                    .HasConstraintName("FK_EquipmentTransfer_SentLocation")
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(et => et.Equipment)
                    .WithMany(e => e.EquipmentTransfers)
                    .HasForeignKey(et => et.EquipmentId)
                    .HasConstraintName("FK_EquipmentTransfer_Equipment")
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Username).IsRequired().HasMaxLength(20);
                entity.HasIndex(e => e.Username).IsUnique();

                entity.Property(e => e.PasswordHash).IsRequired();

                entity.Property(e => e.Email).IsRequired();
                entity.HasIndex(e => e.Email).IsUnique();

                entity.Property(e => e.FullName).IsRequired().HasMaxLength(30);
                entity.Property(e => e.JobPosition).IsRequired();
                entity.Property(e => e.Role).IsRequired();
                entity.Property(e => e.LoginAttempts).IsRequired().HasDefaultValue(0);
                entity.Property(e => e.isLocked).IsRequired();
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("NOW()");
            });

            modelBuilder.Entity<Token>(entity =>
            {
                entity.ToTable("Tokens");
                entity.HasKey(t => t.Id);
                entity.Property(t => t.TokenValue).IsRequired();
                entity.Property(e => e.CreatedAt)
                  .HasColumnType("timestamp without time zone");
                entity.Property(e => e.Expiration)
                      .HasColumnType("timestamp without time zone");

                //Token_User
                entity.HasOne(t => t.User)
                    .WithMany(u => u.Tokens)
                    .HasForeignKey(t => t.UserId)
                    .HasConstraintName("FK_Token_User")
                    .OnDelete(DeleteBehavior.Restrict);
            });


            modelBuilder.Entity<UsageHistory>(entity =>
            {
                entity.ToTable("UsageHistories");
                entity.HasKey(u => u.Id);
                entity.Property(u => u.StartTime).IsRequired();              
                entity.Property(u => u.StartTime)
                  .HasColumnType("timestamp without time zone");
                entity.Property(u => u.EndTime)
                  .HasColumnType("timestamp without time zone");
                //UsageHistory_User
                entity.HasOne(t => t.User)
                    .WithMany(u => u.UsageHistories)
                    .HasForeignKey(t => t.UserId)
                    .HasConstraintName("FK_UsageHistory_User")
                    .OnDelete(DeleteBehavior.Restrict);
                //UsageHistory_Equipment
                entity.HasOne(t => t.Equipment)
                    .WithMany(u => u.UsageHistories)
                    .HasForeignKey(t => t.EquipmentId)
                    .HasConstraintName("FK_UsageHistory_Equipment")
                    .OnDelete(DeleteBehavior.Restrict);

            });

            modelBuilder.Entity<MaintenanceSchedule>(entity =>
            {
                entity.ToTable("MaintenanceSchedules");
                entity.HasKey(m => m.Id);
                entity.Property(m => m.ScheduledDate).IsRequired();
                entity.Property(m => m.Description).IsRequired();
                entity.Property(m => m.ScheduledDate)
                  .HasColumnType("timestamp without time zone");
                //MaintenanceSchedule_Equipment
                entity.HasOne(t => t.Equipment)
                    .WithMany(u => u.MaintenanceSchedules)
                    .HasForeignKey(t => t.EquipmentId)
                    .HasConstraintName("FK_MaintenanceSchedule_Equipment")
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Inventory>(entity =>
            {
                entity.ToTable("Inventories");
                entity.HasKey(i => i.Id);
                //Inventory_Location
                entity.HasOne(t => t.Location)
                    .WithMany(u => u.Inventories)
                    .HasForeignKey(t => t.LocationId)
                    .HasConstraintName("FK_Inventory_Location")
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<InventoryOrder>(entity =>
            {
                entity.ToTable("InventoryOrders");
                entity.HasKey(i => i.Id);
                entity.Property(i => i.OrderDate).HasDefaultValueSql("NOW()");

            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.ToTable("OrderDetails");
                entity.HasKey(i => i.Id);
                //Inventory_Location
                entity.HasOne(t => t.Inventory)
                    .WithMany(u => u.OrderDetails)
                    .HasForeignKey(t => t.InventoryId)
                    .HasConstraintName("FK_OrderDetail_Inventory")
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(t => t.InventoryOrder)
                    .WithMany(u => u.OrderDetails)
                    .HasForeignKey(t => t.OrderId)
                    .HasConstraintName("FK_OrderDetail_Order")
                    .OnDelete(DeleteBehavior.Restrict);
            });


            modelBuilder.Entity<ReplacementRecord>(entity =>
            {
                entity.ToTable("ReplacementRecords");
                entity.HasKey(i => i.Id);
                entity.Property(i => i.ReplacementDate).HasDefaultValueSql("NOW()");
                //Inventory_Location
                entity.HasOne(t => t.Inventory)
                    .WithMany(u => u.ReplacementRecords)
                    .HasForeignKey(t => t.InventoryId)
                    .HasConstraintName("FK_ReplacementRecord_Inventory")
                    .OnDelete(DeleteBehavior.Restrict);
                
                entity.HasOne(t => t.Equipment)
                    .WithMany(u => u.ReplacementRecords)
                    .HasForeignKey(t => t.EquipmentId)
                    .HasConstraintName("FK_ReplacementRecord_Equipment")
                    .OnDelete(DeleteBehavior.Restrict);
            });

           base.OnModelCreating(modelBuilder);
        }
    }
}
