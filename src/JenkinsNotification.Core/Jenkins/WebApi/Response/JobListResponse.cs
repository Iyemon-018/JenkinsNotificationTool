namespace JenkinsNotification.Core.Jenkins.WebApi.Response
{
    /// <summary>
    /// ジョブ一覧取得APIのレスポンスデータ クラスです。
    /// </summary>
    public class JobListResponse
    {
        public string _class { get; set; }
        public Assignedlabel[] assignedLabels { get; set; }
        public string mode { get; set; }
        public string nodeDescription { get; set; }
        public string nodeName { get; set; }
        public int numExecutors { get; set; }
        public object description { get; set; }
        public Job[] jobs { get; set; }
        public Overallload overallLoad { get; set; }
        public Primaryview primaryView { get; set; }
        public bool quietingDown { get; set; }
        public int slaveAgentPort { get; set; }
        public Unlabeledload unlabeledLoad { get; set; }
        public bool useCrumbs { get; set; }
        public bool useSecurity { get; set; }
        public View[] views { get; set; }
    }

    public class Overallload
    {
    }

    public class Primaryview
    {
        public string _class { get; set; }
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Unlabeledload
    {
        public string _class { get; set; }
    }

    public class Assignedlabel
    {
    }

    public class Job
    {
        public string _class { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public string color { get; set; }
    }

    public class View
    {
        public string _class { get; set; }
        public string name { get; set; }
        public string url { get; set; }
    }

}