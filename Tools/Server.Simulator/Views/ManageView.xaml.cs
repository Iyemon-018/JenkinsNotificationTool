namespace Server.Simulator.Views
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Threading.Tasks;
    using JenkinsNotification.Core.ComponentModels;
    using JenkinsNotification.CustomControls;
    using Server.Simulator.ViewModels;

    /// <summary>
    /// ManageView.xaml の相互作用ロジック
    /// </summary>
    [ViewModel(typeof(ManageViewModel))]
    public partial class ManageView : View
    {
        #region Ctor

        public ManageView()
        {
            InitializeComponent();
        }

        #endregion
    }
}
