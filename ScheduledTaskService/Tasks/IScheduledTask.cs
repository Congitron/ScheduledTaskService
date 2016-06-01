using ScheduledTaskService.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduledTaskService.Tasks
{
    public interface IScheduledTask
    {
        // Probably don't need the properties to be part of the interface, just the validation and init

        // What units of time are used? Seconds? Minutes?
        string Name { get; set; }
        string Description { get; set; }
        int RunTime { get; set; }
        string Endpoint { get; set; }
        string Method { get; set; }
        string Domain { get; set; }
        Cookies Cookies { get; set; }
        Data Data { get; set; }
        void RunTask();
        bool ValidateSettings(ScheduledTaskSettings settings);
        void AssignSettings(ScheduledTaskSettings settings);
    }
}
