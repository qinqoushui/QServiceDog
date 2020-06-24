using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Q.DevExtreme.Tpl.Controllers;
using QServiceDog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QServiceDog.Controllers
{
    public class EventSubscriberController : EntityController<EventSubscriber, Guid>
    {
        public EventSubscriberController(DbContext db) : base(db)
        {

        }

        public IActionResult Index()
        {
            return View();
        }
    }

    public class EventPushRecordController : EntityController<EventPushRecord, Guid>
    {
        public EventPushRecordController(DbContext db) : base(db)
        {

        }

        public IActionResult Index()
        {
            return View();
        }

        public override IList<EventPushRecord> Get()
        {
            return db.Set<EventPushRecord>().Include(r => r.EventSubscriber).Include(r => r.EventInfo).OrderByDescending(r=>r.EventInfo.Time).Take(1000).ToList();
        }

    }
    public class SenderController : EntityController<Sender, Guid>
    {
        public SenderController(DbContext db) : base(db)
        {

        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("GetSenderType")]
        public List<string> GetSenderType()
        {
            return Enum.GetNames(typeof(Enums.EnumSender)).Select(r => r.Substring(1)).ToList();
        }

    }
}
