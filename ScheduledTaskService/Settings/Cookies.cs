using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ScheduledTaskService.Settings
{
    [Serializable]
    public class Cookies
    {
        [XmlElement("Cookie", Type = typeof(Cookie))]
        public Cookie[] Cookie { get; set; }
    }
}
