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
        public EventInfoController(Microsoft.AspNetCore.Authorization.IAuthorizationService authorizationService, Microsoft.EntityFrameworkCore.DbContext _db) : base(authorizationService, _db)
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

        public IList<string> GetClients()
        {
            return db.Set<EventInfo>().Select(r => r.Client).Distinct().ToList();
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

        /// <summary>
        /// 服务当前状态上报
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost("Service")]
        public object RecvService([FromBody] List<ServiceInfo> data)
        {
            int c = ServiceBLL.Instance.Add4Cloud(data);
            return new { Code = 0, Msg = $"{c}/{data.Count}" };
        }


        [HttpPost("SyncTime")]
        public object SyncTime()
        {
#if DEBUG
            //return new { Code = 0, Msg = $"{DateTime.Now.AddMinutes(15).ToString("yyyy-MM-dd HH:mm:ss")}" };
#else
#endif
            return new { Code = 0, Msg = $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}" };
        }

    }
}
