namespace JenkinsNotification.Core
{
    using System.Collections.Generic;
    using System.Linq;
    using JenkinsNotification.Core.Communicators;
    using JenkinsNotification.Core.Executers;
    using JenkinsNotification.Core.Logs;

    public class DataManager : IDataManager
    {
        public List<IExecuter> Tasks => new List<IExecuter>();

        public void ReceivedData(ReceivedType receivedType, string message, byte[] data)
        {
            var receivedData = new JenkinsData(receivedType, message, data);
            var task = Tasks.FirstOrDefault(x => x.CanExecute(receivedData));
            task?.Execute();
            if (task == null)
            {
                LogManager.Info("実行可能なタスクはありませんでした。");
            }
        }

        public void AddTask(IExecuter task)
        {
            Tasks.Add(task);
        }

        public void AddTasks(IEnumerable<IExecuter> tasks)
        {
            Tasks.AddRange(tasks);
        }
    }
}