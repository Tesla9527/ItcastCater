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
    public partial class FrmChangeRoom : Form
    {
        public FrmChangeRoom()
        {
            InitializeComponent();
        }

        private int Tp { get; set; }

        public void SetText(object sender, EventArgs e)
        {
            //第一清空所有文本框

            foreach (Control item in this.Controls)
            {
                if (item is TextBox)
                {
                    TextBox tb = item as TextBox;
                    tb.Text = "";
                }
            }

            //获取传过来的值
            FrmEventAgrs fea = e as FrmEventAgrs;
            this.Tp = fea.Temp;//把标识存起来
            if (fea.Temp == 2)
            {
                RoomInfo room = fea.obj as RoomInfo;
                labId.Text = room.RoomId.ToString();//id存起来
                txtIsDeflaut.Text = room.IsDefault.ToString();
                txtRMinMoney.Text = room.RoomMinimunConsume.ToString();
                txtRName.Text = room.RoomName;
                txtRPerNum.Text = room.RoomMaxConsumer.ToString();
                txtRType.Text = room.RoomType.ToString();
            }

        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            //判断每个文本框不能为空
            if (CheckEmpty())
            {
                RoomInfo room = new RoomInfo();
                room.IsDefault = Convert.ToInt32(txtIsDeflaut.Text);
                room.RoomMaxConsumer = Convert.ToInt32(txtRPerNum.Text);
                room.RoomMinimunConsume = Convert.ToDecimal(txtRMinMoney.Text);
                room.RoomName = txtRName.Text;
                room.RoomType = Convert.ToInt32(txtRType.Text);

                if (this.Tp == 1)//新增
                {
                    room.DelFlag = 0;
                    room.SubBy = 1;
                    room.SubTime = System.DateTime.Now;
                }
                else if (this.Tp == 2)//修改
                {
                    room.RoomId = Convert.ToInt32(labId.Text);
                }
                RoomInfoBLL bll = new RoomInfoBLL();
                string msg = bll.SaveRoom(room, this.Tp) ? "操作成功" : "操作失败";
                MessageBox.Show(msg);
                this.Close();
            }
        }

        //每个文本框不能为空
        private bool CheckEmpty()
        {
            if (string.IsNullOrEmpty(txtIsDeflaut.Text))
            {
                MessageBox.Show("默认编号不能为空");
                return false;
            }
            if (string.IsNullOrEmpty(txtRMinMoney.Text))
            {
                MessageBox.Show("最低消费不能为空");
                return false;
            }
            if (string.IsNullOrEmpty(txtRName.Text))
            {
                MessageBox.Show("房间的编号不能为空");
                return false;
            }
            if (string.IsNullOrEmpty(txtRPerNum.Text))
            {
                MessageBox.Show("容纳的人数不能为空");
                return false;
            }
            if (string.IsNullOrEmpty(txtRType.Text))
            {
                MessageBox.Show("类型不能为空");
                return false;
            }
            return true;
        }
    }
}
