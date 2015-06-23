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
    public partial class FrmLogin : Form
    {
        public FrmLogin()
        {
            InitializeComponent();
        }

        //登录
        private void btnLogin_Click(object sender, EventArgs e)
        {
            //首先获取帐号和密码，再判断帐号和密码不能为空
            string name = txtName.Text.Trim();
            string pwd = txtPwd.Text;
            string msg = "";
            if (CheckEmpty(name,pwd))
            {
                UserInfoBLL bll = new UserInfoBLL();
                if (bll.GetUserPwdByLoginName(name, pwd, out msg))
                {
                    //登录成功
                    //msgDiv1.MsgDivShow(msg, 1, Bind);
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                }
                else
                {
                    msgDiv1.MsgDivShow(msg, 1);
                }
            }
        }

        private bool CheckEmpty(string name, string pwd)
        {
            if (string.IsNullOrEmpty(name))
            {
               msgDiv1.MsgDivShow("帐号不能为空",1);
                return false;
            }
            if (string.IsNullOrEmpty(pwd))
            {
                msgDiv1.MsgDivShow("密码不能为空", 1);
                return false;
            }
            return true;
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtPwd_TextChanged(object sender, EventArgs e)
        {

        }  
    }
}
