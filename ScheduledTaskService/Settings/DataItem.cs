using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduledTaskService.Settings
{
    [Serializable]
    public class DataItem
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
