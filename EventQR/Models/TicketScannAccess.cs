using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EventQR.Models
{
    public class QrScannAccount
    {
        [Key]
        public Guid UniqueId { get; set; }
        public Guid ?EventId { get; set; }

        [ForeignKey("EventId")]
        public Event? Event { get; set; }

        public Guid LoggedInUserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Address { get; set; } = string.Empty;
        public string MobileNo1 { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public string? Email { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }
    }
}
