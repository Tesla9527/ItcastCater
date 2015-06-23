using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItcastCater.DAL;
using ItcastCater.Model;

namespace ItcastCater.BLL
{
   public  class MemberTypeBLL
    {
       MemberTypeDAL dal = new MemberTypeDAL();
       /// <summary>
       /// 查询所有没有被删除的会员类型
       /// </summary>
       /// <returns>会员类型集合</returns>
       public List<MemberType> GetAllMemberTypeByDelFlag()
       {
           return dal.GetAllMemberTypeByDelFlag();
       }
    }
}
