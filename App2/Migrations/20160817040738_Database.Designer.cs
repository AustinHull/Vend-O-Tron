﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using App2;

namespace App2.Migrations
{
    [DbContext(typeof(VendingInfoContext))]
    [Migration("20160817040738_Database")]
    partial class Database
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431");

            modelBuilder.Entity("App2.Machine", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("machineLocation");

                    b.Property<string>("machineName");

                    b.HasKey("Id");

                    b.ToTable("Machines");
                });
        }
    }
}
