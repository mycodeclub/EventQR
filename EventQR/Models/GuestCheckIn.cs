using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventQR.Models
{
    public class GuestCheckIn
    {
        [Key]
        public int UniqueId { get; set; }

        public Guid? EventId { get; set; }
        [ForeignKey("EventId")]
        public Event? Event { get; set; }

        public Guid? SubEventId { get; set; }
        [ForeignKey("SubEventId")]
        public SubEvent? SubEvent { get; set; }



        public Guid? UserLoginId { get; set; }

        //  public int GuestCheckIn_Scanner { get; set; }
        public int ScannerLoginId { get; set; }
        [ForeignKey("ScannerLoginId")]
        public TicketScanner? Scanner { get; set; }


        public Guid? GuestId { get; set; }
        [ForeignKey("GuestId")]
        public EventGuest? Guest { get; set; }
        public DateTime CheckIn { get; set; }

    }
}
