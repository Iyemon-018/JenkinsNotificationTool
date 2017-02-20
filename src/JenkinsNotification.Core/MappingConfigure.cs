namespace JenkinsNotification.Core
{
    using System;
    using System.Collections.Generic;
    using AutoMapper;

    /// <summary>
    /// マッピング情報を構成するための機能クラスです。
    /// </summary>
    public class MappingConfigure
    {
        #region Fields

        /// <summary>
        /// マッピング プロファイル情報コレクション
        /// </summary>
        private readonly List<Type> _profileTypes;

        #endregion

        #region Ctor

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MappingConfigure()
        {
            _profileTypes = new List<Type>();
        }

        #endregion

        #region Methods

        /// <summary>
        /// 登録されたマッピング プロファイル構成を初期化します。
        /// </summary>
        public void Initialize()
        {
            Mapper.Initialize(x =>
                              {
                                  foreach (var profileType in _profileTypes)
                                  {
                                      x.AddProfile(profileType);
                                  }
                              });
            Mapper.AssertConfigurationIsValid();
        }

        /// <summary>
        /// プロファイルの型を登録します。
        /// </summary>
        /// <param name="profileType">マッピング プロファイルの型</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="profileType"/> がnull の場合にスローされます。</exception>
        public void RegisterProfileType(Type profileType)
        {
            if (profileType == null) throw new ArgumentNullException(nameof(profileType));
            _profileTypes.Add(profileType);
        }

        #endregion
    }
}