using Microsoft.AspNetCore.Components;
using QServiceDog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QServiceDog.BLL
{
    public class ServiceBLL
    {
        static ServiceBLL _ = new ServiceBLL();
        public static ServiceBLL Instance { get; } = _;


        public List<ServiceInfo> FetchList(bool isAll = false)
        {
            using (var ef = new ServiceDBContext())
            {
                if (isAll)
                    return ef.ServiceInfo.ToList();
                else
                    return ef.ServiceInfo.Where(r => r.IsEnable).ToList();
            }
        }

        internal int Add4Cloud(List<ServiceInfo> data)
        {
            //新增或更新
            using (var ef = new ServiceDBContext())
            {
                var client = data.First().Client;
                var oldList = ef.ServiceInfo.Where(r=>r.Client==client).ToList();
                ef.ServiceInfo.RemoveRange(oldList);
                ef.ServiceInfo.AddRange(data);
                return ef.SaveChanges();
            }
        }

        //public void Update(List<ServiceInfo> serviceInfo)
        //{
        //    using (var ef = new ServiceDBContext())
        //    {
        //        if (isAll)
        //            return ef.ServiceInfo.ToList();
        //        else
        //            return ef.ServiceInfo.Where(r => r.IsEnable).ToList();
        //    }
        //}

    }
}
