using Microsoft.AspNetCore.Identity;
using outTube.Models.Enums;
using outTube.Models.JunctionTables;
using System.ComponentModel.DataAnnotations;

namespace outTube.Models;

public class User : IdentityUser
{
    [MaxLength(255)]
    public string FirstName { get; set; }

    [MaxLength(255)]
    public string LastName { get; set; }

    public Gender? Gender { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [MaxLength(255)]
    public string? ImageUrl { get; set; }


    public ICollection<Video> Videos { get; set; } = new List<Video>();
    public ICollection<LikeVideo> LikedVideos { get; set; } = new List<LikeVideo>();
    public ICollection<WatchVideo> WatchedVideos { get; set; } = new List<WatchVideo>();
    public ICollection<UserCreateComment> Comments { get; set; } = new List<UserCreateComment>();
    public ICollection<UserCreateReport> Reports { get; set; } = new List<UserCreateReport>();

    // For admins to review reports
    public ICollection<ReviewReport> ReviewedReports { get; set; } = new List<ReviewReport>();

    // Subscriptions
    public ICollection<UserSubscriber> Subscribers { get; set; } = new List<UserSubscriber>();
    public ICollection<UserSubscriber> Subscriptions { get; set; } = new List<UserSubscriber>();

    // Bans
    public ICollection<BannedUser> BansReceived { get; set; } = new List<BannedUser>();
    public ICollection<BannedUser> BansIssued { get; set; } = new List<BannedUser>();
}
