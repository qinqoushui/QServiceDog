using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Q.DevExtreme.Tpl.Controllers;
using QServiceDog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QServiceDog.Controllers
{
    //public class CloudEntityController<T, TKey> : EntityController<T, TKey> where T : class, Q.DevExtreme.Tpl.Models.IKeyEntity<TKey>, new()
    //{
    //    string client = string.Empty;
    //    public CloudEntityController(Microsoft.AspNetCore.Authorization.IAuthorizationService authorizationService, Microsoft.EntityFrameworkCore.DbContext _db) : base(authorizationService, _db)
    //    {
    //        this.client = Helpers.GlobalConfig.Cloud;
    //    }

    //    public override void OnActionExecuting(ActionExecutingContext context)
    //    {
    //        base.OnActionExecuting(context);
    //        if (client == Helpers.GlobalConfig.Cloud)
    //        {
    //            if (Helpers.GlobalConfig.Instance.Client == Helpers.GlobalConfig.Cloud)
    //            {
    //                //云模式下才显示与云模式有关的action

    //            }
    //            else
    //            {
    //                context.Result = new ContentResult() { Content = "仅限在服务器上使用，请后退" };
    //            }
    //        }
    //        else
    //        {

    //        }
    //    }
    //}
    public class DogActionController : EntityController<DogAction, Guid>
    {

        public DogActionController(Microsoft.AspNetCore.Authorization.IAuthorizationService authorizationService, Microsoft.EntityFrameworkCore.DbContext _db) : base(authorizationService, _db)
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
