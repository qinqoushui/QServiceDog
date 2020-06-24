using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using QServiceDog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QServiceDog.BLL
{
    public class EventBLL
    {
        static EventBLL _ = new EventBLL();
        public static EventBLL Instance { get; } = _;

        public void AddEvent(EventInfo info)
        {
            using (var ef = new ServiceDBContext())
            {
                ef.EventInfo.Add(info);
                //TODO:根据订阅规则进行订阅
                ef.EventSubscriber.Where(r => r.IsEnable
               ).ToList().ForEach(s =>
               {
                   ef.EventPushRecord.Add(new EventPushRecord()
                   {
                       Id = Guid.NewGuid(),
                       Pushed = false,
                       PushTime = DateTime.Now,
                       Event = info.Id,
                       Subscriber = s.Id
                   });
               });
                ef.SaveChanges();
            }
        }

        public List<EventInfo> FetchListByTime(DateTime s, DateTime e)
        {
            using (var ef = new ServiceDBContext())
            {
                return ef.EventInfo.Where(r => r.Time >= s && r.Time <= e).ToList();
            }
        }

        public List<EventPushRecord> FetchNeedPushListByTime(DateTime s, DateTime e)
        {
            using (var ef = new ServiceDBContext())
            {
                return ef.EventPushRecord.Include(r=>r.EventInfo).Include(r=>r.EventSubscriber).Where(r =>!r.Pushed && r.EventInfo.Time >= s && r.EventInfo.Time <= e).ToList();
            }
        }

        public List<Sender> FetchSenderList()
        {
            using (var ef = new ServiceDBContext())
            {
                return ef.Sender.Where(r=>r.IsEnable).ToList();
            }
        }
    }
}
