﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace QDBDogUI.Properties {
    using System;
    
    
    /// <summary>
    ///   一个强类型的资源类，用于查找本地化的字符串等。
    /// </summary>
    // 此类是由 StronglyTypedResourceBuilder
    // 类通过类似于 ResGen 或 Visual Studio 的工具自动生成的。
    // 若要添加或移除成员，请编辑 .ResX 文件，然后重新运行 ResGen
    // (以 /str 作为命令选项)，或重新生成 VS 项目。
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   返回此类使用的缓存的 ResourceManager 实例。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("QDBDogUI.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   重写当前线程的 CurrentUICulture 属性
        ///   重写当前线程的 CurrentUICulture 属性。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   查找类似 USE master;
        ///CREATE MASTER KEY ENCRYPTION BY PASSWORD = &apos;ZyDBBak20200408&apos;;
        ///go
        /// 
        ///CREATE CERTIFICATE AutoBakCert FROM FILE = &apos;{0}\AutoBakCert.cer&apos;
        ///WITH PRIVATE KEY (  FILE = &apos;{0}\AutoBakCert.pkey&apos;, DECRYPTION  BY PASSWORD =&apos;Cert123&apos;);
        ///go
        ///
        ///print N&apos;启用证书和主控密钥完成&apos;;
        ///go  的本地化字符串。
        /// </summary>
        internal static string AutoBakCert {
            get {
                return ResourceManager.GetString("AutoBakCert", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找 System.Byte[] 类型的本地化资源。
        /// </summary>
        internal static byte[] AutoBakCert1 {
            get {
                object obj = ResourceManager.GetObject("AutoBakCert1", resourceCulture);
                return ((byte[])(obj));
            }
        }
        
        /// <summary>
        ///   查找 System.Byte[] 类型的本地化资源。
        /// </summary>
        internal static byte[] AutoBakCert2 {
            get {
                object obj = ResourceManager.GetObject("AutoBakCert2", resourceCulture);
                return ((byte[])(obj));
            }
        }
        
        /// <summary>
        ///   查找类似 USE master;
        ///IF EXISTS(SELECT 1 FROM sys.certificates WHERE name=&apos;AutoBakCert&apos;)
        ///BEGIN
        ///	BACKUP DATABASE [@dbname@] TO  DISK = N&apos;@path@.dll&apos; WITH RETAINDAYS = 30, NOFORMAT, INIT,  NAME = N&apos;@dbname@自动完整备份&apos;, SKIP, NOREWIND, NOUNLOAD,COMPRESSION,   STATS = 10 ,ENCRYPTION (ALGORITHM = AES_256, SERVER CERTIFICATE = AutoBakCert);
        ///END 
        ///go 的本地化字符串。
        /// </summary>
        internal static string BackupDB {
            get {
                return ResourceManager.GetString("BackupDB", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似于 (图标) 的 System.Drawing.Icon 类型的本地化资源。
        /// </summary>
        internal static System.Drawing.Icon database {
            get {
                object obj = ResourceManager.GetObject("database", resourceCulture);
                return ((System.Drawing.Icon)(obj));
            }
        }
        
        /// <summary>
        ///   查找 System.Byte[] 类型的本地化资源。
        /// </summary>
        internal static byte[] dbBack {
            get {
                object obj = ResourceManager.GetObject("dbBack", resourceCulture);
                return ((byte[])(obj));
            }
        }
        
        /// <summary>
        ///   查找类似 USE master;
        ///DROP CERTIFICATE AutoBakCert ;
        ///go
        ///
        ///drop MASTER KEY ;
        ///go
        ///
        ///print N&apos;移除证书和主控密钥完成&apos;;
        ///go  的本地化字符串。
        /// </summary>
        internal static string DropAutoBakCert {
            get {
                return ResourceManager.GetString("DropAutoBakCert", resourceCulture);
            }
        }
    }
}
