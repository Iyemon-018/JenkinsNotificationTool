namespace JenkinsNotification.Core.ComponentModels
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;
    using JenkinsNotification.Core.Logs;
    using JenkinsNotification.Core.Properties;
    using JenkinsNotification.Core.Utility;
    using Microsoft.Practices.Prism.Mvvm;
    using Microsoft.Practices.Prism.ViewModel;

    /// <summary>
    /// 全てのViewModel 基底オブジェクト クラスです。
    /// </summary>
    /// <seealso cref="BindableBase" />
    /// <seealso cref="INotifyDataErrorInfo" />
    public abstract class ViewModelBase : BindableBase, INotifyDataErrorInfo
    {
        #region Fields

        /// <summary>
        /// このエンティティの子要素コレクション
        /// </summary>
        private readonly List<ViewModelBase> _children = new List<ViewModelBase>();

        /// <summary>
        /// エラー情報を保持するコンテナ
        /// </summary>
        private readonly ErrorsContainerCustom<string> _errorsContainer;

        /// <summary>
        /// <see cref="_errorsContainer"/> の非同期ロックオブジェクト
        /// </summary>
        private readonly object _validationLock = new object();

        #endregion

        #region Ctor

        /// <summary>
        /// コンストラクタ
        /// </summary>
        protected ViewModelBase()
        {
            _errorsContainer = new ErrorsContainerCustom<string>(OnErrorChanged);
            PropertyChangedEventManager.AddHandler(this, Self_OnPropertyChanged, string.Empty);
        }

        #endregion

        #region Methods

        /// <summary>
        /// このインスタンスの検証を行います。
        /// </summary>
        public void Validate()
        {
            lock (_validationLock)
            {
                using (TimeTracer.StartNew($"{GetType().Name} のオブジェクト検証を実施する。"))
                {
                    var context = new ValidationContext(this, null, null);
                    var validationResults = new List<ValidationResult>();

                    //
                    // プロパティを検証する。
                    // 検証結果はvalidationErrors に格納される。
                    //
                    var validate = Validator.TryValidateObject(this, context, validationResults, true);

                    // まとめて検証結果をクリアする。
                    _errorsContainer.ClearAll();

                    if (!validate)
                    {
                        //
                        // グループごとの検証結果を生成する。
                        // Key:プロパティ名, Value:Key プロパティの検証結果コレクション
                        // 検証結果をコンテナに登録する。
                        //
                        var byPropertyNames = from result in validationResults
                                              from memberName in result.MemberNames
                                              group result by memberName
                                              into g
                                              select g;
                        foreach (var property in byPropertyNames)
                        {
                            _errorsContainer.SetErrors(property.Key, property.Select(x => x.ErrorMessage));
                        }
                    }

                    //
                    // 派生先で独自の検証ロジックを実行する。
                    //
                    OnValidate();
                }
            }
        }

        /// <summary>
        /// エンティティの子要素を追加します。
        /// </summary>
        /// <param name="child"><see cref="ViewModelBase" /> オブジェクトの子要素</param>
        /// <exception cref="System.ArgumentException"><paramref name="child"/> が既に登録されている場合にスローされます。</exception>
        /// <example>
        /// 現在のインスタンスの持つ<see cref="ViewModelBase" /> プロパティを追加します。
        /// <code><![CDATA[
        /// public class ExampleClass : ViewModelBase
        /// {
        ///     public PersonViewModel Person { get; private set; }
        /// 
        ///     public ExampleClass()
        ///     {
        ///         Person = new PersonViewModel();
        /// 
        ///         // Call "AddChild" method when constructor.
        ///         AddChild(Person);
        ///     }
        /// }
        /// 
        /// public class PersonViewModel : ViewModelBase
        /// {
        ///     private int _age;
        /// 
        ///     public int Age
        ///     {
        ///         get { return _age; }
        ///         set { SetProperty(ref _age, value); }
        ///     }
        /// }
        /// ]]></code>
        /// </example>
        protected void AddChild(ViewModelBase child)
        {
            if (_children.Contains(child))
            {
                throw new ArgumentException(Resources.ViewModelAddChildExistErrorMessage, nameof(child));
            }
            _children.Add(child);
        }

        /// <summary>
        /// エンティティの子要素コレクションを追加します。
        /// </summary>
        /// <param name="children"><see cref="ViewModelBase"/> オブジェクトの子要素コレクション</param>
        /// <exception cref="System.ArgumentException"><paramref name="children"/> の要素が既に登録されている場合にスローされます。</exception>
        /// <example>
        /// 現在のインスタンスの持つ<see cref="IEnumerable{ViewModelBase}"/> プロパティを追加します。
        /// <code><![CDATA[
        /// public class ExampleClass : ViewModelBase
        /// {
        ///     public ObservableCollection<PersonViewModel> People { get; private set; }
        /// 
        ///     public ExampleClass()
        ///     {
        ///         People = new ObservableCollection<PersonViewModel>();
        /// 
        ///         // Call "AddChildren" method when constructor.
        ///         AddChildren(People);
        ///     }
        /// }
        /// 
        /// public class PersonViewModel : ViewModelBase
        /// {
        ///     private int _age;
        /// 
        ///     public int Age
        ///     {
        ///         get { return _age; }
        ///         set { SetProperty(ref _age, value); }
        ///     }
        /// }
        /// ]]></code>
        /// </example>
        protected void AddChildren(IEnumerable<ViewModelBase> children)
        {
            foreach (var child in children)
            {
                if (_children.Contains(child))
                {
                    throw new ArgumentException(Resources.ViewModelAddChildExistErrorMessage, nameof(child));
                }
            }
            _children.AddRange(children);
        }

        /// <summary>
        /// リスナーにプロパティのエラーを通知します。
        /// </summary>
        /// <typeparam name="TProperty">プロパティを通知するインスタンスの型</typeparam>
        /// <param name="propertyExpression">プロパティを識別する式木</param>
        /// <param name="message">通知するエラーメッセージ</param>
        /// <remarks>
        /// このメソッドを呼び出すことで<see cref="INotifyDataErrorInfo"/> を経由してエラー情報がリスナー(コントロール) に通知されます。<para/>
        /// コントロールが受信したエラー情報は依存関係プロパティを経由してXaml(View) に伝搬します。
        /// </remarks>
        /// <example>
        /// 通常、このメソッドはオーバーライドしたOnValidate() メソッドで使用します。
        /// プロパティの属性による検証で判定できないような場合にこのメソッドを使用します。<para/>
        /// 例えば、以下に示すようにExampleViewModel.Value プロパティは、ExampleViewModel.IsChecked = true の場合のみ、しきい値の検証を行います。<para/>
        /// 検証結果が異常な場合は、<see cref="NotificationError{TProperty}"/> メソッドを呼び出してエラーメッセージを通知します。
        /// <code><![CDATA[
        /// public class ExampleViewModel : ViewModelBase
        /// {
        ///     private int _value;
        /// 
        ///     private bool _isChecked;
        /// 
        ///     public int Value
        ///     {
        ///         get { return _value; }
        ///         set { SetProperty(ref _value, value); }
        ///     }
        ///     
        ///     public bool IsChecked
        ///     {
        ///         get { return _isChecked; }
        ///         set { SetProperty(ref _isChecked, value); }
        ///     }
        /// 
        ///     protected override void OnValidate()
        ///     {
        ///         base.OnValidate();
        /// 
        ///         if (IsChecked)
        ///         {
        ///             if (Value < 0 || 100 < Value)
        ///             {
        ///                 NotificationError(() => Value, "Can not input Value. Range 0 To 100.");
        ///             }
        ///         }
        ///     }
        /// }
        /// ]]></code>
        /// </example>
        protected void NotificationError<TProperty>(Expression<Func<TProperty>> propertyExpression, string message)
        {
            _errorsContainer.SetError(propertyExpression, message);
        }

        /// <summary>
        /// リスナーにプロパティのエラーを通知します。
        /// </summary>
        /// <param name="propertyName">プロパティ名</param>
        /// <param name="message">通知するエラーメッセージ</param>
        protected void NotificationError(string propertyName, string message)
        {
            _errorsContainer.SetError(propertyName, message);
        }

        /// <summary>
        /// リスナーにプロパティのエラーを通知します。
        /// </summary>
        /// <typeparam name="TProperty">プロパティを通知するインスタンスの型</typeparam>
        /// <param name="propertyExpression">プロパティを識別する式木</param>
        /// <param name="messages">通知するエラーメッセージ</param>
        protected void NotificationErrors<TProperty>(Expression<Func<TProperty>> propertyExpression, IEnumerable<string> messages)
        {
            _errorsContainer.SetErrors(propertyExpression, messages);
        }

        /// <summary>
        /// リスナーにプロパティのエラーを通知します。
        /// </summary>
        /// <param name="propertyName">プロパティ名</param>
        /// <param name="messages">通知するエラーメッセージ</param>
        protected void NotificationErrors(string propertyName, IEnumerable<string> messages)
        {
            _errorsContainer.SetErrors(propertyName, messages);
        }

        /// <summary>
        /// このインスタンスの検証を実行します。<para/>
        /// 属性による検証以外で必要な検証はここで行います。
        /// </summary>
        protected virtual void OnValidate()
        {
            // Do nothing this class.
        }

        /// <summary>
        /// このオブジェクトのプロパティの値が変更された際に呼ばれるイベントハンドラです。
        /// </summary>
        /// <param name="sender">イベント送信元オブジェクト</param>
        /// <param name="e">イベント引数オブジェクト</param>
        /// <remarks>
        /// プロパティが変更された際に必要な処理を呼び出すには、このメソッドをオーバーライドしてください。
        /// </remarks>
        /// <example>
        /// あるプロパティの値が変化した際に特定の処理を実施する例を以下に示します。
        /// 以下の例では、ExampleViewModel のValue プロパティの値が変更された際に
        /// OnValueChanged メソッドが呼び出されます。
        /// <code><![CDATA[
        /// public class ExampleViewModel : ViewModelBase
        /// {
        ///     private int _value;
        /// 
        ///     public int Value
        ///     {
        ///         get { return _value; }
        ///         set { SetProperty(ref _value, value); }
        ///     }
        /// 
        ///     protected override void Self_OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        ///     {
        ///         base.Self_OnPropertyChanged(sender, e);
        /// 
        ///         switch (e.PropertyName)
        ///         {
        ///             case nameof(Value):
        ///                 OnValueChanged(Value);
        ///                 break;
        ///         }
        ///     }
        /// 
        ///     private void OnValueChanged(int newValue)
        ///     {
        ///         // Execute "Value" property changed work...
        ///     }
        /// }
        /// ]]></code>
        /// </example>
        protected virtual void Self_OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
        }

        /// <summary>
        /// プロパティが目的の値に一致するかどうかを判定します。<para/>
        /// プロパティの値を設定し、必要な場合にリスナーに通知します。
        /// </summary>
        /// <typeparam name="T">プロパティの型</typeparam>
        /// <param name="storage">getter およびsetter を両方持つプロパティへの参照です。</param>
        /// <param name="value">変更したいプロパティの値</param>
        /// <param name="propertyName">
        /// リスナーへ通知するプロパティの名称<para/>
        /// この値はオプションで、<see cref="CallerMemberNameAttribute"/> をサポートするコンパイラから呼び出されたときに自動的に提供されます。
        /// </param>
        /// <returns>値が変更されたときはtrue, 既存の値が目的の値と同じ場合はfalse を返します。</returns>
        protected override bool SetProperty<T>(ref T storage, T value,[CallerMemberName] string propertyName = null)
        {
            var changed = base.SetProperty(ref storage, value, propertyName);
            if (changed)
            {
                //
                // 変更されたのでプロパティの検証も行う。
                //
                ValidateProperty(value, propertyName);
        }
            return changed;
        }

        /// <summary>
        /// <see cref="ValidationAttribute"/> によるプロパティの検証を行います。
        /// </summary>
        /// <typeparam name="T">プロパティの型</typeparam>
        /// <param name="value">プロパティの値</param>
        /// <param name="propertyName">
        /// リスナーへ通知するプロパティの名称<para/>
        /// この値はオプションで、<see cref="CallerMemberNameAttribute"/> をサポートするコンパイラから呼び出されたときに自動的に提供されます。
        /// </param>
        internal void ValidateProperty(object value, [CallerMemberName] string propertyName = null)
        {
            lock (_validationLock)
            {
                using (TimeTracer.StartNew($"{propertyName} の検証を行う。"))
                {
                    var context = new ValidationContext(this) {MemberName = propertyName};
                    var validationErrors = new List<ValidationResult>();

                    //
                    // プロパティを検証する。
                    // 検証結果はvalidationErrors に格納される。
                    //
                    if (Validator.TryValidateProperty(value, context, validationErrors))
                    {
                        // 検証結果が正常だったため、エラー内容をクリアする。
                        _errorsContainer.ClearErrors(propertyName);
                    }
                    else
                    {
                        // 検証異常だった内容をコンテナに保存する。
                        // この内容はリスナーに通知される。
                        var errors = validationErrors.Select(x => x.ErrorMessage);
                        _errorsContainer.SetErrors(propertyName, errors);

                        foreach (var error in errors)
                        {
                            LogManager.Debug($"{GetType().Name}.{propertyName} をエラーにする。({error})");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// <see cref="ErrorsChanged"/>イベントを呼び出します。
        /// </summary>
        /// <param name="propertyName">プロパティ名</param>
        private void OnErrorChanged([CallerMemberName] string propertyName = null)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        #endregion

        #region INotifyDataErrorInfo メンバーの実装

        /// <summary>
        /// 指定されたプロパティまたはエンティティ全体の検証エラーを取得します。
        /// </summary>
        /// <param name="propertyName">検証エラーを取得するプロパティの名前。または、エンティティ レベルのエラーを取得する場合は null または <see cref="F:System.String.Empty" />。</param>
        /// <returns>プロパティまたはエンティティの検証エラー。</returns>
        public IEnumerable GetErrors(string propertyName)
        {
            return _errorsContainer.GetErrors(propertyName);
        }

        /// <summary>
        /// エンティティに検証エラーがあるかどうかを示す値を取得します。<para/>
        /// この<see cref="ViewModelBase"/> の子要素に検証エラーがある場合でもtrueを返します。
        /// </summary>
        public bool HasErrors => _errorsContainer.HasErrors || _children.Any(x => x.HasErrors);

        /// <summary>
        /// プロパティまたはエンティティ全体の検証エラーが変更されたときに発生します。
        /// </summary>
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        #endregion
    }
}