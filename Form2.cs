using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 家計簿アプリ2
{
    public partial class Form2 : Form
    {
        DataSet ds;

        Class1 select = new Class1();
        Class1 tran = new Class1();

        string _id = string.Empty;
        

        public Form2(string id)
        {
            InitializeComponent();

            if (id == string.Empty)
            {
                lblTitle.Text = "追加";
            }
            else
            {
                lblTitle.Text = "更新";
                _id = id;
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            AddCmb();
            SetData();
        }

        /// <summary>
        /// 登録ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOk_Click(object sender, EventArgs e)
        {
            if (lblTitle.Text.Equals("追加"))
            {
                Add();
            }
            else if(lblTitle.Text.Equals("更新"))
            {
                Up();
            }
            
        }

        /// <summary>
        /// キャンセルボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 追加
        /// </summary>
        private void Add()
        {
            string sql;
            sql = "";
            sql += " insert";
            sql += " into mst_household";
            sql += " (date, category, item, money, emarks)";
            sql += " values";
            sql += " ('" + monCalendar.SelectionStart + "', '" + cmbCategory.Text + "', '" + txtItem.Text + "'," +  mtxtMoney.Text + ", '" + txtRemarks.Text + "')";

            //sql実行
            tran.TranSpl(sql);

            MessageBox.Show("登録完了");
            this.Close();

        }

        private void Up()
        {
            string sql;
            sql = "";
            sql += " update";
            sql += " mst_household";
            sql += " set";
            sql += " date = '" + monCalendar.SelectionStart + "',";
            sql += " category = '" + cmbCategory.Text + "',";
            sql += " item = '" + txtItem.Text + "',";
            sql += " money =" + mtxtMoney.Text;
            sql += ", emarks = '" + txtRemarks.Text + "'";
            sql += " where id =" + _id;

            //sql実行
            tran.TranSpl(sql);

            MessageBox.Show("更新完了");
            this.Close();

        }

        /// <summary>
        /// コンボ追加
        /// </summary>
        private void AddCmb()
        {
            string sql;
            sql = "";
            sql += " select";
            sql += " id, category_name";
            sql += " from mst_category";
            sql += " order by id asc";

            DataTable dt = select.SelectSpl(sql);

            DataTable dtCombo = new DataTable();
            dtCombo.Columns.Add("category_id");
            dtCombo.Columns.Add("category_name");

            DataRow dtRowCombo;

            foreach (DataRow row in dt.Rows)
            {
                dtRowCombo = dtCombo.NewRow();

                dtRowCombo["category_id"] = row["id"];
                dtRowCombo["category_name"] = row["category_name"];
                dtCombo.Rows.Add(dtRowCombo);
            }
            cmbCategory.DataSource = dtCombo;
            cmbCategory.DisplayMember = ("category_name");
            cmbCategory.ValueMember = ("category_id");
        }

        /// <summary>
        /// データ取得
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private DataTable GetData(string id)
        {
            string sql;
            sql = "";
            sql += " select *";
            sql += " from mst_household";
            sql += " where id =" + id;

            DataTable dt = select.SelectSpl(sql);

            return dt;
        }

        /// <summary>
        /// データセット
        /// </summary>
        private void SetData()
        {
            if(_id == string.Empty)
            {
                txtItem.Text = "";
                mtxtMoney.Text = "";
                txtRemarks.Text = "";
            }
            else
            {
                DataTable dt = GetData(_id);

                cmbCategory.Text = dt.Rows[0]["category"].ToString();
                txtItem.Text = dt.Rows[0]["item"].ToString();
                mtxtMoney.Text = dt.Rows[0]["money"].ToString();
                txtRemarks.Text = dt.Rows[0]["emarks"].ToString();
            }
        }
    }
}
