﻿// <auto-generated />
using System;
using Edelstein.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Edelstein.Data.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.0-rtm-35687")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Edelstein.Data.Entities.Account", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<byte?>("Gender");

                    b.Property<string>("LatestConnectedService");

                    b.Property<byte>("LatestConnectedWorld");

                    b.Property<int>("MaplePoint");

                    b.Property<int>("NexonCash");

                    b.Property<string>("Password")
                        .HasMaxLength(128);

                    b.Property<int>("PrepaidNXCash");

                    b.Property<string>("PreviousConnectedService");

                    b.Property<string>("SecondPassword")
                        .HasMaxLength(128);

                    b.Property<string>("Username")
                        .HasMaxLength(13);

                    b.HasKey("ID");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("Edelstein.Data.Entities.AccountData", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("AccountID");

                    b.Property<int?>("LockerID");

                    b.Property<int>("SlotCount");

                    b.Property<int?>("TrunkID");

                    b.Property<byte>("WorldID");

                    b.HasKey("ID");

                    b.HasIndex("AccountID");

                    b.HasIndex("LockerID");

                    b.HasIndex("TrunkID");

                    b.ToTable("AccountData");
                });

            modelBuilder.Entity("Edelstein.Data.Entities.Character", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<short>("AP");

                    b.Property<short>("DEX");

                    b.Property<int?>("DataID");

                    b.Property<int>("EXP");

                    b.Property<int>("Face");

                    b.Property<int>("FieldID");

                    b.Property<byte>("FieldPortal");

                    b.Property<byte>("Gender");

                    b.Property<int>("HP");

                    b.Property<int>("Hair");

                    b.Property<short>("INT");

                    b.Property<short>("Job");

                    b.Property<short>("LUK");

                    b.Property<byte>("Level");

                    b.Property<int>("MP");

                    b.Property<int>("MaxHP");

                    b.Property<int>("MaxMP");

                    b.Property<int>("Money");

                    b.Property<string>("Name")
                        .HasMaxLength(13);

                    b.Property<short>("POP");

                    b.Property<int>("PlayTime");

                    b.Property<short>("SP");

                    b.Property<short>("STR");

                    b.Property<byte>("Skin");

                    b.Property<short>("SubJob");

                    b.Property<int>("TempEXP");

                    b.HasKey("ID");

                    b.HasIndex("DataID");

                    b.ToTable("Characters");
                });

            modelBuilder.Entity("Edelstein.Data.Entities.Inventory.ItemInventory", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("CharacterID");

                    b.Property<byte>("SlotMax");

                    b.Property<int>("Type");

                    b.HasKey("ID");

                    b.HasIndex("CharacterID");

                    b.ToTable("ItemInventory");
                });

            modelBuilder.Entity("Edelstein.Data.Entities.Inventory.ItemLocker", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<short>("SlotMax");

                    b.HasKey("ID");

                    b.ToTable("ItemLocker");
                });

            modelBuilder.Entity("Edelstein.Data.Entities.Inventory.ItemSlot", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<long?>("CashItemSN");

                    b.Property<DateTime?>("DateExpire");

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<int?>("ItemInventoryID");

                    b.Property<int?>("ItemLockerID");

                    b.Property<int?>("ItemTrunkID");

                    b.Property<short>("Position");

                    b.Property<int>("TemplateID");

                    b.HasKey("ID");

                    b.HasIndex("ItemInventoryID");

                    b.HasIndex("ItemLockerID");

                    b.HasIndex("ItemTrunkID");

                    b.ToTable("ItemSlot");

                    b.HasDiscriminator<string>("Discriminator").HasValue("ItemSlot");
                });

            modelBuilder.Entity("Edelstein.Data.Entities.Inventory.ItemTrunk", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Money");

                    b.Property<byte>("SlotMax");

                    b.HasKey("ID");

                    b.ToTable("ItemTrunk");
                });

            modelBuilder.Entity("Edelstein.Data.Entities.WishList", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("CharacterID");

                    b.Property<int>("SN");

                    b.HasKey("ID");

                    b.HasIndex("CharacterID");

                    b.ToTable("WishList");
                });

            modelBuilder.Entity("Edelstein.Data.Entities.Inventory.ItemSlotBundle", b =>
                {
                    b.HasBaseType("Edelstein.Data.Entities.Inventory.ItemSlot");

                    b.Property<short>("Attribute");

                    b.Property<short>("MaxNumber");

                    b.Property<short>("Number");

                    b.Property<string>("Title")
                        .HasMaxLength(13);

                    b.HasDiscriminator().HasValue("ItemSlotBundle");
                });

            modelBuilder.Entity("Edelstein.Data.Entities.Inventory.ItemSlotEquip", b =>
                {
                    b.HasBaseType("Edelstein.Data.Entities.Inventory.ItemSlot");

                    b.Property<short>("ACC");

                    b.Property<short>("Attribute")
                        .HasColumnName("ItemSlotEquip_Attribute");

                    b.Property<byte>("CHUC");

                    b.Property<byte>("CUC");

                    b.Property<short>("Craft");

                    b.Property<short>("DEX");

                    b.Property<int>("Durability");

                    b.Property<short>("EVA");

                    b.Property<int>("EXP");

                    b.Property<byte>("Grade");

                    b.Property<short>("INT");

                    b.Property<int>("IUC");

                    b.Property<short>("Jump");

                    b.Property<short>("LUK");

                    b.Property<byte>("Level");

                    b.Property<byte>("LevelUpType");

                    b.Property<short>("MAD");

                    b.Property<short>("MDD");

                    b.Property<short>("MaxHP");

                    b.Property<short>("MaxMP");

                    b.Property<short>("Option1");

                    b.Property<short>("Option2");

                    b.Property<short>("Option3");

                    b.Property<short>("PAD");

                    b.Property<short>("PDD");

                    b.Property<byte>("RUC");

                    b.Property<short>("STR");

                    b.Property<short>("Socket1");

                    b.Property<short>("Socket2");

                    b.Property<short>("Speed");

                    b.Property<string>("Title")
                        .HasColumnName("ItemSlotEquip_Title");

                    b.HasDiscriminator().HasValue("ItemSlotEquip");
                });

            modelBuilder.Entity("Edelstein.Data.Entities.Inventory.ItemSlotPet", b =>
                {
                    b.HasBaseType("Edelstein.Data.Entities.Inventory.ItemSlot");

                    b.Property<short>("Attribute")
                        .HasColumnName("ItemSlotPet_Attribute");

                    b.Property<DateTime?>("DateDead");

                    b.Property<byte>("Level")
                        .HasColumnName("ItemSlotPet_Level");

                    b.Property<short>("PetAttribute");

                    b.Property<string>("PetName");

                    b.Property<short>("PetSkill");

                    b.Property<int>("RemainLife");

                    b.Property<byte>("Repleteness");

                    b.Property<short>("Tameness");

                    b.HasDiscriminator().HasValue("ItemSlotPet");
                });

            modelBuilder.Entity("Edelstein.Data.Entities.AccountData", b =>
                {
                    b.HasOne("Edelstein.Data.Entities.Account", "Account")
                        .WithMany("Data")
                        .HasForeignKey("AccountID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Edelstein.Data.Entities.Inventory.ItemLocker", "Locker")
                        .WithMany()
                        .HasForeignKey("LockerID");

                    b.HasOne("Edelstein.Data.Entities.Inventory.ItemTrunk", "Trunk")
                        .WithMany()
                        .HasForeignKey("TrunkID");
                });

            modelBuilder.Entity("Edelstein.Data.Entities.Character", b =>
                {
                    b.HasOne("Edelstein.Data.Entities.AccountData", "Data")
                        .WithMany("Characters")
                        .HasForeignKey("DataID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Edelstein.Data.Entities.Inventory.ItemInventory", b =>
                {
                    b.HasOne("Edelstein.Data.Entities.Character")
                        .WithMany("Inventories")
                        .HasForeignKey("CharacterID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Edelstein.Data.Entities.Inventory.ItemSlot", b =>
                {
                    b.HasOne("Edelstein.Data.Entities.Inventory.ItemInventory", "ItemInventory")
                        .WithMany("Items")
                        .HasForeignKey("ItemInventoryID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Edelstein.Data.Entities.Inventory.ItemLocker", "ItemLocker")
                        .WithMany("Items")
                        .HasForeignKey("ItemLockerID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Edelstein.Data.Entities.Inventory.ItemTrunk", "ItemTrunk")
                        .WithMany("Items")
                        .HasForeignKey("ItemTrunkID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Edelstein.Data.Entities.WishList", b =>
                {
                    b.HasOne("Edelstein.Data.Entities.Character")
                        .WithMany("WishList")
                        .HasForeignKey("CharacterID");
                });
#pragma warning restore 612, 618
        }
    }
}
