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

        //-------------------------------------------------------------------------------
        public void SetCurrentEvent(Event _event)
        {
            _httpContextAccessor.HttpContext.Session.SetString("thisEvent", JsonConvert.SerializeObject(_event));
        }

        public Event GetCurrentEvent()
        {
            string _thisEventJsonStr = _httpContextAccessor.HttpContext.Session.GetString("thisEvent");
            if (!string.IsNullOrWhiteSpace(_thisEventJsonStr))
            {
                var currentEvent = JsonConvert.DeserializeObject<Event>(_thisEventJsonStr.ToString());
                if (currentEvent != null)
                    return currentEvent;
            }
            return null;
        }




        public Organizer GetLoggedInEventOrgSession()
        {
            string _thisEventJsonStr = _httpContextAccessor.HttpContext.Session.GetString("loggedInEventOrganizer");
            if (!string.IsNullOrWhiteSpace(_thisEventJsonStr))
            {
                var currentEvent = JsonConvert.DeserializeObject<Organizer>(_thisEventJsonStr.ToString());
                if (currentEvent != null)
                    return currentEvent;
            }
            return null;
        }

        public void SetLoggedInEventOrgSession(Organizer _org)
        {
            _httpContextAccessor.HttpContext.Session.SetString("loggedInEventOrganizer", JsonConvert.SerializeObject(_org));
        }
    }
}
