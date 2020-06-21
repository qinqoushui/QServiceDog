using Microsoft.AspNetCore.Components;
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
    }
}
