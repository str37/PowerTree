﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PowerTree.Maui;

#nullable disable

namespace PowerTree.Maui.Migrations
{
    [DbContext(typeof(PTContext))]
    partial class PTContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("PowerTree.Maui.Model.PTHierarchy", b =>
                {
                    b.Property<int>("HierarchyId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("HierarchyId"), 10L);

                    b.Property<string>("HierarchyName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Subsystem")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("HierarchyId");

                    b.HasIndex("Subsystem", "HierarchyName")
                        .IsUnique();

                    b.ToTable("pthierarchy");
                });

            modelBuilder.Entity("PowerTree.Maui.Model.PTNode", b =>
                {
                    b.Property<int>("NodeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("NodeId"), 10000L);

                    b.Property<int>("HierarchyId")
                        .HasColumnType("int");

                    b.Property<string>("NodeName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("NodeOrder")
                        .HasColumnType("int");

                    b.Property<int?>("ParentNodeId")
                        .HasColumnType("int");

                    b.HasKey("NodeId");

                    b.HasIndex("HierarchyId");

                    b.HasIndex("ParentNodeId");

                    b.ToTable("ptnode");
                });

            modelBuilder.Entity("PowerTree.Maui.Model.PTNodeItem", b =>
                {
                    b.Property<int>("NodeItemId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("NodeItemId"));

                    b.Property<int>("EntityId")
                        .HasColumnType("int");

                    b.Property<int>("NodeId")
                        .HasColumnType("int");

                    b.Property<byte[]>("NodeImage")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("NodeItemName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Order")
                        .HasColumnType("int");

                    b.HasKey("NodeItemId");

                    b.HasIndex("NodeId");

                    b.ToTable("ptnodeitem");
                });

            modelBuilder.Entity("PowerTree.Maui.Model.PTNode", b =>
                {
                    b.HasOne("PowerTree.Maui.Model.PTHierarchy", "Hierarchy")
                        .WithMany()
                        .HasForeignKey("HierarchyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PowerTree.Maui.Model.PTNode", "ParentNode")
                        .WithMany()
                        .HasForeignKey("ParentNodeId");

                    b.Navigation("Hierarchy");

                    b.Navigation("ParentNode");
                });

            modelBuilder.Entity("PowerTree.Maui.Model.PTNodeItem", b =>
                {
                    b.HasOne("PowerTree.Maui.Model.PTNode", "Node")
                        .WithMany("NodeItems")
                        .HasForeignKey("NodeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Node");
                });

            modelBuilder.Entity("PowerTree.Maui.Model.PTNode", b =>
                {
                    b.Navigation("NodeItems");
                });
#pragma warning restore 612, 618
        }
    }
}
