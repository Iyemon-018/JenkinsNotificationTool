namespace JenkinsNotification.Debug.LogViewer.Services
{
    using System.Windows;
    using JenkinsNotification.Core.Services;
    public class ApplicationService : IApplicationService
    {
        public void Shutdown()
        {
            Application.Current.Shutdown();
        }
    }
}