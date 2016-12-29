namespace JenkinsNotification.Core
{
    using JenkinsNotification.Core.Configurations;
    using JenkinsNotification.Core.Extensions;
    using JenkinsNotification.Core.Jenkins.Api;
    using JenkinsNotification.Core.ViewModels.Api;
    using JenkinsNotification.Core.ViewModels.Configurations;

    /// <summary>
    /// このアセンブリのマッピング構成クラスです。
    /// </summary>
    /// <seealso cref="AutoMapper.Profile" />
    public class Profile : AutoMapper.Profile
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Profile()
        {
            //
            // 列挙体のマッピング
            //
            CreateMap<string, JobStatus>().ConvertUsing(s => s.ToEnum(JobStatus.None));

            //
            // オブジェクトのマッピング
            //
            CreateMap<NotifyConfiguration, NotifyConfigurationViewModel>()
                .ReverseMap()
                .ForMember(d => d.PopupTimeoutValue, o => o.Ignore());

            CreateMap<JobExecuteResult, IJobExecuteResult>()
                .ForMember(d => d.Name, o => o.MapFrom(s => s.project))
                .ForMember(d => d.BuildNumber, o=>o.MapFrom(s => s.number))
                .ForMember(d => d.Status, o=> o.MapFrom(s => s.status))
                .ForMember(d => d.Result, o => o.MapFrom(s => s.result));
        }
    }
}