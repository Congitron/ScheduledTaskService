using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ScheduledTaskService.Settings
{
    [Serializable]
    public class Tasks
    {
        [XmlElement("Task", Type = typeof(ScheduledTaskSettings))]
        public ScheduledTaskSettings[] Task { get; set; }
    }
}
