using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduledTaskService.Settings
{
    [System.AttributeUsage(System.AttributeTargets.Class|System.AttributeTargets.Struct)]
    class TaskSettingAttribute : System.Attribute
    {
        private string _name;
        public string Name { get { return _name; } } // This name is used to find the settings for the task class in the xml file
        
        public TaskSettingAttribute(string name) { _name = name; }
    }
}