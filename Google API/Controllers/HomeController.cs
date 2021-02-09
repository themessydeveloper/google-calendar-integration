using Newtonsoft.Json.Linq;
using System;
using System.Globalization;
using System.Web.Mvc;

namespace GoogleAPI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult OauthRedirect()
        {
            var credentialsFile = "C:\\Users\\Purshotam\\Desktop\\Google API\\Google API\\Files\\credentials.json";

            JObject credentials = JObject.Parse(System.IO.File.ReadAllText(credentialsFile));
            var client_id = credentials["client_id"];

            var redirectUrl = "https://accounts.google.com/o/oauth2/v2/auth?" +
                               "scope=https://www.googleapis.com/auth/calendar+https://www.googleapis.com/auth/calendar.events&" +
                               "access_type=offline&" +
                               "include_granted_scopes=true&" +
                               "response_type=code&" +
                               "state=hellothere&" +
                               "redirect_uri=https://localhost:44372/oauth/callback&" +
                               "client_id="+client_id;

            return Redirect(redirectUrl);
        }
    }
}