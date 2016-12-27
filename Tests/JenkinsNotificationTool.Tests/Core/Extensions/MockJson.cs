namespace JenkinsNotificationTool.Tests.Core.Extensions
{
    using System.Runtime.Serialization;

    /// <summary>
    /// テスト用のモックJsonクラスです。
    /// </summary>
    [DataContract]
    public class MockJson
    {
        [DataMember]
        public string project { get; set; }

        [DataMember]
        public int number { get; set; }

        [DataMember]
        public string status { get; set; }

        [DataMember]
        public string result { get; set; }
    }
}