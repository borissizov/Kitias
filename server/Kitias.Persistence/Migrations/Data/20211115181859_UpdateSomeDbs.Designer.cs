﻿// <auto-generated />
using System;
using Kitias.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Kitias.Persistence.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20211115181859_UpdateSomeDbs")]
    partial class UpdateSomeDbs
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.11")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("Kitias.Persistence.Entities.People.Person", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("FullName")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("text")
                        .HasComputedColumnSql("trim(\"Surname\" || ' ' || \"Name\" || ' ' || \"Patronymic\")", true);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<string>("Patronymic")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)")
                        .HasDefaultValue("");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.HasKey("Id");

                    b.HasAlternateKey("Email");

                    b.HasIndex("Email")
                        .IsUnique()
                        .HasDatabaseName("PersonEmailIndex")
                        .HasFilter("\"Email\" IS NOT NULL");

                    b.ToTable("Persons");
                });

            modelBuilder.Entity("Kitias.Persistence.Entities.People.Student", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("GroupId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("PersonId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.HasIndex("PersonId");

                    b.ToTable("Students");
                });

            modelBuilder.Entity("Kitias.Persistence.Entities.People.Teacher", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("PersonId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("PersonId");

                    b.ToTable("Teachers");
                });

            modelBuilder.Entity("Kitias.Persistence.Entities.Scheduler.Attendence.Attendance", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("Attended")
                        .HasColumnType("integer");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp without time zone");

                    b.Property<byte>("Score")
                        .HasColumnType("smallint");

                    b.Property<Guid?>("StudentAttendanceId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("StudentId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("SubjectId")
                        .HasColumnType("uuid");

                    b.Property<string>("Theme")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("StudentAttendanceId");

                    b.HasIndex("StudentId");

                    b.HasIndex("SubjectId");

                    b.ToTable("Attendances");
                });

            modelBuilder.Entity("Kitias.Persistence.Entities.Scheduler.Attendence.AttendanceSheduler", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("GroupId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("TeacherId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.HasIndex("TeacherId");

                    b.ToTable("AttendanceShedulers");
                });

            modelBuilder.Entity("Kitias.Persistence.Entities.Scheduler.Attendence.StudentAttendance", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("AttendanceShedulerId")
                        .HasColumnType("uuid");

                    b.Property<int>("Grade")
                        .HasColumnType("integer");

                    b.Property<byte>("Raiting")
                        .HasColumnType("smallint");

                    b.Property<Guid>("StudentId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("TeacherId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("AttendanceShedulerId");

                    b.HasIndex("StudentId");

                    b.HasIndex("TeacherId");

                    b.ToTable("StudentAttendances");
                });

            modelBuilder.Entity("Kitias.Persistence.Entities.Scheduler.Group", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<byte>("Course")
                        .HasColumnType("smallint");

                    b.Property<string>("EducationType")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<DateTime>("IssueDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Number")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("character varying(10)");

                    b.Property<DateTime>("ReceiptDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Speciality")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.HasKey("Id");

                    b.HasAlternateKey("Number");

                    b.HasIndex("Number")
                        .IsUnique()
                        .HasDatabaseName("GroupNameIndex")
                        .HasFilter("\"Number\" IS NOT NULL");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("Kitias.Persistence.Entities.Scheduler.Subject", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("Date")
                        .HasColumnType("date");

                    b.Property<string>("Day")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<TimeSpan>("Time")
                        .HasColumnType("time");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<string>("Week")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.HasKey("Id");

                    b.ToTable("Subjects");
                });

            modelBuilder.Entity("Kitias.Persistence.Entities.Scheduler.SubjectGroup", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("GroupId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("SubjectId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.HasIndex("SubjectId");

                    b.ToTable("SubjectsGroups");
                });

            modelBuilder.Entity("Kitias.Persistence.Entities.People.Student", b =>
                {
                    b.HasOne("Kitias.Persistence.Entities.Scheduler.Group", "Group")
                        .WithMany("Students")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Kitias.Persistence.Entities.People.Person", "Person")
                        .WithMany()
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Group");

                    b.Navigation("Person");
                });

            modelBuilder.Entity("Kitias.Persistence.Entities.People.Teacher", b =>
                {
                    b.HasOne("Kitias.Persistence.Entities.People.Person", "Person")
                        .WithMany()
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Person");
                });

            modelBuilder.Entity("Kitias.Persistence.Entities.Scheduler.Attendence.Attendance", b =>
                {
                    b.HasOne("Kitias.Persistence.Entities.Scheduler.Attendence.StudentAttendance", null)
                        .WithMany("Attendances")
                        .HasForeignKey("StudentAttendanceId");

                    b.HasOne("Kitias.Persistence.Entities.People.Student", "Student")
                        .WithMany()
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Kitias.Persistence.Entities.Scheduler.Subject", "Subject")
                        .WithMany()
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Student");

                    b.Navigation("Subject");
                });

            modelBuilder.Entity("Kitias.Persistence.Entities.Scheduler.Attendence.AttendanceSheduler", b =>
                {
                    b.HasOne("Kitias.Persistence.Entities.Scheduler.Group", "Group")
                        .WithMany()
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Kitias.Persistence.Entities.People.Teacher", "Teacher")
                        .WithMany()
                        .HasForeignKey("TeacherId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Group");

                    b.Navigation("Teacher");
                });

            modelBuilder.Entity("Kitias.Persistence.Entities.Scheduler.Attendence.StudentAttendance", b =>
                {
                    b.HasOne("Kitias.Persistence.Entities.Scheduler.Attendence.AttendanceSheduler", null)
                        .WithMany("StudentAttendances")
                        .HasForeignKey("AttendanceShedulerId");

                    b.HasOne("Kitias.Persistence.Entities.People.Student", "Student")
                        .WithMany()
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Kitias.Persistence.Entities.People.Teacher", "Teacher")
                        .WithMany()
                        .HasForeignKey("TeacherId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Student");

                    b.Navigation("Teacher");
                });

            modelBuilder.Entity("Kitias.Persistence.Entities.Scheduler.SubjectGroup", b =>
                {
                    b.HasOne("Kitias.Persistence.Entities.Scheduler.Group", "Group")
                        .WithMany("Groups")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Kitias.Persistence.Entities.Scheduler.Subject", "Subject")
                        .WithMany("Groups")
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Group");

                    b.Navigation("Subject");
                });

            modelBuilder.Entity("Kitias.Persistence.Entities.Scheduler.Attendence.AttendanceSheduler", b =>
                {
                    b.Navigation("StudentAttendances");
                });

            modelBuilder.Entity("Kitias.Persistence.Entities.Scheduler.Attendence.StudentAttendance", b =>
                {
                    b.Navigation("Attendances");
                });

            modelBuilder.Entity("Kitias.Persistence.Entities.Scheduler.Group", b =>
                {
                    b.Navigation("Groups");

                    b.Navigation("Students");
                });

            modelBuilder.Entity("Kitias.Persistence.Entities.Scheduler.Subject", b =>
                {
                    b.Navigation("Groups");
                });
#pragma warning restore 612, 618
        }
    }
}
