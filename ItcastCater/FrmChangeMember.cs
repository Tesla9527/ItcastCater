using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ItcastCater.Model;
using ItcastCater.BLL;

namespace ItcastCater
{
    public partial class FrmChangeMember : Form
    {
        public FrmChangeMember()
        {
            InitializeComponent();
        }

        //加载所有会员类别
        public void LoadMemberType()
        {
            MemberTypeBLL bll = new MemberTypeBLL();
            List<MemberType> list = bll.GetAllMemberTypeByDelFlag();
            list.Insert(0, new MemberType() { MemType = -1, MemTpName = "请选择" });
            cmbMemType.DataSource = list;
            cmbMemType.DisplayMember = "MemTpName";
            cmbMemType.ValueMember = "MemType";
        }

        private int Tp { get; set; }

        //用来传值
        public void SetText(object sender, EventArgs e)
        {
            LoadMemberType();
            FrmEventAgrs fea = e as FrmEventAgrs;
            this.Tp = fea.Temp;
            foreach (Control item in this.Controls)
            {
                if (item is TextBox)
                {
                    TextBox tb = item as TextBox;//把控件转化成文本框
                    tb.Text = "";//清空所有文本框
                }
            }
            if (fea.Temp == 2) //修改
            {
                MemberInfo mem = fea.obj as MemberInfo;
                txtBirs.Text = mem.MemBirthday.ToString();//生日
                txtMemDiscount.Text = mem.MemDiscount.ToString();
                txtMemIntegral.Text = mem.MemIntegral.ToString();
                txtmemMoney.Text = mem.MemMoney.ToString();
                txtMemName.Text = mem.MemName;
                txtMemNum.Text = mem.MemNum;
                txtMemPhone.Text = mem.MemMobilePhone;
                cmbMemType.SelectedIndex = mem.MemType;
                txtAddress.Text = mem.MemAddress;
                rdoMan.Checked = mem.MemGender == "男" ? true : false;
                rdoWomen.Checked = mem.MemGender == "女" ? true : false;
                labId.Text = mem.MemberId.ToString();//id存起来
            }
            else if (fea.Temp == 1)
            {
                txtMemIntegral.Text = "0";
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            //获取会员信息
            //判断用户选择的是不是男还是女
            //每个文本框不能为空
            MemberInfo mem = new MemberInfo();
            if (CheckEmpty())
            {              
                mem.MemAddress = txtAddress.Text;
                mem.MemBirthday = Convert.ToDateTime(txtBirs.Text);
                mem.MemDiscount = Convert.ToDecimal(txtMemDiscount.Text);
                mem.MemEndServerTime = dtEndServerTime.Value;
                mem.MemGender = CheckGender();
                mem.MemIntegral = Convert.ToInt32(txtMemIntegral.Text);
                mem.MemMobilePhone = txtMemPhone.Text;
                mem.MemMoney = Convert.ToDecimal(txtmemMoney.Text);
                mem.MemName = txtMemName.Text;
                mem.MemNum = txtMemNum.Text;
                mem.MemType = Convert.ToInt32(cmbMemType.SelectedIndex);
            }

            MemberInfoBLL bll = new MemberInfoBLL();
            //新增还是修改
            if (this.Tp == 1)
            {
                mem.DelFlag = 0;
                mem.SubTime = System.DateTime.Now;
              
            }
            else if (this.Tp == 2)
            {
                mem.MemberId = Convert.ToInt32(labId.Text);
            }
            string st = bll.SaveMember(mem, this.Tp) ? "操作成功" : "操作失败";
            MessageBox.Show(st);
            this.Close();
        }

        //检查男女
        public string CheckGender()
        {
            if (rdoMan.Checked)
            {
                return "男";
            }
            else 
            {
                return  "女";
            }
      
        }

        //检查文本框不能为空
        private bool CheckEmpty()
        {
            if (string.IsNullOrEmpty(txtBirs.Text))
            {
                MessageBox.Show("生日不能为空");
                return false;
            }
            if (string.IsNullOrEmpty(txtMemDiscount.Text))
            {
                MessageBox.Show("折扣不能为空");
                return false;
            }
            if (string.IsNullOrEmpty(txtMemIntegral.Text))
            {
                MessageBox.Show("积分不能为空");
                return false;
            }
            if (string.IsNullOrEmpty(txtmemMoney.Text))
            {
                MessageBox.Show("余额不能为空");
                return false;
            }
            if (string.IsNullOrEmpty(txtMemName.Text))
            {
                MessageBox.Show("名字不能为空");
                return false;

            }
            if (string.IsNullOrEmpty(txtMemNum.Text))
            {
                MessageBox.Show("编号不能为空");
                return false;
            }
            if (string.IsNullOrEmpty(txtMemPhone.Text))
            {
                MessageBox.Show("电话不能为空");
                return false;
            }
            if (string.IsNullOrEmpty(dtEndServerTime.Text))
            {
                MessageBox.Show("有效期不能为空");
                return false;
            }
            return true;
        }

        private void FrmChangeMember_Load(object sender, EventArgs e)
        {
            //加载所有的会员类型
        }
    }
}
