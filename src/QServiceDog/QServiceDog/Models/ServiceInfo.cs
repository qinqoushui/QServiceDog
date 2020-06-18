using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QServiceDog.Models
{
    /// <summary>
    /// 服务信息
    /// </summary>
    public class ServiceInfo:Q.DevExtreme.Tpl.Models.IKeyEntity<Guid>
    {
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// 服务名
        /// </summary>
        [Required]
        public string Name { get; set; }
        public string Desc { get; set; }
        public string FileName { get; set; } //文件名决定 进程 名

        public string Run { get; set; } //服务启动参数
        public string Stop { get; set; } //服务启动参数

        public string Check  { get; set; } //检测服务是否存在的方法：Get/Post Url ,Telnet IPEndPoint ,Ping IP,GetProcess ProcessName


        public DateTime AliveTime { get; set; } //上次存活时间

        public TimeSpan IdleTime { get; set; }//允许发呆的时间

        public TimeSpan RestartTime { get; set; } //强制重启时间
    }
}
