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
    public class DogActionController : EntityController<DogAction, Guid>
    {
        public DogActionController(DbContext db) : base(db)
        {

        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("GetByType")]
        public List<DogAction> GetByType(string Type)
        {
            return db.Set<DogAction>().Where(r => r.Type == Type).ToList();
        }

    }
}
