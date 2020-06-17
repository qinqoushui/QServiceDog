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
        public UserBLL(DbContext _context)
        {
            dbSet = _context.Set<User>();
        }
        public bool GetUserByPassword(string userNo, string password, out bool needChangePassword, out (string no, string msg) error)
        {
            needChangePassword = false;
            var oldUser = dbSet.Where(r => r.UserNo == userNo).ToList();
            if (oldUser?.Count < 1)
            {
                error = ("UserName", "用户名错误");
                return false;
            }
            //密码加盐计算
            var saltPassword = password.makePassword();
            var a = oldUser.FirstOrDefault(r => r.Password == saltPassword);
            if (a != null)
            {
                needChangePassword = userNo.Equals("admin", StringComparison.OrdinalIgnoreCase) && password.Equals("admin", StringComparison.OrdinalIgnoreCase);
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
