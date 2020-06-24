﻿using AutoMapper.Configuration.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace QServiceDog.Models
{
    /// <summary>
    /// 事件订阅者
    /// </summary>
    public class EventSubscriber : Q.DevExtreme.Tpl.Models.IKeyEntity<Guid>
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string EMail { get; set; }

        public string WXName { get; set; }

        public bool IsEnable { get; set; } = true;
    }

    /// <summary>
    /// 访问类型（暂不实现）
    /// </summary>
    public class aa
    {

    }

    /// <summary>
    /// 事件推送记录
    /// </summary>
    public class EventPushRecord : Q.DevExtreme.Tpl.Models.IKeyEntity<Guid>
    {
        [Key]
        public Guid Id { get; set; }

        public Guid Subscriber { get; set; }

        public Guid Event { get; set; }

        public DateTime PushTime { get; set; }
        public bool Pushed { get; set; }

        [NotMapped]
        public virtual EventInfo EventInfo { get; set; }

        [NotMapped]
        public virtual EventSubscriber EventSubscriber { get; set; }

    }

    public class Sender : Q.DevExtreme.Tpl.Models.IKeyEntity<Guid>
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }

        public string TypeName { get; set; } = "EMail";// email ,wx

        public string Para { get; set; }//发送参数
        public bool IsEnable { get; set; }
    }
}
