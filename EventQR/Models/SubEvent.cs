using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace EventQR.Models
{
    public class SubEvent
    {
        [Key]
        public Guid UniqueId { get; set; }
        public Guid? EventId { get; set; }
        [ForeignKey("EventId")]
        public Event? Event { get; set; } = null;

        public string SubEventName { get; set; }
        [DisplayName("Start Date Time")]
        public DateTime? StartDateTime { get; set; }
        [DisplayName("Start Date Time")]
        public DateTime? EndDateTime { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
    }
}
