using DevExtreme.AspNet.Data.Helpers;
using Microsoft.AspNetCore.Mvc;
using Q.DevExtreme.Tpl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QServiceDog.Controllers
{
    public class HomeController : Q.DevExtreme.Tpl.Controllers.MyHomeController
    {
        public HomeController(IUserBLL _bll) : base(_bll)
        {

        }

        public IActionResult Index()
        {
            return RedirectToAction("Index", "EventInfo");
        }
    }
}
