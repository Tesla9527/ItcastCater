using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItcastCater.DAL;
using ItcastCater.Model;

namespace ItcastCater.BLL
{
    public class MemberInfoBLL
    {
        MemberInfoDAL dal = new MemberInfoDAL();

        /// <summary>
        /// 新增和修改
        /// </summary>
        /// <param name="member">会员对象</param>
        /// <param name="temp">标识， 1---新增， 2---修改</param>
        /// <returns></returns>
        public bool SaveMember(MemberInfo member, int temp)
        {
            int r = -1;
            if (temp == 1) //新增
            {
                r = dal.AddMemberInfo(member);
            }
            else if (temp == 2) //修改
            {
                r = dal.UpdateMemberInfo(member);
            }
            return r > 0 ? true : false;
        }

        /// <summary>
        /// 根据会员的id查询该会员的信息
        /// </summary>
        /// <param name="memberId">会员id</param>
        /// <returns></returns>
        public MemberInfo GetMemberInfoByMemberId(int memberId)
        {
            return dal.GetMemberInfoByMemberId(memberId);
        }
        /// <summary>
        /// 根据会员id删除该会员
        /// </summary>
        /// <param name="memberId">会员id</param>
        /// <returns>受影响的行数</returns>
        public bool DeleteMemberByMemberId(int memberId)
        {
            return dal.DeleteMemberByMemberId(memberId) > 0 ? true : false ;
        }

        /// <summary>
        /// 查询所有没被删除的会员
        /// </summary>
        /// <param name="delFlag">删除标识</param>
        /// <returns>会员对象集合</returns>
        public List<MemberInfo> GetAllMemberInfoByDelFlag(int delFlag)
        {
            return dal.GetAllMemberInfoByDelFlag(delFlag);
        }
    }
}
