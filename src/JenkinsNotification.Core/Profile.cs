namespace JenkinsNotification.Core
{
    using System;
    using Configurations;
    using Extensions;
    using Jenkins.Api;
    using JenkinsNotification.Core.Jenkins.WebApi.Response;
    using JenkinsNotification.Core.ViewModels.Api.Converter;
    using JenkinsNotification.Core.ViewModels.WebApi;
    using ViewModels.Api;
    using ViewModels.Configurations;

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
            CreateMap<string, JobStatus>().ConvertUsing(ApiConverter.JobStatusStringToEnum);
            CreateMap<string, JobResultType>().ConvertUsing(ApiConverter.JobResultTypeStringToEnum);

            //
            // オブジェクトのマッピング
            //
            CreateMap<NotifyConfiguration, NotifyConfigurationViewModel>()
                .ReverseMap()
                .ForMember(d => d.PopupTimeoutValue, o => o.Ignore());

            CreateMap<JobExecuteResult, JobExecuteResultViewModel>()
                .ForMember(d => d.Received, o => o.Ignore())
                .ForMember(d => d.Name, o => o.MapFrom(s => s.project))
                .ForMember(d => d.BuildNumber, o=>o.MapFrom(s => s.number))
                .ForMember(d => d.Status, o=> o.MapFrom(s => s.status))
                .ForMember(d => d.Result, o => o.MapFrom(s => s.result));

            // WebAPI データのマッピング
            CreateMap<Job, JobViewModel>()
                    .ForMember(d => d.TypeName, o => o.MapFrom(s => s._class))
                    .ForMember(d => d.Name, o => o.MapFrom(s => s.name))
                    .ForMember(d => d.Url, o => o.MapFrom(s => s.url))
                    .ForMember(d => d.Color, o => o.MapFrom(s => WebApiConverter.ToJobColor(s.color)));
        }
    }
}