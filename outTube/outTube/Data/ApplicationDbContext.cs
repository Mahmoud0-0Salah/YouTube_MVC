using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using outTube.Models;
using outTube.Models.JunctionTables;

namespace outTube.Data;

public class ApplicationDbContext : IdentityDbContext<User>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<Video> Videos { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Report> Reports { get; set; }
    public DbSet<LikeVideo> LikeVideos { get; set; }
    public DbSet<WatchVideo> WatchVideos { get; set; }
    public DbSet<UserCreateComment> UserCreateComments { get; set; }
    public DbSet<UserCreateReport> UserCreateReports { get; set; }
    public DbSet<ReviewReport> ReviewReports { get; set; }
    public DbSet<UserSubscriber> UserSubscribers { get; set; }
    public DbSet<BannedUser> BannedUsers { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // ============ GLOBAL QUERY FILTERS ============
        builder.Entity<Video>().HasQueryFilter(v => v.Visible && !v.User.BansReceived.Any());

        // ============ UNIQUE INDEXES FOR JUNCTION TABLES ============

        // LikeVideo: (VideoId, UserId) should be unique
        builder.Entity<LikeVideo>()
            .HasIndex(lv => new { lv.VideoId, lv.UserId })
            .IsUnique();

        // WatchVideo: (VideoId, UserId) should not be unique to allow repeat views
        builder.Entity<WatchVideo>()
            .HasIndex(wv => new { wv.VideoId, wv.UserId });

        // UserSubscriber: (UserId, SubscriberId) should be unique
        builder.Entity<UserSubscriber>()
            .HasIndex(us => new { us.UserId, us.SubscriberId })
            .IsUnique();

        // BannedUser: (UserId, AdminId) should be unique
        builder.Entity<BannedUser>()
            .HasIndex(bu => new { bu.UserId, bu.AdminId })
            .IsUnique();

        // ============ RELATIONSHIPS ============

        // Video -> User
        builder.Entity<Video>()
            .HasOne(v => v.User)
            .WithMany(u => u.Videos)
            .HasForeignKey(v => v.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // LikeVideo -> Video
        builder.Entity<LikeVideo>()
            .HasOne(lv => lv.Video)
            .WithMany(v => v.Likes)
            .HasForeignKey(lv => lv.VideoId)
            .OnDelete(DeleteBehavior.Cascade);

        // LikeVideo -> User
        builder.Entity<LikeVideo>()
            .HasOne(lv => lv.User)
            .WithMany(u => u.LikedVideos)
            .HasForeignKey(lv => lv.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // WatchVideo -> Video
        builder.Entity<WatchVideo>()
            .HasOne(wv => wv.Video)
            .WithMany(v => v.Views)
            .HasForeignKey(wv => wv.VideoId)
            .OnDelete(DeleteBehavior.Cascade);

        // WatchVideo -> User
        builder.Entity<WatchVideo>()
            .HasOne(wv => wv.User)
            .WithMany(u => u.WatchedVideos)
            .HasForeignKey(wv => wv.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // UserCreateComment -> Comment (1:1)
        builder.Entity<UserCreateComment>()
            .HasOne(uc => uc.Comment)
            .WithOne(c => c.UserCreateComment)
            .HasForeignKey<UserCreateComment>(uc => uc.CommentId)
            .OnDelete(DeleteBehavior.Cascade);

        // UserCreateComment -> Video
        builder.Entity<UserCreateComment>()
            .HasOne(uc => uc.Video)
            .WithMany(v => v.Comments)
            .HasForeignKey(uc => uc.VideoId)
            .OnDelete(DeleteBehavior.Cascade);

        // UserCreateComment -> User
        builder.Entity<UserCreateComment>()
            .HasOne(uc => uc.User)
            .WithMany(u => u.Comments)
            .HasForeignKey(uc => uc.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // UserCreateReport -> Report (1:1)
        builder.Entity<UserCreateReport>()
            .HasOne(ur => ur.Report)
            .WithOne(r => r.UserCreateReport)
            .HasForeignKey<UserCreateReport>(ur => ur.ReportId)
            .OnDelete(DeleteBehavior.Cascade);

        // UserCreateReport -> Video
        builder.Entity<UserCreateReport>()
            .HasOne(ur => ur.Video)
            .WithMany(v => v.Reports)
            .HasForeignKey(ur => ur.VideoId)
            .OnDelete(DeleteBehavior.Cascade);

        // UserCreateReport -> User
        builder.Entity<UserCreateReport>()
            .HasOne(ur => ur.User)
            .WithMany(u => u.Reports)
            .HasForeignKey(ur => ur.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // ReviewReport -> Report (1:1)
        builder.Entity<ReviewReport>()
            .HasOne(rr => rr.Report)
            .WithOne(r => r.ReviewReport)
            .HasForeignKey<ReviewReport>(rr => rr.ReportId)
            .OnDelete(DeleteBehavior.Cascade);

        // ReviewReport -> Admin (User)
        builder.Entity<ReviewReport>()
            .HasOne(rr => rr.Admin)
            .WithMany(u => u.ReviewedReports)
            .HasForeignKey(rr => rr.AdminId)
            .OnDelete(DeleteBehavior.Restrict);

        // UserSubscriber -> User (channel owner)
        builder.Entity<UserSubscriber>()
            .HasOne(us => us.User)
            .WithMany(u => u.Subscribers)
            .HasForeignKey(us => us.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // UserSubscriber -> Subscriber (the follower)
        builder.Entity<UserSubscriber>()
            .HasOne(us => us.Subscriber)
            .WithMany(u => u.Subscriptions)
            .HasForeignKey(us => us.SubscriberId)
            .OnDelete(DeleteBehavior.Restrict);

        // BannedUser -> User (banned user)
        builder.Entity<BannedUser>()
            .HasOne(bu => bu.User)
            .WithMany(u => u.BansReceived)
            .HasForeignKey(bu => bu.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // BannedUser -> Admin (who banned)
        builder.Entity<BannedUser>()
            .HasOne(bu => bu.Admin)
            .WithMany(u => u.BansIssued)
            .HasForeignKey(bu => bu.AdminId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}