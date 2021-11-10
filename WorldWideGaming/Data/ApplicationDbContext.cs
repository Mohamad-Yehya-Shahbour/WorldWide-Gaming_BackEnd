using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using WorldWideGaming.Models;

#nullable disable

namespace WorldWideGaming.Data
{
    public partial class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Event> Events { get; set; }
        public virtual DbSet<EventsJoiner> EventsJoiners { get; set; }
        public virtual DbSet<Friend> Friends { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<GroupPost> GroupPosts { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<PostsComment> PostsComments { get; set; }
        public virtual DbSet<PostsLike> PostsLikes { get; set; }
        public virtual DbSet<Team> Teams { get; set; }
        public virtual DbSet<TeamsAchievement> TeamsAchievements { get; set; }
        public virtual DbSet<TeamsFollower> TeamsFollowers { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UsersGroup> UsersGroups { get; set; }
        public virtual DbSet<UsersTeam> UsersTeams { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=DESKTOP-L0L2QA4\\PROJECTSQL;database=WorldWideGamingDB;trusted_connection=true;MultipleActiveResultSets=true;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Event>(entity =>
            {
                entity.HasOne(d => d.User)
                    .WithMany(p => p.Events)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_events_users");
            });

            modelBuilder.Entity<EventsJoiner>(entity =>
            {
                entity.HasOne(d => d.Event)
                    .WithMany(p => p.EventsJoiners)
                    .HasForeignKey(d => d.EventId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_events_joiners_events");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.EventsJoiners)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_events_joiners_users");
            });

            modelBuilder.Entity<Friend>(entity =>
            {
                entity.HasOne(d => d.UserOne)
                    .WithMany(p => p.FriendUserOnes)
                    .HasForeignKey(d => d.UserOneId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_friends_users");

                entity.HasOne(d => d.UserTwo)
                    .WithMany(p => p.FriendUserTwos)
                    .HasForeignKey(d => d.UserTwoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_friends_users1");
            });

            modelBuilder.Entity<Group>(entity =>
            {
                entity.HasOne(d => d.User)
                    .WithMany(p => p.Groups)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_groups_users");
            });

            modelBuilder.Entity<GroupPost>(entity =>
            {
                entity.HasOne(d => d.Group)
                    .WithMany(p => p.GroupPosts)
                    .HasForeignKey(d => d.GroupId)
                    .HasConstraintName("FK_group_posts_groups");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.GroupPosts)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_group_posts_users");
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.HasOne(d => d.User1)
                    .WithMany(p => p.NotificationUser1s)
                    .HasForeignKey(d => d.User1Id)
                    .HasConstraintName("FK_notifications_users");

                entity.HasOne(d => d.User2)
                    .WithMany(p => p.NotificationUser2s)
                    .HasForeignKey(d => d.User2Id)
                    .HasConstraintName("FK_notifications_users1");
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.HasOne(d => d.User)
                    .WithMany(p => p.Posts)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_clips_users");
            });

            modelBuilder.Entity<PostsComment>(entity =>
            {
                entity.HasOne(d => d.Post)
                    .WithMany(p => p.PostsComments)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_posts_comments_posts1");

                entity.HasOne(d => d.UserWhoComments)
                    .WithMany(p => p.PostsComments)
                    .HasForeignKey(d => d.UserWhoCommentsId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_clips_comments_users");
            });

            modelBuilder.Entity<PostsLike>(entity =>
            {
                entity.HasOne(d => d.Post)
                    .WithMany(p => p.PostsLikes)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_posts_likes_posts");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.PostsLikes)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_posts_likes_users");
            });

            modelBuilder.Entity<Team>(entity =>
            {
                entity.HasOne(d => d.Achievements)
                    .WithMany(p => p.Teams)
                    .HasForeignKey(d => d.AchievementsId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_teams_teams_achievements");

                entity.HasOne(d => d.Admin)
                    .WithMany(p => p.Teams)
                    .HasForeignKey(d => d.AdminId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_teams_users");
            });

            modelBuilder.Entity<TeamsFollower>(entity =>
            {
                entity.HasOne(d => d.Team)
                    .WithMany(p => p.TeamsFollowers)
                    .HasForeignKey(d => d.TeamId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_teams_followers_teams");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TeamsFollowers)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_teams_followers_users");
            });

            modelBuilder.Entity<UsersGroup>(entity =>
            {
                entity.HasOne(d => d.Group)
                    .WithMany(p => p.UsersGroups)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_users_groups_groups");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UsersGroups)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_users_groups_users");
            });

            modelBuilder.Entity<UsersTeam>(entity =>
            {
                entity.HasOne(d => d.Team)
                    .WithMany(p => p.UsersTeams)
                    .HasForeignKey(d => d.TeamId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_users_teams_teams");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UsersTeams)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_users_teams_users");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
