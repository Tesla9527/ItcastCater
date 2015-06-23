using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ItcastCater.BLL;
using ItcastCater.Model;

namespace ItcastCater
{
    public partial class FrmMemberInfo : Form
    {
        public FrmMemberInfo()
        {
            InitializeComponent();
        }

        public event EventHandler evt;//事件，传值用

        private void FrmMemberInfo_Load(object sender, EventArgs e)
        {
            //加载所有未被删除的会员
            LoadMemberInfoByDelFlag(0);
        }

        //加载所有会员
        private void LoadMemberInfoByDelFlag(int p)
        {
            MemberInfoBLL bll = new MemberInfoBLL();
            dgvMember.AutoGenerateColumns = false;//禁止自动生成列
            dgvMember.DataSource = bll.GetAllMemberInfoByDelFlag(p);
            dgvMember.SelectedRows[0].Selected = false;//默认第一行不选中
        }

        //删除会员
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvMember.SelectedRows.Count > 0)
            {
                //获取选中行的id
                int id = Convert.ToInt32(dgvMember.SelectedRows[0].Cells[0].Value);
                MemberInfoBLL bll = new MemberInfoBLL();
                if (bll.DeleteMemberByMemberId(id))
                {
                    MessageBox.Show("操作成功");
                    LoadMemberInfoByDelFlag(0);
                }
                else
                {
                    MessageBox.Show("操作失败");
                }
            }
            else
            {
                MessageBox.Show("对不起，请选中删除的行");
            }


        }

        //新增会员，1表示新增
        private void btnAddMember_Click(object sender, EventArgs e)
        {
            ShowFrmChangeMember(1);
        }

        //修改会员，2表示修改
        private void btnUpdateMember_Click(object sender, EventArgs e)
        {
            if (dgvMember.SelectedRows.Count > 0)
            {
                int id = Convert.ToInt32(dgvMember.SelectedRows[0].Cells[0].Value);
                //根据id去数据库查询这条数据是否存在
                MemberInfoBLL bll = new MemberInfoBLL();
                MemberInfo mem = bll.GetMemberInfoByMemberId(id);
                fea.obj = mem;
                ShowFrmChangeMember(2);
            }   
            else
            {
                MessageBox.Show("请选中修改的行");
            }

        }

        FrmEventAgrs fea = new FrmEventAgrs();
        private void ShowFrmChangeMember(int p)
        {
            FrmChangeMember fcm = new FrmChangeMember();
            this.evt += new EventHandler(fcm.SetText);//注册事件

            fea.Temp = p;//传的新增或修改的标识
            if (this.evt != null) //执行事件之前要判断不能为空
            {
                this.evt(this, fea);

            }
            //新增和修改窗体关闭后会员窗口刷新
            fcm.FormClosed += new FormClosedEventHandler(fcm_FormClosed);
            fcm.ShowDialog();

        }

        private void fcm_FormClosed(object sender, FormClosedEventArgs e)
        {
            LoadMemberInfoByDelFlag(0);
        }
    }
}
