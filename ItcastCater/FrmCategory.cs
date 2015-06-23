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
    public partial class FrmCategory : Form
    {
        public FrmCategory()
        {
            InitializeComponent();
        }

        //窗体加载
        private void FrmCategory_Load(object sender, EventArgs e)
        {
            //显示商品类别
            LoadCategoryInfoByDelFlag(0);

            //显示产品
            LoadProductInfoByDelFlag(0);

            //加载类别
            LoadCategory();
        }

        private void LoadCategory()
        {
            CategoryInfoBLL bll = new CategoryInfoBLL();
            List<CategoryInfo> list = bll.GetAllCategoryInfoByDelFlag(0);
            list.Insert(0, new CategoryInfo(){CatId = -1, CatName = "请选择"});
            cmbCategory.DataSource = list;
            cmbCategory.DisplayMember = "CatName";
            cmbCategory.ValueMember = "CatId";
        }




        //加载商品类别
        private void LoadCategoryInfoByDelFlag(int p)
        {
            CategoryInfoBLL bll = new CategoryInfoBLL();
            dgvCategoryInfo.AutoGenerateColumns = false;//禁止自动生成列
            dgvCategoryInfo.DataSource = bll.GetAllCategoryInfoByDelFlag(p);
            dgvCategoryInfo.SelectedRows[0].Selected = false;//没有被选中的行
        }

        //加载产品
        private void LoadProductInfoByDelFlag(int p)
        {
            ProductInfoBLL bll = new ProductInfoBLL();
            dgvProductInfo.AutoGenerateColumns = false;
            dgvProductInfo.DataSource = bll.GetAllProductInfoByDelFlag(p);
            dgvProductInfo.SelectedRows[0].Selected = false;
        }

        public event EventHandler evtCategory;

        //增加商品类别 1---增加
        private void btnAddCategory_Click(object sender, EventArgs e)
        {
            ShowFrmChangeCategory(1);
        }

        //修改商品类别 2---修改
        private void btnUpdateCategory_Click(object sender, EventArgs e)
        {
            if (dgvCategoryInfo.SelectedRows.Count > 0)
            {
                //获取选中的id
                int id = Convert.ToInt32(dgvCategoryInfo.SelectedRows[0].Cells[0].Value);
                //根据id查询数据
                CategoryInfoBLL bll = new CategoryInfoBLL();
                CategoryInfo cat = bll.GetCategoryInfoByCaitId(id);
                cat.CatId = id;
                //传对象
                fea.obj = cat;
                ShowFrmChangeCategory(2);
            }
            else
            {
                MessageBox.Show("请选中修改的行");
            }
            
        }

        FrmEventAgrs fea = new FrmEventAgrs();
        private void ShowFrmChangeCategory(int p)
        {
            FrmChangeCategory fcc = new FrmChangeCategory();
            this.evtCategory+=new EventHandler(fcc.SetText);
            fea.Temp = p;
            if (true)
            {
                this.evtCategory(this, fea);
            }
            fcc.FormClosed += new FormClosedEventHandler(fcc_FormClosed); ;
            fcc.ShowDialog();
        }

        //刷新
        void fcc_FormClosed(object sender, FormClosedEventArgs e)
        {
          LoadCategoryInfoByDelFlag(0);
        }

        //删除产品
        private void btnDeletePro_Click(object sender, EventArgs e)
        {
            if (dgvProductInfo.SelectedRows.Count > 0)
            {
                //id
                int id = Convert.ToInt32(dgvProductInfo.SelectedRows[0].Cells[0].Value);
                //根据id删除选中的行
                ProductInfoBLL bll = new ProductInfoBLL();
                string msg = bll.DeleteProductById(id) ? "操作成功" : "操作失败";
                MessageBox.Show(msg);
                LoadProductInfoByDelFlag(0);
            }
            else
            {
                MessageBox.Show("请选中删除的行");
            }
        }

        public event EventHandler evtProduct;

        //新增产品 3---新增
        private void btnAddPro_Click(object sender, EventArgs e)
        {
            ShowFrmChangeProduct(3);
        }

        //修改产品 4---修改
        private void btnUpdatePro_Click(object sender, EventArgs e)
        {         
            if (dgvProductInfo.SelectedRows.Count > 0)
            {
                //获取id
                //根据id查询修改行数据是否真的存在---要获取这行数据
                //把对象传到另一个窗体中---对象
                int id = Convert.ToInt32(dgvProductInfo.SelectedRows[0].Cells[0].Value);
                ProductInfoBLL bll = new ProductInfoBLL();
                ProductInfo pro = bll.GetProductInfoByProId(id);
                if (pro != null)
                {
                    fea.obj = pro;
                    ShowFrmChangeProduct(4);
                }
            }
        }

        public void ShowFrmChangeProduct(int p)
        {
            FrmChangeProduct fcp = new FrmChangeProduct();
            this.evtProduct+=new EventHandler(fcp.SetText);
            fea.Temp = p;//传标识
            if (this.evtProduct != null)
            {
                this.evtProduct(this, fea);
            }
            fcp.FormClosed+=new FormClosedEventHandler(fcp_FormClosed);
            fcp.ShowDialog();
        }

        private void fcp_FormClosed(object sender, FormClosedEventArgs e)
        {
           LoadProductInfoByDelFlag(0);
        }

        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbCategory.SelectedIndex == 0)
            {
                LoadProductInfoByDelFlag(0);
            }
            else
            {
                int id = Convert.ToInt32(cmbCategory.SelectedValue);
                ProductInfoBLL bll = new ProductInfoBLL();
                List<ProductInfo> list = bll.GetProductInfoByCatId(id);
                dgvProductInfo.DataSource = list;
                if (list.Count > 0)
                {
                    dgvProductInfo.AutoGenerateColumns = false;                   
                    dgvProductInfo.SelectedRows[0].Selected = false;
                }
               
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            //搜索
            if (!string.IsNullOrEmpty(txtSearch.Text))
            {
                ProductInfoBLL bll = new ProductInfoBLL();
                dgvProductInfo.DataSource = bll.GetProductByProNum(txtSearch.Text);
                dgvProductInfo.AutoGenerateColumns = false;
                //dgvProductInfo.SelectedRows[0].Selected = false;
            }
            else
            {
                LoadProductInfoByDelFlag(0);
            }

        }
    }
}
