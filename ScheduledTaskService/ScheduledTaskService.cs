using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using ScheduledTaskService.Settings;
using ScheduledTaskService.Tasks;
using System.Threading;
using System.Xml.Serialization;
using System.Configuration;

namespace ScheduledTaskService
{
    public partial class ScheduledTaskService : ServiceBase
    {
        private TaskConfig _config;
        private List<IScheduledTask> _tasks = new List<IScheduledTask>();

        public ScheduledTaskService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            LoadSettings();
            InitializeTasks();
            InitializeScheduler();
        }

        protected override void OnStop()
        {

        }

        private void LoadSettings()
        {
            AppSettingsReader appSettings = new AppSettingsReader();
            string configPath = (string)appSettings.GetValue("ConfigPath", typeof(String));
            string xml = File.ReadAllText(configPath);
            var reader = new StringReader(xml);
            var serializer = new XmlSerializer(typeof(TaskConfig));
            _config = (TaskConfig)serializer.Deserialize(reader);
        }

        private void InitializeTasks()
        {
            try
            {
                Dictionary<string, ScheduledTaskSettings> nameToSettings = new Dictionary<string, ScheduledTaskSettings>();
                foreach (ScheduledTaskSettings task in _config.Tasks.Task)
                {
                    if (string.IsNullOrEmpty(task.Name)) { continue; }
                    nameToSettings.Add(task.Name, task);
                }

                Type searchType = typeof(IScheduledTask);
                var taskTypes = System.Reflection.Assembly
                        .GetExecutingAssembly()
                        .GetTypes()
                        .Where(t => searchType.IsAssignableFrom(t) &&
                                    t != searchType)
                        .ToArray();

                foreach (Type type in taskTypes)
                {
                    Attribute[] typeAttributes = (Attribute[])type.GetCustomAttributes(typeof(TaskSettingAttribute), false);
                    if (typeAttributes.Length == 0) { continue; }
                    TaskSettingAttribute taskSettingAttribute = (TaskSettingAttribute)typeAttributes[0];

                    if (string.IsNullOrEmpty(taskSettingAttribute.Name)) { continue; }
                    if (!nameToSettings.ContainsKey(taskSettingAttribute.Name)) { continue; }

                    ScheduledTaskSettings taskSettings = nameToSettings[taskSettingAttribute.Name];
                    IScheduledTask scheduledTask = (IScheduledTask)Activator.CreateInstance(type);

                    scheduledTask.ValidateSettings(taskSettings);
                    scheduledTask.AssignSettings(taskSettings);

                    _tasks.Add(scheduledTask);
                }
            }
            catch (Exception ex) { } // Log error and exit
        }

        private void InitializeScheduler()
        {
            foreach (IScheduledTask task in _tasks)
            {
                TaskRunner taskRunner = new TaskRunner(task);
                Thread thread = new Thread(taskRunner.Start);
                thread.Start();
            }
        }
    }
}
