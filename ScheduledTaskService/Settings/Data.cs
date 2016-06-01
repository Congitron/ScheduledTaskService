using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ScheduledTaskService.Settings
{
    [Serializable]
    public class Data
    {
        [XmlElement("DataItem", Type = typeof(DataItem))]
        public DataItem[] DataItem { get; set; }
    }
}