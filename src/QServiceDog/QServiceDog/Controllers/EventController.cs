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
    public class EventInfoController :   EntityController<EventInfo, Guid>
    {
        public EventInfoController(DbContext db) : base(db)
        {

        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
