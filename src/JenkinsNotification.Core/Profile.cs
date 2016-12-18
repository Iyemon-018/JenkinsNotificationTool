namespace JenkinsNotification.Core
{
    using JenkinsNotification.Core.Configurations;
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
            CreateMap<NotifyConfiguration, NotifyConfigurationViewModel>()
                .ReverseMap();
        }
    }
}