using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItcastCater.Model;
using System.Data;
using System.Data.SQLite;

namespace ItcastCater.DAL
{
    public class MemberTypeDAL
    {
        /// <summary>
        /// 查询所有没有被删除的会员类型
        /// </summary>
        /// <returns>会员类型集合</returns>
        public List<MemberType> GetAllMemberTypeByDelFlag()
        {
            string sql = "select MemType, MemTpName from MemberType where DelFlag = 0";
            DataTable dt = SqliteHelper.ExecuteTable(sql);
            List<MemberType> list = new List<MemberType>();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                {
                    MemberType memtp = RowToMemberType(item);
                    list.Add(memtp);
                }
            }
            return list;
        }

        private MemberType RowToMemberType(DataRow item)
        {
            MemberType mem = new MemberType();
            mem.MemTpName = item["MemTpName"].ToString();
            mem.MemType = Convert.ToInt32(item["MemType"]);
            return mem;
        }
    }
}
