using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
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
        protected override void setEntityKey(EventSubscriber r)
        {
            r.Id = Guid.NewGuid();
        }

        [HttpGet("Get2")]
        public IList<EventSubscriber> Get2()
        {
            return db.Set<EventSubscriber>().Where(r => r.IsEnable).ToList();
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
            return db.Set<EventPushRecord>().Include(r => r.EventSubscriber).Include(r => r.EventInfo).OrderByDescending(r => r.EventInfo.Time).Take(1000).ToList();
        }

    }
    public class ClientEventSubscriberController : EntityController<ClientEventSubscriber, Guid>
    {
        public ClientEventSubscriberController(DbContext db) : base(db)
        {

        }

        public IActionResult Index()
        {
            return View();
        }

        public override IList<ClientEventSubscriber> Get()
        {
            return db.Set<ClientEventSubscriber>().Include(r => r.EventSubscriber).ToList();
        }
        protected override void setEntityKey(ClientEventSubscriber r)
        {
            r.Id = Guid.NewGuid();
        }
        protected override bool beforeSave(ClientEventSubscriber r, out string err)
        {
            if (db.Set<ClientEventSubscriber>().Any(o =>r.Id!=o.Id && r.Subscriber == o.Subscriber && r.Client == o.Client))
            {
                err = "已存在该规则";
                return false;
            }
            else
                return base.beforeSave(r, out err);

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
        protected override void setEntityKey(Sender r)
        {
            r.Id = Guid.NewGuid();
        }
    }
}
