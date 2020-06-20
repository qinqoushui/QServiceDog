using Microsoft.AspNetCore.Components;
using QServiceDog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QServiceDog.BLL
{
    public class DogActionBLL
    {
        static DogActionBLL _ = new DogActionBLL();
        public static DogActionBLL Instance { get; } = _;


        public List<DogAction> FetchList()
        {
            using (var ef = new ServiceDBContext())
            {
                return ef.DogAction.ToList();
            }
        }

       

    }
}
