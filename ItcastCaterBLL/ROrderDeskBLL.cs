using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ItcastCater.DAL;
using ItcastCater.Model;

namespace ItcastCater.BLL
{
    public class ROrderDeskBLL
    {
        ROrderDeskDAL dal = new ROrderDeskDAL();
        /// <summary>
        /// 向中间表插入一条数据
        /// </summary>
        /// <param name="rod">中间表对象</param>
        /// <returns></returns>
        public bool AddROrderDesk(ROrderDesk rod)
        {
            return dal.AddROrderDesk(rod) > 0 ? true : false;
        }
    }
}
