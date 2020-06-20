using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QServiceDog.Models
{
    /// <summary>
    /// 行为动作
    /// </summary>
    public class DogAction : Q.DevExtreme.Tpl.Models.IKeyEntity<Guid>
    {
        [Key]
        public Guid Id { get; set; }

        
        public string Command { get; set; } 

        public string Name { get; set; }

        public string Type { get; set; }
        
    }
}
