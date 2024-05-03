using EventQR.Models;

namespace EventQR.Services
{
    public interface IEventOrganizer
    {
        public Organizer GetLoggedInEventOrg();
        public Task<Event> GetEventById(Guid _eventId);
        public void SetCurrentEvent(Event _event);
        public Event GetCurrentEvent();
    }
}
