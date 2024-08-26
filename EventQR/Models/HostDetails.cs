using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Text;

namespace EventQR.Models
{
    public class HostDetails
    {
        [Key]
        public Guid UniqueId { get; set; }
        public Guid? EventId { get; set; }
        [ForeignKey("EventId")]   
        public string? HostOne {  get; set; }
        public string? HostTwo { get; set; }
        public string? ContactNumberOne {  get; set; }
        public string? ContactNumberTwo { get; set; }
        public string? HostOneDesignation { get; set; }
        public string? HostTwoDesignation { get; set; }
       
    }
}
