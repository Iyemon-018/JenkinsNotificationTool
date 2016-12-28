namespace JenkinsNotification.Core.Executers
{
    public interface IExecuter
    {
        bool CanExecute(object value);

        void Execute();
    }
}