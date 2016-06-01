using ScheduledTaskService.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduledTaskService.Tasks
{
    public class ScheduledTask : IScheduledTask
    {
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual int RunTime { get; set; }
        public virtual string Endpoint { get; set; }
        public virtual string Method { get; set; }
        public virtual string Domain { get; set; }
        public Cookies Cookies { get; set; }
        public Data Data { get; set; }
        public virtual bool ValidateSettings(ScheduledTaskSettings settings) { return false; }
        public virtual void AssignSettings(ScheduledTaskSettings settings) { }

        public virtual void RunTask() { }
    }
}
