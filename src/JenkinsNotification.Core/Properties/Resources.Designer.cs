﻿//------------------------------------------------------------------------------
// <auto-generated>
//     このコードはツールによって生成されました。
//     ランタイム バージョン:4.0.30319.42000
//
//     このファイルへの変更は、以下の状況下で不正な動作の原因になったり、
//     コードが再生成されるときに損失したりします。
// </auto-generated>
//------------------------------------------------------------------------------

namespace JenkinsNotification.Core.Properties {
    using System;
    
    
    /// <summary>
    ///   ローカライズされた文字列などを検索するための、厳密に型指定されたリソース クラスです。
    /// </summary>
    // このクラスは StronglyTypedResourceBuilder クラスが ResGen
    // または Visual Studio のようなツールを使用して自動生成されました。
    // メンバーを追加または削除するには、.ResX ファイルを編集して、/str オプションと共に
    // ResGen を実行し直すか、または VS プロジェクトをビルドし直します。
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public Resources() {
        }
        
        /// <summary>
        ///   このクラスで使用されているキャッシュされた ResourceManager インスタンスを返します。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("JenkinsNotification.Core.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   厳密に型指定されたこのリソース クラスを使用して、すべての検索リソースに対し、
        ///   現在のスレッドの CurrentUICulture プロパティをオーバーライドします。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   アプリケーション設定ファイルの読み込みに失敗しました。 に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string ConfigurationLoadFailedMessage {
            get {
                return ResourceManager.GetString("ConfigurationLoadFailedMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   構成ファイルの検証結果がエラーです。 に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string ConfigurationVerifyLoadedErrorMessage {
            get {
                return ResourceManager.GetString("ConfigurationVerifyLoadedErrorMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   構成情報の検証結果がエラーです。 に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string ConfigurationVerifySaveErrorMessage {
            get {
                return ResourceManager.GetString("ConfigurationVerifySaveErrorMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   受信履歴に表示する最大データ数は、{0}～{1} を設定してください。 に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string DisplayHistoryCountOutOfRangeMessage {
            get {
                return ResourceManager.GetString("DisplayHistoryCountOutOfRangeMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   指定した型は列挙体ではありません。 に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string EnumTypeUnmatchMessage {
            get {
                return ResourceManager.GetString("EnumTypeUnmatchMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   指定したファイルは存在しません。 に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string FileNotFoundMessage {
            get {
                return ResourceManager.GetString("FileNotFoundMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   指定したオブジェクトは既に子要素として登録されています。 に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string ViewModelAddChildExistErrorMessage {
            get {
                return ResourceManager.GetString("ViewModelAddChildExistErrorMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   &apos;ViewModelBase&apos; クラスを継承していない型が設定されています。&apos;ViewModelBase&apos; の派生クラスを設定してください。 に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string ViewModelAttributeUnmatchTypeErrorMessage {
            get {
                return ResourceManager.GetString("ViewModelAttributeUnmatchTypeErrorMessage", resourceCulture);
            }
        }
    }
}
