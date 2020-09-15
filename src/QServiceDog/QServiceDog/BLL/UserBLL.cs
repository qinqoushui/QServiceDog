using Microsoft.EntityFrameworkCore;
using Q.DevExtreme.Tpl;
using Q.DevExtreme.Tpl.Models;
using System;
using System.Linq;

namespace QServiceDog.BLL
{
    public class UserBLL : IUserBLL
    {

        DbSet<User> dbSet = null;
        DbContext context;
        public UserBLL(DbContext _context)
        {
            context = _context;
            dbSet = _context.Set<User>();
        }
        public bool GetUserByPassword(string userNo, string password, out IUser user, out (string no, string msg) error)
        {
            var oldUser = dbSet.Where(r => r.UserNo == userNo).ToList();
            if (oldUser == null || oldUser.Count == 0)
            {
                if (userNo.Equals("admin", StringComparison.OrdinalIgnoreCase))
                {
                    //自动创建用户
                    var tmpUser = new User()
                    {
                        Id = Guid.NewGuid(),
                        UserName = "管理员",
                        Role = "管理员",
                        UserNo = "admin",
                        Password = "admin".makePassword(),
                        NeedChangePassword = true
                    };
                    user = tmpUser as IUser;
                    dbSet.Add(tmpUser);
                    context.SaveChanges();
                    error = ("", "");
                    return true;
                }

                error = ("UserName", "用户名错误");
                user = null;
                return false;
            }
            //密码加盐计算
            var saltPassword = password.makePassword();
            user = oldUser.FirstOrDefault(r => r.Password == saltPassword);
            if (user != null)
            {
                user.NeedChangePassword = userNo.Equals("admin", StringComparison.OrdinalIgnoreCase) && password.Equals("admin", StringComparison.OrdinalIgnoreCase);
                error = ("", "");
                return true;
            }
            else
            {

                error = ("Password", "密码错误");
                return false;
            }
        }


    }
}
