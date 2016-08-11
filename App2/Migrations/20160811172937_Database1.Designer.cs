using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using App2;

namespace App2.Migrations
{
    [DbContext(typeof(VendingInfoContext))]
    [Migration("20160811172937_Database1")]
    partial class Database1
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431");

            modelBuilder.Entity("App2.Machine", b =>
                {
                    b.Property<int>("machineID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("machineLocation");

                    b.Property<string>("machineName");

                    b.HasKey("machineID");

                    b.ToTable("Machine");
                });
        }
    }
}
