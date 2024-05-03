using EventQR.EF;
using EventQR.Models;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;

namespace EventQR.Services
{
    public class EventOrganizer : IEventOrganizer
    {
        private readonly AppDbContext _dbContext;
        private readonly ITempDataDictionary _tempData;

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITempDataDictionaryFactory _tempDataDictionaryFactory;

        public EventOrganizer(AppDbContext dbContext, IHttpContextAccessor httpContextAccessor, ITempDataDictionaryFactory tempDataDictionaryFactory)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _tempDataDictionaryFactory = tempDataDictionaryFactory;
            _tempData = _tempDataDictionaryFactory.GetTempData(_httpContextAccessor.HttpContext);
        }

        public Event GetCurrentEvent()
        {

            string _thisEventJsonStr = _httpContextAccessor.HttpContext.Session.GetString("thisEvent");
            var currentEvent = JsonConvert.DeserializeObject<Event>(_thisEventJsonStr);
            return currentEvent;

        }

        public async Task<Event> GetEventById(Guid _eventId)
        {
            return await _dbContext.Events.FindAsync(_eventId);
        }

        public Organizer GetLoggedInEventOrg()
        {
            var loggedInEventOrg = _httpContextAccessor
                 .HttpContext.User.FindFirst("organizer").Value;
            return JsonConvert.DeserializeObject<Organizer>(loggedInEventOrg);
        }

        public void SetCurrentEvent(Event _event)
        {
            _tempData["CurrentEvent"] = _event;
        }
    }
}
