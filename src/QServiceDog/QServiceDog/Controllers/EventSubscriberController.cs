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
        public EventSubscriberController(Microsoft.AspNetCore.Authorization.IAuthorizationService authorizationService, Microsoft.EntityFrameworkCore.DbContext _db) : base(authorizationService, _db)
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

        public override IActionResult Put(string key, string values)
        {
            try
            {
                return base.Put(key, values);
            }
            catch (Exception ex)
            {
                return NotFound(ex.ToString());
            }
        }

        [HttpGet("Get2")]
        public IList<EventSubscriber> Get2()
        {
            return db.Set<EventSubscriber>().Where(r => r.IsEnable).ToList();
        }
    }

    public class EventPushRecordController : EntityController<EventPushRecord, Guid>
    {

        public EventPushRecordController(Microsoft.AspNetCore.Authorization.IAuthorizationService authorizationService, Microsoft.EntityFrameworkCore.DbContext _db) : base(authorizationService, _db)
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

        public ClientEventSubscriberController(Microsoft.AspNetCore.Authorization.IAuthorizationService authorizationService, Microsoft.EntityFrameworkCore.DbContext _db) : base(authorizationService, _db)
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
        protected override bool beforeSave(ClientEventSubscriber r, ClientEventSubscriber old, out string err)
        {
            if (db.Set<ClientEventSubscriber>().Any(o =>r.Id!=o.Id && r.Subscriber == o.Subscriber && r.Client == o.Client))
            {
                err = "已存在该规则";
                return false;
            }
            else
                return base.beforeSave(r,old, out err);

        }
    }
    public class SenderController : EntityController<Sender, Guid>
    {

        public SenderController(Microsoft.AspNetCore.Authorization.IAuthorizationService authorizationService, Microsoft.EntityFrameworkCore.DbContext _db) : base(authorizationService, _db)
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
