using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ItcastCater.DAL;
using ItcastCater.Model;

namespace ItcastCater.BLL
{
    public class UserInfoBLL
    {
        UserInfoDAL dal = new UserInfoDAL();
        //判断用户是否登录成功
        public bool GetUserPwdByLoginName(string loginName, string pwd, out string msg)
        {
            bool flag = false;
            //根据用户帐号查询该用户的信息
            UserInfo user = dal.GetUserPwdByLoginName(loginName);
            if (user != null)
            {
                //帐号存在
                //判断密码
                if (user.Pwd == pwd)
                {
                    msg = "登录成功";
                    flag = true;
                }
                else
                {
                    //密码错误
                    msg = "帐号或密码错误";
                }
            }
            else
            {
                msg = "用户名不存在";
            }
            return flag;
        }

    }
}
