using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace EventQR.Models
{
    public class Organizer
    {
        [Key]
        public Guid UniqueId { get; set; }
        public Guid OrganizerUserId { get; set; }
        [Required]
        [DisplayName("Organization Name")]
        public string OrganizationName { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;
        [Required]
        public string Phone1 { get; set; } = string.Empty;
        public string Phone2 { get; set; } = string.Empty;
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        public string Website { get; set; } = string.Empty;
        public string OfficeAddress { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public string LogoImageName { get; internal set; }
        public string ProfileImageName { get; internal set; }
    }
}
