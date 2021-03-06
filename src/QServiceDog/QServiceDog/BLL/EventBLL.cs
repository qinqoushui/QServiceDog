﻿using Microsoft.AspNetCore.Components;
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

        public void AddEvent(EventInfo info, bool onlyNew)
        {
            //本地添加事件不产生订阅
            using (var ef = new ServiceDBContext())
            {
                if (onlyNew)
                {
                    if (ef.EventInfo.Count(r => r.Type == info.Type && r.Time == info.Time) > 0)
                        return;
                }
                ef.EventInfo.Add(info);
                ef.SaveChanges();
            }
        }

        public int AddEvent4Cloud(List<EventInfo> infoList)
        {
            using (var ef = new ServiceDBContext())
            {
                int c = 0;
                foreach (var info in infoList)
                {
                    if (!ef.EventInfo.Any(r => r.Id == info.Id))
                    {
                        ef.EventInfo.Add(info);
                        c++;
#if DEBUG
                    }
                    //补足推送记录
                    if (!ef.EventPushRecord.Any(r => r.Event == info.Id))
                    {
#endif 
                        //根据订阅规则生成待推送记录
                        var sub = ef.ClientEventSubscriber.Include(r => r.EventSubscriber).Where(r => r.Client == info.Client && r.EventSubscriber.IsEnable).ToList();
                        sub.ForEach(s =>
                        {
                            ef.EventPushRecord.Add(new EventPushRecord()
                            {
                                Id = Guid.NewGuid(),
                                Pushed = false,
                                PushTime = DateTime.Now,
                                Event = info.Id,
                                Subscriber = s.Subscriber
                            });
                        });
                    }
                }
                ef.SaveChanges();
                return c;
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
                return ef.EventPushRecord.Include(r => r.EventInfo).Include(r => r.EventSubscriber).Where(r => !r.Pushed && r.EventInfo.Time >= s && r.EventInfo.Time <= e).ToList();
            }
        }

        public List<Sender> FetchSenderList()
        {
            using (var ef = new ServiceDBContext())
            {
                return ef.Sender.Where(r => r.IsEnable).ToList();
            }
        }
    }
}
