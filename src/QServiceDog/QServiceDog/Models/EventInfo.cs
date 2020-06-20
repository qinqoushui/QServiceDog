using QServiceDog.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QServiceDog.Models
{
    public class EventInfo : Q.DevExtreme.Tpl.Models.IKeyEntity<Guid>
    {
        [Key]
        public Guid Id { get; set; }

        public string Client { get; set; } = GlobalConfig.Instance.Client;
        
        public string Type { get; set; } //类型
        public DateTime Time { get; set; }

        public string Msg { get; set; }
    }
}
