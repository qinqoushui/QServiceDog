using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Q.DevExtreme.Tpl.Controllers;
using QServiceDog.BLL;
using QServiceDog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QServiceDog.Controllers
{
    public class EventInfoController : EntityController<EventInfo, Guid>
    {
        public EventInfoController(DbContext db) : base(db)
        {

        }

        public IActionResult Index()
        {
            return View();
        }

        public override IList<EventInfo> Get()
        {
            DateTime time = DateTime.Now.AddDays(-10);
            return db.Set<EventInfo>().Where(r => r.Time > time).ToList();
        }
        [HttpGet("GetClients")]
        public IList<string> GetClients()
        {
            return db.Set<EventInfo>().Select(r=>r.Client).Distinct().ToList();
        }

    }

    [ApiController]
    [Route("[Controller]")]
    public class SyncController : ControllerBase
    {
        /// <summary>
        /// 收取客户端上报来的数据
        /// </summary>
        [HttpPost("Event")]
        public object RecvEvent([FromBody] List<EventInfo> data)
        {
            if (data?.Count > 0)
            {
                int c = EventBLL.Instance.AddEvent4Cloud(data);
                //db.Set<EventInfo>()
                return new { Code = 0, Msg = $"AddNew:{c}/{data.Count}" };
            }
            else
                return new { Code = 0, Msg = "" };
        }

        [HttpPost("Service")]
        public object RecvService([FromBody] List<ServiceInfo> data)
        {
            int c = ServiceBLL.Instance.Add4Cloud(data);
            return new { Code = 0, Msg = $"{c}/{data.Count}" };
        }
    }
}
