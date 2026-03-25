using Microsoft.AspNetCore.Http;
using outTube.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace outTube.ViewModels
{
    public class ProfileViewModel
    {
        public string? Id { get; set; }

        public string? UserName { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public Gender? Gender { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        [Display(Name = "Profile Image")]
        public string? ImageUrl { get; set; }

        [Display(Name = "Upload New Image")]
        public IFormFile? ImageFile { get; set; }
    }
}
