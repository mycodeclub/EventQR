using System.Text;

namespace EventQR.Common.Static
{
    public static class Variables
    {
        private static string HostUrl = "http://foodcoupon.bitprosofttech.com/";

        public static string OrgProfilePicsPath = $"/ApplicationDocs/Organizer/ProfilePics/";
        public static string OrgLogoPath = $"/ApplicationDocs/Organizer/Logos/";

        public static string GetMyTicketUri(Guid guestId, Guid eventId)
        {
            return $"/Admin/Guests/ShowMyTicket?guestId={guestId}&eventId={eventId}";
        }

        public static string GetQrCodeUriStr(Guid guestId, Guid eventId)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(HostUrl);
            sb.Append("/Scanner/CheckIn/AllowGuest?");
            var parameters= $"guestId={guestId}&eventId={eventId}";
            sb.Append(parameters);
            return sb.ToString();
        }

        public static string GenerateTicketKey(string guestId, string eventId) => guestId + "|" + eventId;
        public static string GetMerchantLogoUrl(string OrganizerId) => $"{OrgLogoPath.Replace("/", "\\")}\\{OrganizerId}";
        public static string GetMerchantProfilePicUrl(string OrganizerId) => $"{OrgProfilePicsPath.Replace("/", "\\")}\\{OrganizerId}";


    }
}
