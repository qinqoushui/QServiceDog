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
