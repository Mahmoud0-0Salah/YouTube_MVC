using System.ComponentModel.DataAnnotations;

public class SubscribeViewModel
{
    [Required(ErrorMessage = "Channel user ID is required.")]
    public string UserId { get; set; }

    public string? ChannelName { get; set; }
    public int SubscriberCount { get; set; }
    public bool IsSubscribed { get; set; }
}