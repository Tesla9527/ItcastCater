using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SQLite;
using ItcastCater.Model;

namespace ItcastCater.DAL
{
    public class MemberInfoDAL
    {
        public int AddMemberInfo(MemberInfo member)
        {
            string sql = "insert into MemberInfo(MemName,MemMobilePhone,MemAddress,MemType,MemNum,MemGender,MemDiscount,MemMoney,DelFlag,SubTime,MemIntegral,MemEndServerTime,MemBirthday)values(@MemName,@MemMobilePhone,@MemAddress,@MemType,@MemNum,@MemGender,@MemDiscount,@MemMoney,@DelFlag,@SubTime,@MemIntegral,@MemEndServerTime,@MemBirthday)";
            return AddAndUpdate(member, sql, 1);
        }

        public int UpdateMemberInfo(MemberInfo member)
        {
            string sql = "update MemberInfo set MemName=@MemName,MemMobilePhone=@MemMobilePhone,MemAddress=@MemAddress,MemType=@MemType,MemNum=@MemNum,MemGender=@MemGender,MemDiscount=@MemDiscount,MemMoney=@MemMoney,MemIntegral=@MemIntegral,MemEndServerTime=@MemEndServerTime,MemBirthday=@MemBirthday where MemberId=@MemberId";           
            return AddAndUpdate(member, sql, 2);
        }

        //新增和修改的合并方法----哈哈,太好了
        private int AddAndUpdate(MemberInfo member, string sql, int temp)
        {
            SQLiteParameter[] param = { 
                  new SQLiteParameter("@MemName",member.MemName),
                  new SQLiteParameter("@MemMobilePhone",member.MemMobilePhone),
                   new SQLiteParameter("@MemAddress",member.MemAddress),
                    new SQLiteParameter("@MemType",member.MemType),
                     new SQLiteParameter("@MemNum",member.MemNum),
                      new SQLiteParameter("@MemGender",member.MemGender),
                       new SQLiteParameter("@MemDiscount",member.MemDiscount),
                        new SQLiteParameter("@MemMoney",member.MemMoney),
                         // new SQLiteParameter("@MemmberId",memmber.MemmberId),
                              new SQLiteParameter("@MemIntegral",member.MemIntegral),
                              new SQLiteParameter("@MemEndServerTime",member.MemEndServerTime),
                              new SQLiteParameter("@MemBirthdaty",member.MemBirthday)

                                      };
            List<SQLiteParameter> list = new List<SQLiteParameter>();
            list.AddRange(param);
            if (temp == 1)//新增
            {
                list.Add(new SQLiteParameter("@SubTime", member.SubTime));
                list.Add(new SQLiteParameter("@DelFlag", member.DelFlag));
            }
            else if (temp == 2)//修改
            {
                list.Add(new SQLiteParameter("@MemberId", member.MemberId));
            }

            return SqliteHelper.ExecuteNonQuery(sql, list.ToArray());
        }

        /// <summary>
        /// 根据会员的id查询该会员的信息
        /// </summary>
        /// <param name="memberId">会员id</param>
        /// <returns></returns>
        public MemberInfo GetMemberInfoByMemberId(int memberId)
        {
            string sql = "select * from MemberInfo where MemberId = @memberId and DelFlag=0";
            DataTable dt = SqliteHelper.ExecuteTable(sql, new SQLiteParameter("@memberId", memberId));
            MemberInfo mem = null;
            if (dt.Rows.Count > 0)
            {
                mem = RowToMemberInfo(dt.Rows[0]);

            }
            return mem;
        }

        /// <summary>
        /// 根据会员id删除该会员
        /// </summary>
        /// <param name="memberId">会员id</param>
        /// <returns>受影响的行数</returns>
        public int DeleteMemberByMemberId(int memberId)
        {
            string sql = "update MemberInfo set DelFlag=1 where MemberId=@MemberId";
            return SqliteHelper.ExecuteNonQuery(sql, new SQLiteParameter("@MemberId", memberId));
        }

        /// <summary>
        /// 查询所有没被删除的会员
         //</summary>
        /// <param name="delFlag">删除标识</param>
        /// <returns>会员对象集合</returns>
        public List<MemberInfo> GetAllMemberInfoByDelFlag(int delFlag)
        {
            string sql = "select * from MemberInfo where DelFlag=@DelFlag";
            DataTable dt = SqliteHelper.ExecuteTable(sql, new SQLiteParameter("@DelFlag", delFlag));
            List<MemberInfo> list = new List<MemberInfo>();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                {
                    MemberInfo mem = RowToMemberInfo(item);
                    list.Add(mem);
                }
            }
            return list;
        }

        //关系转对象
        private MemberInfo RowToMemberInfo(DataRow dr)
        {
            MemberInfo mem = new MemberInfo();
            mem.MemAddress = dr["MemAddress"].ToString();
            mem.MemBirthday = Convert.ToDateTime(dr["MemBirthday"]);
            mem.MemDiscount = Convert.ToDecimal(dr["MemDiscount"]);
            mem.MemEndServerTime = Convert.ToDateTime(dr["MemEndServerTime"]);
            mem.MemGender = dr["MemGender"].ToString();
            mem.MemIntegral = Convert.ToInt32(dr["MemIntegral"]);
            mem.MemberId = Convert.ToInt32(dr["MemberId"]);
            mem.MemMobilePhone = dr["MemMobilePhone"].ToString();
            mem.MemMoney = Convert.ToDecimal(dr["MemMoney"]);
            mem.MemName = dr["MemName"].ToString();
            mem.MemNum = dr["MemNum"].ToString();
            //mem.MemTpName
            mem.MemType = Convert.ToInt32(dr["MemType"]);
            mem.SubTime = Convert.ToDateTime(dr["SubTime"]);
            return mem;
        }
    }
}
