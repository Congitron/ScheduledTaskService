using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ScheduledTaskService.Settings
{
    [XmlRoot("TaskConfig")]
    public class TaskConfig
    {
        [XmlElement("Tasks", Type = typeof(Tasks))]
        public Tasks Tasks { get; set; }
    }
}
