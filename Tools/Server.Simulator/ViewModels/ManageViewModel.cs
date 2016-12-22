namespace Server.Simulator.ViewModels
{
    using JenkinsNotification.Core.ComponentModels;
    using JenkinsNotification.Core.Services;

    public class ManageViewModel : ShareViewModelBase
    {
        public ManageViewModel(IDialogService dialogService) : base(dialogService)
        {
        }
    }
}