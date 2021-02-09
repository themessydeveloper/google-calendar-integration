using Google_API.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace GoogleAPI.Controllers
{
    public class CalendarEventController : Controller
    {
        [HttpPost]
        public ActionResult CreateEvent(Event calendarEvent)
        {
            var tokenFile = "TOKEN FILE PATH e.g. C:\\Users\\USER NAME\\Desktop\\Google API\\Google API\\Files\\tokens.json";
            var tokens = JObject.Parse(System.IO.File.ReadAllText(tokenFile));

            RestClient restClient = new RestClient();
            RestRequest request = new RestRequest();

            calendarEvent.Start.DateTime = DateTime.Parse(calendarEvent.Start.DateTime).ToString("yyyy-MM-dd'T'HH:mm:ss.fffK");
            calendarEvent.End.DateTime = DateTime.Parse(calendarEvent.End.DateTime).ToString("yyyy-MM-dd'T'HH:mm:ss.fffK");

            var model = JsonConvert.SerializeObject(calendarEvent, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });

            request.AddQueryParameter("key", "YOUR API KEY");
            request.AddHeader("Authorization", "Bearer " + tokens["access_token"]);
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", model, ParameterType.RequestBody);

            restClient.BaseUrl = new System.Uri("https://www.googleapis.com/calendar/v3/calendars/primary/events");
            var response = restClient.Post(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return RedirectToAction("Index", "Home", new { status = "success"});
            }

            return View("Error");
        }

        public ActionResult Event(string identifier)
        {
            var tokenFile = "TOKEN FILE PATH e.g. C:\\Users\\USER NAME\\Desktop\\Google API\\Google API\\Files\\tokens.json";
            var tokens = JObject.Parse(System.IO.File.ReadAllText(tokenFile));

            RestClient restClient = new RestClient();
            RestRequest request = new RestRequest();

            request.AddQueryParameter("key", "YOUR API KEY");
            request.AddHeader("Authorization", "Bearer " + tokens["access_token"]);
            request.AddHeader("Accept", "application/json");

            restClient.BaseUrl = new System.Uri("https://www.googleapis.com/calendar/v3/calendars/primary/events/"+identifier);
            var response = restClient.Get(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                JObject calendarEvent = JObject.Parse(response.Content);
                return View(calendarEvent.ToObject<Event>());
            }

            return View("Error");
        }

        public ActionResult AllEvents()
        {
            var tokenFile = "TOKEN FILE PATH e.g. C:\\Users\\USER NAME\\Desktop\\Google API\\Google API\\Files\\tokens.json";
            var tokens = JObject.Parse(System.IO.File.ReadAllText(tokenFile));

            RestClient restClient = new RestClient();
            RestRequest request = new RestRequest();

            request.AddQueryParameter("key", "YOUR API KEY");
            request.AddHeader("Authorization", "Bearer " + tokens["access_token"]);
            request.AddHeader("Accept", "application/json");

            restClient.BaseUrl = new System.Uri("https://www.googleapis.com/calendar/v3/calendars/primary/events");
            var response = restClient.Get(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                JObject calendarEvents = JObject.Parse(response.Content);
                var allEvents = calendarEvents["items"].ToObject<IEnumerable<Event>>();
                return View(allEvents);
            }

            return View("Error");
        }

        [HttpGet]
        public ActionResult UpdateEvent(string identifier)
        {
            var tokenFile = "TOKEN FILE PATH e.g. C:\\Users\\USER NAME\\Desktop\\Google API\\Google API\\Files\\tokens.json";
            var tokens = JObject.Parse(System.IO.File.ReadAllText(tokenFile));

            RestClient restClient = new RestClient();
            RestRequest request = new RestRequest();

            request.AddQueryParameter("key", "YOUR API KEY");
            request.AddHeader("Authorization", "Bearer " + tokens["access_token"]);
            request.AddHeader("Accept", "application/json");

            restClient.BaseUrl = new System.Uri("https://www.googleapis.com/calendar/v3/calendars/primary/events/" + identifier);
            var response = restClient.Get(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                JObject calendarEvent = JObject.Parse(response.Content);
                return View(calendarEvent.ToObject<Event>());
            }

            return View("Error");
        }

        [HttpPost]
        public ActionResult UpdateEvent(string identifier, Event calendarEvent)
        {
            var tokenFile = "TOKEN FILE PATH e.g. C:\\Users\\USER NAME\\Desktop\\Google API\\Google API\\Files\\tokens.json";
            var tokens = JObject.Parse(System.IO.File.ReadAllText(tokenFile));

            RestClient restClient = new RestClient();
            RestRequest request = new RestRequest();

            calendarEvent.Start.DateTime = DateTime.Parse(calendarEvent.Start.DateTime).ToString("yyyy-MM-dd'T'HH:mm:ss.fffK");
            calendarEvent.End.DateTime = DateTime.Parse(calendarEvent.End.DateTime).ToString("yyyy-MM-dd'T'HH:mm:ss.fffK");

            var model = JsonConvert.SerializeObject(calendarEvent, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });

            request.AddQueryParameter("key", "YOUR API KEY");
            request.AddHeader("Authorization", "Bearer " + tokens["access_token"]);
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", model, ParameterType.RequestBody);

            restClient.BaseUrl = new System.Uri("https://www.googleapis.com/calendar/v3/calendars/primary/events/"+identifier);
            var response = restClient.Patch(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return RedirectToAction("AllEvents", "CalendarEvent", new { status = "updated" });
            }

            return View("Error");
        }

        public ActionResult DeleteEvent(string identifier)
        {
            var tokenFile = "TOKEN FILE PATH e.g. C:\\Users\\USER NAME\\Desktop\\Google API\\Google API\\Files\\tokens.json";
            var tokens = JObject.Parse(System.IO.File.ReadAllText(tokenFile));

            RestClient restClient = new RestClient();
            RestRequest request = new RestRequest();

            request.AddQueryParameter("key", "YOUR API KEY");
            request.AddHeader("Authorization", "Bearer " + tokens["access_token"]);
            request.AddHeader("Accept", "application/json");

            restClient.BaseUrl = new System.Uri("https://www.googleapis.com/calendar/v3/calendars/primary/events/" + identifier);
            var response = restClient.Delete(request);

            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return RedirectToAction("AllEvents", "CalendarEvent", new { status = "deleted" });
            }

            return View("Error");
        }
    }
}