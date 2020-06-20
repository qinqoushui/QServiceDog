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
    public class ServiceInfoController :   EntityController<ServiceInfo, Guid>
    {
        public ServiceInfoController(DbContext db) : base(db)
        {

        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
