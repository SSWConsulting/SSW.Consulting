﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SSW.Rewards.Persistence;

namespace SSW.Rewards.Persistence.Migrations
{
    [DbContext(typeof(SSWRewardsDbContext))]
    [Migration("20220720235130_AddQuizzes")]
    partial class AddQuizzes
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("SSW.Rewards.Domain.Entities.Achievement", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Code")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedUtc")
                        .HasColumnType("datetime2");

                    b.Property<int>("Icon")
                        .HasColumnType("int");

                    b.Property<bool>("IconIsBranded")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<int>("Value")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Achievements");
                });

            modelBuilder.Entity("SSW.Rewards.Domain.Entities.CompletedQuiz", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedUtc")
                        .HasColumnType("datetime2");

                    b.Property<int>("QuizId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("QuizId");

                    b.HasIndex("UserId");

                    b.ToTable("CompletedQuizzes");
                });

            modelBuilder.Entity("SSW.Rewards.Domain.Entities.Notifications", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("Message")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NotificationAction")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NotificationTag")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SentByStaffMemberId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SentByStaffMemberId");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("SSW.Rewards.Domain.Entities.PostalAddress", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("BuildingNameOrNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Country")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Municipality")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PostalCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("State")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StreetName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("SSW.Rewards.Domain.Entities.Quiz", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AchievementId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Icon")
                        .HasColumnType("int");

                    b.Property<bool>("IsArchived")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastUpdatedUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AchievementId");

                    b.ToTable("Quizzes");
                });

            modelBuilder.Entity("SSW.Rewards.Domain.Entities.QuizAnswer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedUtc")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsCorrect")
                        .HasColumnType("bit");

                    b.Property<int>("QuestionId")
                        .HasColumnType("int");

                    b.Property<string>("Text")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("QuestionId");

                    b.ToTable("QuizAnswers");
                });

            modelBuilder.Entity("SSW.Rewards.Domain.Entities.QuizQuestion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedUtc")
                        .HasColumnType("datetime2");

                    b.Property<int>("QuizId")
                        .HasColumnType("int");

                    b.Property<string>("Text")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("QuizId");

                    b.ToTable("QuizQuestions");
                });

            modelBuilder.Entity("SSW.Rewards.Domain.Entities.Reward", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Code")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Cost")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedUtc")
                        .HasColumnType("datetime2");

                    b.Property<int>("Icon")
                        .HasColumnType("int");

                    b.Property<bool>("IconIsBranded")
                        .HasColumnType("bit");

                    b.Property<string>("ImageUri")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RewardType")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Rewards");
                });

            modelBuilder.Entity("SSW.Rewards.Domain.Entities.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("SSW.Rewards.Domain.Entities.Skill", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Skills");
                });

            modelBuilder.Entity("SSW.Rewards.Domain.Entities.StaffMember", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("GitHubUsername")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsExternal")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<string>("LinkedInUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Profile")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProfilePhoto")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("StaffAchievementId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TwitterUsername")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("StaffAchievementId");

                    b.ToTable("StaffMembers");
                });

            modelBuilder.Entity("SSW.Rewards.Domain.Entities.StaffMemberSkill", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedUtc")
                        .HasColumnType("datetime2");

                    b.Property<int>("Level")
                        .HasColumnType("int");

                    b.Property<int>("SkillId")
                        .HasColumnType("int");

                    b.Property<int>("StaffMemberId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("StaffMemberId");

                    b.HasIndex("SkillId", "StaffMemberId")
                        .IsUnique();

                    b.ToTable("StaffMemberSkills");
                });

            modelBuilder.Entity("SSW.Rewards.Domain.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Activated")
                        .HasColumnType("bit");

                    b.Property<int?>("AddressId")
                        .HasColumnType("int");

                    b.Property<string>("Avatar")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("FullName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AddressId");

                    b.HasIndex("Email");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("SSW.Rewards.Domain.Entities.UserAchievement", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AchievementId")
                        .HasColumnType("int");

                    b.Property<DateTime>("AwardedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getdate()");

                    b.Property<DateTime>("CreatedUtc")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AchievementId");

                    b.HasIndex("UserId", "AchievementId")
                        .IsUnique();

                    b.ToTable("UserAchievements");
                });

            modelBuilder.Entity("SSW.Rewards.Domain.Entities.UserReward", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("AwardedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getdate()");

                    b.Property<DateTime>("CreatedUtc")
                        .HasColumnType("datetime2");

                    b.Property<int>("RewardId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RewardId");

                    b.HasIndex("UserId");

                    b.ToTable("UserRewards");
                });

            modelBuilder.Entity("SSW.Rewards.Domain.Entities.UserRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedUtc")
                        .HasColumnType("datetime2");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserId");

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("SSW.Rewards.Domain.Entities.CompletedQuiz", b =>
                {
                    b.HasOne("SSW.Rewards.Domain.Entities.Quiz", "Quiz")
                        .WithMany("CompletedQuizzes")
                        .HasForeignKey("QuizId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SSW.Rewards.Domain.Entities.User", "User")
                        .WithMany("CompletedQuizzes")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SSW.Rewards.Domain.Entities.Notifications", b =>
                {
                    b.HasOne("SSW.Rewards.Domain.Entities.User", "SentByStaffMember")
                        .WithMany("SentNotifications")
                        .HasForeignKey("SentByStaffMemberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SSW.Rewards.Domain.Entities.Quiz", b =>
                {
                    b.HasOne("SSW.Rewards.Domain.Entities.Achievement", "Achievement")
                        .WithMany("Quizzes")
                        .HasForeignKey("AchievementId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SSW.Rewards.Domain.Entities.QuizAnswer", b =>
                {
                    b.HasOne("SSW.Rewards.Domain.Entities.QuizQuestion", "Question")
                        .WithMany("Answers")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SSW.Rewards.Domain.Entities.QuizQuestion", b =>
                {
                    b.HasOne("SSW.Rewards.Domain.Entities.Quiz", "Quiz")
                        .WithMany("Questions")
                        .HasForeignKey("QuizId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SSW.Rewards.Domain.Entities.StaffMember", b =>
                {
                    b.HasOne("SSW.Rewards.Domain.Entities.Achievement", "StaffAchievement")
                        .WithMany()
                        .HasForeignKey("StaffAchievementId");
                });

            modelBuilder.Entity("SSW.Rewards.Domain.Entities.StaffMemberSkill", b =>
                {
                    b.HasOne("SSW.Rewards.Domain.Entities.Skill", "Skill")
                        .WithMany()
                        .HasForeignKey("SkillId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SSW.Rewards.Domain.Entities.StaffMember", "StaffMember")
                        .WithMany("StaffMemberSkills")
                        .HasForeignKey("StaffMemberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SSW.Rewards.Domain.Entities.User", b =>
                {
                    b.HasOne("SSW.Rewards.Domain.Entities.PostalAddress", "Address")
                        .WithMany()
                        .HasForeignKey("AddressId");
                });

            modelBuilder.Entity("SSW.Rewards.Domain.Entities.UserAchievement", b =>
                {
                    b.HasOne("SSW.Rewards.Domain.Entities.Achievement", "Achievement")
                        .WithMany("UserAchievements")
                        .HasForeignKey("AchievementId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SSW.Rewards.Domain.Entities.User", "User")
                        .WithMany("UserAchievements")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SSW.Rewards.Domain.Entities.UserReward", b =>
                {
                    b.HasOne("SSW.Rewards.Domain.Entities.Reward", "Reward")
                        .WithMany("UserRewards")
                        .HasForeignKey("RewardId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SSW.Rewards.Domain.Entities.User", "User")
                        .WithMany("UserRewards")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SSW.Rewards.Domain.Entities.UserRole", b =>
                {
                    b.HasOne("SSW.Rewards.Domain.Entities.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SSW.Rewards.Domain.Entities.User", "User")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
