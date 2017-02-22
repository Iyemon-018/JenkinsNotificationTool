namespace JenkinsNotificationTool.Services
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using JenkinsNotification.Core.Services;
    public class ApplicationService : IApplicationService
    {
        private readonly Application _appCurrent = Application.Current;

        private readonly List<Action> _shutdownTasks;

        public ApplicationService()
        {
            _shutdownTasks = new List<Action>();
        }

        public void AddShutdownTask(Action task)
        {
            _shutdownTasks.Add(task);
        }

        public void Shutdown()
        {
            foreach (var task in _shutdownTasks)
            {
                task.Invoke();
            }

            _appCurrent.Shutdown();
        }
    }
}