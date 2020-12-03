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

        public ServiceInfoController(Microsoft.AspNetCore.Authorization.IAuthorizationService authorizationService, Microsoft.EntityFrameworkCore.DbContext _db) : base(authorizationService, _db)
        {

        }

        public IActionResult Index()
        {
            return View();
        }


    }


    public class ServiceTplController : EntityController<ServiceTpl, Guid>
    {
        public ServiceTplController(Microsoft.AspNetCore.Authorization.IAuthorizationService authorizationService, Microsoft.EntityFrameworkCore.DbContext _db)
            : base(authorizationService, _db)
        {

        }
        public IActionResult Index()
        {
            ViewData["IsAdmin"] = User != null && User.Claims != null && User.Claims.FirstOrDefault(r => r.Type == "Admin")?.Value == "true";
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
