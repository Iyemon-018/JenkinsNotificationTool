namespace JenkinsNotificationTool.Tests.Core.Executers
{
    using System;
    using JenkinsNotification.Core.Executers;

    /// <summary>
    /// テスト用のモックExecuter クラスです。
    /// </summary>
    /// <seealso cref="JenkinsNotification.Core.Executers.IExecuter" />
    public class MockExecuter : IExecuter
    {
        private readonly Func<string, bool> _canExecuteMessage;

        private readonly Func<byte[], bool> _canExecuteData;

        private readonly Action _execute;

        public MockExecuter(Func<string, bool> canExecuteMessage, Action execute)
        {
            _canExecuteMessage = canExecuteMessage;
            _execute = execute;
        }

        public MockExecuter(Func<byte[], bool> canExecuteData, Action execute)
        {
            _canExecuteData = canExecuteData;
            _execute = execute;
        }

        public bool CanExecute(string message)
        {
            return _canExecuteMessage(message);
        }

        public bool CanExecute(byte[] data)
        {
            return _canExecuteData(data);
        }

        public void Execute()
        {
            _execute();
        }
    }
}