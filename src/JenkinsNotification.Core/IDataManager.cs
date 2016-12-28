namespace JenkinsNotification.Core
{
    using System.Collections.Generic;
    using JenkinsNotification.Core.Executers;

    public interface IDataManager
    {
        void AddTask(IExecuter task);

        void AddTasks(IEnumerable<IExecuter> tasks);

        void ReceivedData(ReceivedType receivedType, string eMessage, byte[] getData);
    }
}