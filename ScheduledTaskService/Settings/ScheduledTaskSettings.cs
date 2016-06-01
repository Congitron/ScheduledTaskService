using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ScheduledTaskService.Settings
{
    [Serializable]
    public class ScheduledTaskSettings
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string RunTime { get; set; }
        public string Endpoint { get; set; }
        public string Method { get; set; }
        public string Domain { get; set; }
        [XmlElement("Data", Type = typeof(Data))]
        public Data Data { get; set; }
        [XmlElement("Cookies", Type = typeof(Cookies))]
        public Cookies Cookies { get; set; }
    }
}
