namespace EventQR.Services
{
    public interface IQrCodeGenerator
    {

        public string GetQRCodeSystemPath(Guid guestId);
        public string GenerateQRCode(Guid guestId, Guid eventId);
    }
}
