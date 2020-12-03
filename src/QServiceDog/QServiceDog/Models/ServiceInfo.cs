using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace QServiceDog.Models
{
    /// <summary>
    /// 服务信息
    /// </summary>
    public class ServiceInfo : Q.DevExtreme.Tpl.Models.IKeyEntity<Guid>
    {
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// 服务名
        /// </summary>
        [Required]
        public string Name { get; set; }
        public string Desc { get; set; }
        public string Client { get; set; }

        public string RunName { get; set; } //服务启动参数
        public string RunData { get; set; } //服务启动参数
        public string StopName { get; set; }
        public string StopData { get; set; }

        public string CheckName { get; set; }
        public string CheckData { get; set; }


        public DateTime LastAliveTime { get; set; } //上次存活时间
        public DateTime LastStopTime { get; set; } //上次存活时间

        public TimeSpan IdleTime { get; set; }//允许发呆的时间

        public TimeSpan RestartTime { get; set; } //强制重启时间

        public bool IsEnable { get; set; } = true;//启用

        [NotMapped]
        public bool IsAlive
        {
            get
            {
                return LastAliveTime.Add(IdleTime) > DateTime.Now;
            }
        }
    }


    public class ServiceTpl : Q.DevExtreme.Tpl.Models.IKeyEntity<Guid>
    {
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// 服务名
        /// </summary>
        [Required]
        public string Name { get; set; }
        public string Desc { get; set; }
        public string Client { get; set; }

        public string RunName { get; set; } //服务启动参数
        public string RunData { get; set; } //服务启动参数
        public string StopName { get; set; }
        public string StopData { get; set; }

        public string CheckName { get; set; }
        public string CheckData { get; set; }


        public DateTime LastAliveTime { get; set; } //上次存活时间
        public DateTime LastStopTime { get; set; } //上次存活时间

        public TimeSpan IdleTime { get; set; }//允许发呆的时间

        public TimeSpan RestartTime { get; set; } //强制重启时间

        public bool IsEnable { get; set; } = true;//启用

    }
}
