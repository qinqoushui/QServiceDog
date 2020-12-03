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
    public class ServiceInfoController : EntityController<ServiceInfo, Guid>
    {
        public ServiceInfoController(DbContext db) : base(db)
        {

        }

        public IActionResult Index()
        {
            return View();
        }


    }


    public class ServiceTplController : EntityController<ServiceTpl, Guid>
    {
        public ServiceTplController(DbContext db) : base(db)
        {

        }

        public IActionResult Index()
        {
            return View();
        }
        //[Authorize(Policy = "IsAdmin")]
        //[HttpPost]
        //public virtual IActionResult Post(string values);
        //[Authorize(Policy = "IsAdmin")]
        //[HttpPut]
        //public virtual IActionResult Put(string key, string values);
    }
}
