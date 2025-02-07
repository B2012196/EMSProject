﻿// <auto-generated />
using System;
using EMS.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EMS.Data.Migrations
{
    [DbContext(typeof(MyDbContext))]
    [Migration("20240627074842_second")]
    partial class second
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("EMS.Data.Entities.Equipment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("Location_Id")
                        .HasColumnType("integer");

                    b.Property<int>("Manufacturer_Id")
                        .HasColumnType("integer");

                    b.Property<DateOnly>("Mfg")
                        .HasColumnType("date");

                    b.Property<int>("Model_Id")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateOnly>("Purchase_Date")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("date")
                        .HasDefaultValueSql("NOW()");

                    b.Property<string>("Seri")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Status_Id")
                        .HasColumnType("integer");

                    b.Property<double>("TotalUsageTime")
                        .HasColumnType("double precision");

                    b.HasKey("Id");

                    b.HasIndex("Location_Id");

                    b.HasIndex("Manufacturer_Id");

                    b.HasIndex("Model_Id");

                    b.HasIndex("Status_Id");

                    b.ToTable("Equipments", (string)null);
                });

            modelBuilder.Entity("EMS.Data.Entities.EquipmentTransfer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateOnly?>("EndDate")
                        .HasColumnType("date");

                    b.Property<int>("EquipmentId")
                        .HasColumnType("integer");

                    b.Property<string>("Note")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("ReceivedLocationId")
                        .HasColumnType("integer");

                    b.Property<int>("SentLocationId")
                        .HasColumnType("integer");

                    b.Property<DateOnly>("StartDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("date")
                        .HasDefaultValueSql("NOW()");

                    b.HasKey("Id");

                    b.HasIndex("EquipmentId");

                    b.HasIndex("ReceivedLocationId");

                    b.HasIndex("SentLocationId");

                    b.ToTable("EquipmentTransfers", (string)null);
                });

            modelBuilder.Entity("EMS.Data.Entities.Inventory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("LocationId")
                        .HasColumnType("integer");

                    b.Property<int>("LowestQuantity")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Quatity")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("LocationId");

                    b.ToTable("Inventories", (string)null);
                });

            modelBuilder.Entity("EMS.Data.Entities.InventoryOrder", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateOnly>("OrderDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("date")
                        .HasDefaultValueSql("NOW()");

                    b.Property<int>("ToTalInventory")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("InventoryOrders", (string)null);
                });

            modelBuilder.Entity("EMS.Data.Entities.Location", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("Floor")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("RoomNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Locations", (string)null);
                });

            modelBuilder.Entity("EMS.Data.Entities.MaintenanceSchedule", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("EquipmentId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("ScheduledDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool>("isRepaired")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.HasIndex("EquipmentId");

                    b.ToTable("MaintenanceSchedules", (string)null);
                });

            modelBuilder.Entity("EMS.Data.Entities.Manufacturer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("character varying(10)");

                    b.HasKey("Id");

                    b.ToTable("Manufacturers", (string)null);
                });

            modelBuilder.Entity("EMS.Data.Entities.Model", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Models", (string)null);
                });

            modelBuilder.Entity("EMS.Data.Entities.OrderDetail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("InventoryId")
                        .HasColumnType("integer");

                    b.Property<int>("OrderId")
                        .HasColumnType("integer");

                    b.Property<int>("Quantity")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("InventoryId");

                    b.HasIndex("OrderId");

                    b.ToTable("OrderDetails", (string)null);
                });

            modelBuilder.Entity("EMS.Data.Entities.ReplacementRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("EquipmentId")
                        .HasColumnType("integer");

                    b.Property<int>("InventoryId")
                        .HasColumnType("integer");

                    b.Property<int>("QuantityUsed")
                        .HasColumnType("integer");

                    b.Property<DateOnly>("ReplacementDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("date")
                        .HasDefaultValueSql("NOW()");

                    b.HasKey("Id");

                    b.HasIndex("EquipmentId");

                    b.HasIndex("InventoryId");

                    b.ToTable("ReplacementRecords", (string)null);
                });

            modelBuilder.Entity("EMS.Data.Entities.Status", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Status", (string)null);
                });

            modelBuilder.Entity("EMS.Data.Entities.Token", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("Expiration")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool>("IsRevoke")
                        .HasColumnType("boolean");

                    b.Property<string>("TokenValue")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Tokens", (string)null);
                });

            modelBuilder.Entity("EMS.Data.Entities.UsageHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("EndTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("EquipmentId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("EquipmentId");

                    b.HasIndex("UserId");

                    b.ToTable("UsageHistories", (string)null);
                });

            modelBuilder.Entity("EMS.Data.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateOnly>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("date")
                        .HasDefaultValueSql("NOW()");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.Property<string>("JobPosition")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("LoginAttempts")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(0);

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<bool>("isLocked")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("EMS.Data.Entities.Equipment", b =>
                {
                    b.HasOne("EMS.Data.Entities.Location", "Location")
                        .WithMany("Equipments")
                        .HasForeignKey("Location_Id")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("FK_Equipment_Location");

                    b.HasOne("EMS.Data.Entities.Manufacturer", "Manufacturer")
                        .WithMany("Equipments")
                        .HasForeignKey("Manufacturer_Id")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("FK_Equipment_Manufacturer");

                    b.HasOne("EMS.Data.Entities.Model", "Model")
                        .WithMany("Equipments")
                        .HasForeignKey("Model_Id")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("FK_Equipment_Model");

                    b.HasOne("EMS.Data.Entities.Status", "Status")
                        .WithMany("Equipments")
                        .HasForeignKey("Status_Id")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("FK_Equipment_Status");

                    b.Navigation("Location");

                    b.Navigation("Manufacturer");

                    b.Navigation("Model");

                    b.Navigation("Status");
                });

            modelBuilder.Entity("EMS.Data.Entities.EquipmentTransfer", b =>
                {
                    b.HasOne("EMS.Data.Entities.Equipment", "Equipment")
                        .WithMany("EquipmentTransfers")
                        .HasForeignKey("EquipmentId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("FK_EquipmentTransfer_Equipment");

                    b.HasOne("EMS.Data.Entities.Location", "ReceivedLocation")
                        .WithMany("ReceivedTransfers")
                        .HasForeignKey("ReceivedLocationId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("FK_EquipmentTransfer_ReceivedLocation");

                    b.HasOne("EMS.Data.Entities.Location", "SentLocation")
                        .WithMany("SentTransfers")
                        .HasForeignKey("SentLocationId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("FK_EquipmentTransfer_SentLocation");

                    b.Navigation("Equipment");

                    b.Navigation("ReceivedLocation");

                    b.Navigation("SentLocation");
                });

            modelBuilder.Entity("EMS.Data.Entities.Inventory", b =>
                {
                    b.HasOne("EMS.Data.Entities.Location", "Location")
                        .WithMany("Inventories")
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("FK_Inventory_Location");

                    b.Navigation("Location");
                });

            modelBuilder.Entity("EMS.Data.Entities.MaintenanceSchedule", b =>
                {
                    b.HasOne("EMS.Data.Entities.Equipment", "Equipment")
                        .WithMany("MaintenanceSchedules")
                        .HasForeignKey("EquipmentId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("FK_MaintenanceSchedule_Equipment");

                    b.Navigation("Equipment");
                });

            modelBuilder.Entity("EMS.Data.Entities.OrderDetail", b =>
                {
                    b.HasOne("EMS.Data.Entities.Inventory", "Inventory")
                        .WithMany("OrderDetails")
                        .HasForeignKey("InventoryId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("FK_OrderDetail_Inventory");

                    b.HasOne("EMS.Data.Entities.InventoryOrder", "InventoryOrder")
                        .WithMany("OrderDetails")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("FK_OrderDetail_Order");

                    b.Navigation("Inventory");

                    b.Navigation("InventoryOrder");
                });

            modelBuilder.Entity("EMS.Data.Entities.ReplacementRecord", b =>
                {
                    b.HasOne("EMS.Data.Entities.Equipment", "Equipment")
                        .WithMany("ReplacementRecords")
                        .HasForeignKey("EquipmentId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("FK_ReplacementRecord_Equipment");

                    b.HasOne("EMS.Data.Entities.Inventory", "Inventory")
                        .WithMany("ReplacementRecords")
                        .HasForeignKey("InventoryId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("FK_ReplacementRecord_Inventory");

                    b.Navigation("Equipment");

                    b.Navigation("Inventory");
                });

            modelBuilder.Entity("EMS.Data.Entities.Token", b =>
                {
                    b.HasOne("EMS.Data.Entities.User", "User")
                        .WithMany("Tokens")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("FK_Token_User");

                    b.Navigation("User");
                });

            modelBuilder.Entity("EMS.Data.Entities.UsageHistory", b =>
                {
                    b.HasOne("EMS.Data.Entities.Equipment", "Equipment")
                        .WithMany("UsageHistories")
                        .HasForeignKey("EquipmentId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("FK_UsageHistory_Equipment");

                    b.HasOne("EMS.Data.Entities.User", "User")
                        .WithMany("UsageHistories")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("FK_UsageHistory_User");

                    b.Navigation("Equipment");

                    b.Navigation("User");
                });

            modelBuilder.Entity("EMS.Data.Entities.Equipment", b =>
                {
                    b.Navigation("EquipmentTransfers");

                    b.Navigation("MaintenanceSchedules");

                    b.Navigation("ReplacementRecords");

                    b.Navigation("UsageHistories");
                });

            modelBuilder.Entity("EMS.Data.Entities.Inventory", b =>
                {
                    b.Navigation("OrderDetails");

                    b.Navigation("ReplacementRecords");
                });

            modelBuilder.Entity("EMS.Data.Entities.InventoryOrder", b =>
                {
                    b.Navigation("OrderDetails");
                });

            modelBuilder.Entity("EMS.Data.Entities.Location", b =>
                {
                    b.Navigation("Equipments");

                    b.Navigation("Inventories");

                    b.Navigation("ReceivedTransfers");

                    b.Navigation("SentTransfers");
                });

            modelBuilder.Entity("EMS.Data.Entities.Manufacturer", b =>
                {
                    b.Navigation("Equipments");
                });

            modelBuilder.Entity("EMS.Data.Entities.Model", b =>
                {
                    b.Navigation("Equipments");
                });

            modelBuilder.Entity("EMS.Data.Entities.Status", b =>
                {
                    b.Navigation("Equipments");
                });

            modelBuilder.Entity("EMS.Data.Entities.User", b =>
                {
                    b.Navigation("Tokens");

                    b.Navigation("UsageHistories");
                });
#pragma warning restore 612, 618
        }
    }
}
