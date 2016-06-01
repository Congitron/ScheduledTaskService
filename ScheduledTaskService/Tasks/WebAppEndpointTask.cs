using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScheduledTaskService.Settings;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;

namespace ScheduledTaskService.Tasks
{
    [TaskSetting("WebAppEndpointTask")]
    class WebAppEndpointTask : ScheduledTask
    { 
        public override bool ValidateSettings(ScheduledTaskSettings settings)
        {
            if (
                (string.IsNullOrWhiteSpace(settings.RunTime) ||
                Regex.Matches(settings.RunTime, "[0-9][0-9]:[0-9][0-9]:[0-9][0-9]").Count != 1)
                ||
                string.IsNullOrWhiteSpace(settings.Endpoint)
                ||
                string.IsNullOrWhiteSpace(settings.Domain)
                ) { return false; }

            return true;
        }

        public override void AssignSettings(ScheduledTaskSettings settings)
        {
            // Alternatively, we could just save the settings object or combine the classes somehow.
            // But this is a little more flexible.
            Name = settings.Name;
            Description = (string.IsNullOrEmpty(settings.Description)) ? "" : settings.Description;
            
            Match dateTimeMatch = Regex.Match(settings.RunTime, "([0-9][0-9]):([0-9][0-9]):([0-9][0-9])");
            int hours = Convert.ToInt32(dateTimeMatch.Groups[1].ToString());
            int minutes = Convert.ToInt32(dateTimeMatch.Groups[2].ToString());
            int seconds = Convert.ToInt32(dateTimeMatch.Groups[3].ToString());
            RunTime = (hours * 3600) + (minutes * 60) + seconds;

            Endpoint = settings.Endpoint;
            Method = settings.Method;
            Domain = settings.Domain;
            if (settings.Cookies != null) { Cookies = settings.Cookies; } // optional
            if (settings.Data != null) { Data = settings.Data; }
        }

        public override void RunTask()
        {
            try
            {
                // Cookies
                CookieContainer container = new CookieContainer();
                CookieCollection collection = new CookieCollection();
                foreach (Settings.Cookie cookieSetting in Cookies.Cookie)
                {
                    System.Net.Cookie cookie = new System.Net.Cookie(cookieSetting.Name, cookieSetting.Value);
                    cookie.Domain = Domain;
                    collection.Add(cookie);
                }
                container.Add(collection);

                // Request
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Endpoint);
                request.ContentType = "application/json";
                request.Method = "POST";
                request.CookieContainer = container;

                // Body / JSON
                string json = "{";
                foreach (DataItem item in Data.DataItem)
                {
                    json += $"\"{item.Key}\":\"{item.Value}\",";
                }
                json = json.TrimEnd(',') + "}";

                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                // Response
                var response = (HttpWebResponse)request.GetResponse();
                string result;
                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }
                response.Close();
                
                // Do some logging if the result isn't true
            }
            catch (Exception ex) { }
        }        
    }
}