﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SSW.Rewards.Infrastructure.Persistence;

#nullable disable

namespace SSW.Rewards.Persistence.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20221005011336_AddMultiscan")]
    partial class AddMultiscan
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("SSW.Rewards.Domain.Entities.Achievement", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

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

                    b.Property<bool>("IsMultiscanEnabled")
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
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("CreatedUtc")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Passed")
                        .HasColumnType("bit");

                    b.Property<int>("QuizId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("QuizId");

                    b.HasIndex("UserId");

                    b.ToTable("CompletedQuizzes");
                });

            modelBuilder.Entity("SSW.Rewards.Domain.Entities.Notification", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

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
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("BuildingNameOrNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Country")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedUtc")
                        .HasColumnType("datetime2");

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
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("AchievementId")
                        .HasColumnType("int");

                    b.Property<int?>("CreatedById")
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

                    b.HasIndex("CreatedById");

                    b.ToTable("Quizzes");
                });

            modelBuilder.Entity("SSW.Rewards.Domain.Entities.QuizAnswer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

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
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

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
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

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

                    b.Property<bool>("IsOnboardingReward")
                        .HasColumnType("bit");

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
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

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
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("CreatedUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Skills");
                });

            modelBuilder.Entity("SSW.Rewards.Domain.Entities.SocialMediaPlatform", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("AchievementId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AchievementId");

                    b.ToTable("SocialMediaPlatforms");
                });

            modelBuilder.Entity("SSW.Rewards.Domain.Entities.StaffMember", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

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
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

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

            modelBuilder.Entity("SSW.Rewards.Domain.Entities.SubmittedQuizAnswer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int?>("AnswerId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedUtc")
                        .HasColumnType("datetime2");

                    b.Property<int>("SubmissionId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AnswerId");

                    b.HasIndex("SubmissionId");

                    b.ToTable("SubmittedAnswers");
                });

            modelBuilder.Entity("SSW.Rewards.Domain.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

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
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

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
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

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
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

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

            modelBuilder.Entity("SSW.Rewards.Domain.Entities.UserSocialMediaId", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("CreatedUtc")
                        .HasColumnType("datetime2");

                    b.Property<int>("SocialMediaPlatformId")
                        .HasColumnType("int");

                    b.Property<string>("SocialMediaUserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SocialMediaPlatformId");

                    b.HasIndex("UserId");

                    b.ToTable("UserSocialMediaIds");
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

                    b.Navigation("Quiz");

                    b.Navigation("User");
                });

            modelBuilder.Entity("SSW.Rewards.Domain.Entities.Notification", b =>
                {
                    b.HasOne("SSW.Rewards.Domain.Entities.User", "SentByStaffMember")
                        .WithMany("SentNotifications")
                        .HasForeignKey("SentByStaffMemberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SentByStaffMember");
                });

            modelBuilder.Entity("SSW.Rewards.Domain.Entities.Quiz", b =>
                {
                    b.HasOne("SSW.Rewards.Domain.Entities.Achievement", "Achievement")
                        .WithMany("Quizzes")
                        .HasForeignKey("AchievementId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SSW.Rewards.Domain.Entities.User", "CreatedBy")
                        .WithMany("CreatedQuizzes")
                        .HasForeignKey("CreatedById")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("Achievement");

                    b.Navigation("CreatedBy");
                });

            modelBuilder.Entity("SSW.Rewards.Domain.Entities.QuizAnswer", b =>
                {
                    b.HasOne("SSW.Rewards.Domain.Entities.QuizQuestion", "Question")
                        .WithMany("Answers")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Question");
                });

            modelBuilder.Entity("SSW.Rewards.Domain.Entities.QuizQuestion", b =>
                {
                    b.HasOne("SSW.Rewards.Domain.Entities.Quiz", "Quiz")
                        .WithMany("Questions")
                        .HasForeignKey("QuizId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Quiz");
                });

            modelBuilder.Entity("SSW.Rewards.Domain.Entities.SocialMediaPlatform", b =>
                {
                    b.HasOne("SSW.Rewards.Domain.Entities.Achievement", "Achievement")
                        .WithMany("SocialMediaPlatforms")
                        .HasForeignKey("AchievementId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Achievement");
                });

            modelBuilder.Entity("SSW.Rewards.Domain.Entities.StaffMember", b =>
                {
                    b.HasOne("SSW.Rewards.Domain.Entities.Achievement", "StaffAchievement")
                        .WithMany()
                        .HasForeignKey("StaffAchievementId");

                    b.Navigation("StaffAchievement");
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

                    b.Navigation("Skill");

                    b.Navigation("StaffMember");
                });

            modelBuilder.Entity("SSW.Rewards.Domain.Entities.SubmittedQuizAnswer", b =>
                {
                    b.HasOne("SSW.Rewards.Domain.Entities.QuizAnswer", "Answer")
                        .WithMany("SubmittedAnswers")
                        .HasForeignKey("AnswerId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("SSW.Rewards.Domain.Entities.CompletedQuiz", "Submission")
                        .WithMany("Answers")
                        .HasForeignKey("SubmissionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Answer");

                    b.Navigation("Submission");
                });

            modelBuilder.Entity("SSW.Rewards.Domain.Entities.User", b =>
                {
                    b.HasOne("SSW.Rewards.Domain.Entities.PostalAddress", "Address")
                        .WithMany()
                        .HasForeignKey("AddressId");

                    b.Navigation("Address");
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

                    b.Navigation("Achievement");

                    b.Navigation("User");
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

                    b.Navigation("Reward");

                    b.Navigation("User");
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

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("SSW.Rewards.Domain.Entities.UserSocialMediaId", b =>
                {
                    b.HasOne("SSW.Rewards.Domain.Entities.SocialMediaPlatform", "SocialMediaPlatform")
                        .WithMany("UserSocialMediaIds")
                        .HasForeignKey("SocialMediaPlatformId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SSW.Rewards.Domain.Entities.User", "User")
                        .WithMany("SocialMediaIds")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SocialMediaPlatform");

                    b.Navigation("User");
                });

            modelBuilder.Entity("SSW.Rewards.Domain.Entities.Achievement", b =>
                {
                    b.Navigation("Quizzes");

                    b.Navigation("SocialMediaPlatforms");

                    b.Navigation("UserAchievements");
                });

            modelBuilder.Entity("SSW.Rewards.Domain.Entities.CompletedQuiz", b =>
                {
                    b.Navigation("Answers");
                });

            modelBuilder.Entity("SSW.Rewards.Domain.Entities.Quiz", b =>
                {
                    b.Navigation("CompletedQuizzes");

                    b.Navigation("Questions");
                });

            modelBuilder.Entity("SSW.Rewards.Domain.Entities.QuizAnswer", b =>
                {
                    b.Navigation("SubmittedAnswers");
                });

            modelBuilder.Entity("SSW.Rewards.Domain.Entities.QuizQuestion", b =>
                {
                    b.Navigation("Answers");
                });

            modelBuilder.Entity("SSW.Rewards.Domain.Entities.Reward", b =>
                {
                    b.Navigation("UserRewards");
                });

            modelBuilder.Entity("SSW.Rewards.Domain.Entities.Role", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("SSW.Rewards.Domain.Entities.SocialMediaPlatform", b =>
                {
                    b.Navigation("UserSocialMediaIds");
                });

            modelBuilder.Entity("SSW.Rewards.Domain.Entities.StaffMember", b =>
                {
                    b.Navigation("StaffMemberSkills");
                });

            modelBuilder.Entity("SSW.Rewards.Domain.Entities.User", b =>
                {
                    b.Navigation("CompletedQuizzes");

                    b.Navigation("CreatedQuizzes");

                    b.Navigation("Roles");

                    b.Navigation("SentNotifications");

                    b.Navigation("SocialMediaIds");

                    b.Navigation("UserAchievements");

                    b.Navigation("UserRewards");
                });
#pragma warning restore 612, 618
        }
    }
}
