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
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        private void btnMember_Click(object sender, EventArgs e)
        {
            FrmMemberInfo fm = new FrmMemberInfo();
            fm.ShowDialog();
        }

        private void btnRoom_Click(object sender, EventArgs e)
        {
            FrmRoom fr = new FrmRoom();
            fr.ShowDialog();
        }

        private void btnCategory_Click(object sender, EventArgs e)
        {
            FrmCategory fc = new FrmCategory();
            fc.ShowDialog();
        }

        //主界面加载
        private void FrmMain_Load(object sender, EventArgs e)
        {
            //加载房间
            LoadRoomInfoByDelFlag(0);

            //加载餐桌
            TabPage tp = tcin.TabPages[0];
            //属性值更改时发生
            tcin.SelectedIndexChanged += new EventHandler(tcin_SelectedIndexChanged);
            LoadDeskInfoByTabPage(tp);

        }

        private void tcin_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDeskInfoByTabPage(tcin.TabPages[tcin.SelectedIndex]);
        }

        //加载房间
        private void LoadRoomInfoByDelFlag(int p)
        {
            RoomInfoBLL bll = new RoomInfoBLL();
            List<RoomInfo> listRoom = bll.GetAllRoomInfoByDelFlag(0);
            for (int i = listRoom.Count - 1; i >= 0; i--)
            {
                TabPage tp = new TabPage();
                tp.Tag = listRoom[i];//存房间的对象
                tp.Text = listRoom[i].RoomName;//显示房间的名字
                ListView lv = new ListView();
                lv.LargeImageList = imageList1;//给listview控件绑定图片集合
                lv.BackColor = Color.White;//设置背景颜色
                lv.Dock = DockStyle.Fill;//让listview在父容器中填充
                lv.MultiSelect = false;//只能选中一个
                lv.View = View.LargeIcon;


                tp.Controls.Add(lv);
                tcin.TabPages.Add(tp);
            }
        }

        //加载餐桌
        private void LoadDeskInfoByTabPage(TabPage tp)
        {
            //获取房间的id
            RoomInfo room = tp.Tag as RoomInfo;          
            DeskInfoBLL bll = new DeskInfoBLL();
            List<DeskInfo> listDesk = bll.GetDeskInfoByRoomId(Convert.ToInt32(room.RoomId));
            ListView lv = tp.Controls[0] as ListView;
            lv.Clear();
            for (int i = 0; i < listDesk.Count; i++)
            {
                //判断餐桌状态显示对象的图片
                lv.Items.Add(listDesk[i].DeskName, Convert.ToInt32(listDesk[i].DeskState));
                lv.Items[i].Tag = listDesk[i];//餐桌对象
            }
        }

        public event EventHandler evtBill;//开单的事件
        //顾客开单   
        private void btnFrmBilling_Click(object sender, EventArgs e)
        {
            //获取当前选中的选项卡
            TabPage tp = tcin.SelectedTab;
            //获取当前选中的房间名字
            RoomInfo room = tp.Tag as RoomInfo;

            //最低消费
            FrmEventAgrs fea = new FrmEventAgrs();
            fea.Money = Convert.ToDecimal(room.RoomMinimunConsume);//最低消费
            fea.Name = room.RoomName;//房间名字

            //获取当前选项卡中的listview控件
            ListView lv = tp.Controls[0] as ListView;
                    
            //判断是否有选中的餐桌
            if (lv.SelectedItems.Count > 0)
            {
                //获取当前选中的餐桌
                DeskInfo dk = lv.SelectedItems[0].Tag as DeskInfo;
                if (dk.DeskState == 0)
                {
                    fea.obj = dk;//餐桌对象
                    FrmBilling fbi = new FrmBilling();
                    this.evtBill += new EventHandler(fbi.SetText);//注册事件
                    if (this.evtBill != null)
                    {
                        this.evtBill(this, fea);
                    }
                    fbi.FormClosed += new FormClosedEventHandler(fbi_FormClosed);
                    fbi.ShowDialog();
                }
                else
                {
                    MessageBox.Show("请选择未开单的餐桌");
                }             
            }
            else
            {
                MessageBox.Show("请选中餐桌");
            }

        }

        //开单窗体关闭后刷新
        private void fbi_FormClosed(object sender, FormClosedEventArgs e)
        {
          LoadDeskInfoByTabPage(tcin.SelectedTab);
        }

        public event EventHandler evtAddMoney;

        private void btnAddMoney_Click(object sender, EventArgs e)
        {
            //获取当前选中的选项卡
            TabPage tp = tcin.TabPages[tcin.SelectedIndex];
            //获取当前选中房间的名字
            RoomInfo room = tp.Tag as RoomInfo;
            FrmEventAgrs fea = new FrmEventAgrs();
            fea.Money = Convert.ToDecimal(room.RoomMinimunConsume);//最低消费
            fea.Name = room.RoomName;//房间的名字
            //最低消费--坑
            //获取当前选项卡中的listview控件
            ListView lv = tp.Controls[0] as ListView;
            //判断是否有选中的餐桌
            if (lv.SelectedItems.Count > 0)
            {
                //获取当前选中的餐桌
                DeskInfo dk = lv.SelectedItems[0].Tag as DeskInfo;
                if (dk.DeskState == 1)
                {
                    fea.Name = dk.DeskName;//餐桌的编号
                    //订单的id,根据餐桌的id查找订单的id
                    OrderInfoBLL obll = new OrderInfoBLL();
                    int orderId = obll.GetOrderIdByDeskId(dk.DeskId);
                    fea.Temp = orderId;//订单的id
                    //好大的一个坑,还没传值呢
                    FrmAddMoney fam = new FrmAddMoney();
                    this.evtAddMoney += new EventHandler(fam.SetText);//注册事件
                    if (this.evtAddMoney != null)
                    {
                        this.evtAddMoney(this, fea);
                    }
                    fam.FormClosed += new FormClosedEventHandler(fbi_FormClosed);
                    fam.ShowDialog();
                }
                else
                {
                    MessageBox.Show("请选择开单的餐桌");
                }
            }
            else
            {
                MessageBox.Show("请选择餐桌");
            }
        }
    }
}
