using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text;

namespace 家計簿アプリ2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        DataSet ds;

        Class1 select = new Class1();
        Class1 tran = new Class1();

        /// <summary>
        /// ロード
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            // この設定は無くても良い
            if (Properties.Settings.Default.F1Size.Width == 0) Properties.Settings.Default.Upgrade();
            // もしC#デスクトップアプリをバージョンアップすると、記憶している情報が消え去るが、この↑を
            // 入れておくと引き継がれる（らしい）。



            if (Properties.Settings.Default.F1Size.Width == 0 || Properties.Settings.Default.F1Size.Height == 0)
            {
                // 初回起動時にはここに来るので必要なら初期値を与えても良い。
                // 何も与えない場合には、デザイナーウインドウで指定されている大きさになる。
            }
            else
            {
                this.WindowState = Properties.Settings.Default.F1State;

                // もし前回終了時に最小化されていても、今回起動時にはNormal状態にしておく
                if (this.WindowState == FormWindowState.Minimized) this.WindowState = FormWindowState.Normal;

                this.Location = Properties.Settings.Default.F1Location;
                this.Size = Properties.Settings.Default.F1Size;
            }


            string sql;
            sql = "";
            sql += " select";
            sql += " id,";
            sql += " date as 日付,";
            sql += " category as 分類,";
            sql += " item as 品名,";
            sql += " money as 金額,";
            sql += " emarks as 備考";
            sql += " from mst_household";
            sql += " order by id asc";

            DataTable dt = select.SelectSpl(sql);
            dataGridView1.DataSource = dt;

            DgvDelete();

            AddCmb();

            GetMonth();
        }

        /// <summary>
        /// 追加ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2("");
            f2.ShowDialog();
        }

        /// <summary>
        /// 変更ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void change_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 削除ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 終了ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEnd_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 絞り込みボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSerch_Click(object sender, EventArgs e)
        {
            Ctegory();
        }

        /// <summary>
        /// 集計ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTotal_Click(object sender, EventArgs e)
        {
            string sql;
            sql = "";
            sql += " select";
            sql += " sum(money)";
            sql += " from mst_household";
            sql += " where category = '" + cmbCategory.Text + "'";
            //sql += " order by id asc";

            DataTable dt = select.SelectSpl(sql);

            string row = dt.Rows[0]["sum"].ToString();
            MessageBox.Show(row);
        }

        /// <summary>
        /// CSV読み込みボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCsv_Click(object sender, EventArgs e)
        {
            CsvGet();
        }

        /// <summary>
        /// グリッドダブルクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            // グリッドを基準としたカーソル位置のポイントを取得
            Point p = dataGridView1.PointToClient(Cursor.Position);
            // 取得したポイントからHitTestでセル位置取得
            DataGridView.HitTestInfo ht = dataGridView1.HitTest(p.X, p.Y);

            if (ht.RowIndex >= 0 && ht.ColumnIndex >= 0)
            {
                string id = dataGridView1.Rows[ht.RowIndex].Cells[0].Value.ToString();

                Form2 f2 = new Form2(id);
                f2.ShowDialog();
            }
            //MessageBox.Show("row:" + ht.RowIndex + " col:" + ht.ColumnIndex);
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

        private void Ctegory()
        {
            dataGridView1.Columns.Clear();

            string sql;
            sql = "";
            sql += " select *";
            sql += " from mst_household";
            sql += " where category = '" + cmbCategory.Text + "'";
            sql += " order by id asc";

            DataTable dt = select.SelectSpl(sql);
            dataGridView1.DataSource = dt;

            DgvDelete();
        }

        

        private void Delete(string id)
        {
            string sql;
            sql = "";
            sql += " delete from mst_household where id =" + id;
            
            //sql実行
            tran.TranSpl(sql);

            dataGridView1.Columns.Clear();

            //string sql;
            sql = "";
            sql += " select";
            sql += " id,";
            sql += " date as 日付,";
            sql += " category as 分類,";
            sql += " item as 品名,";
            sql += " money as 金額,";
            sql += " emarks as 備考";
            sql += " from mst_household";
            sql += " order by id asc";

            DataTable dt = select.SelectSpl(sql);
            dataGridView1.DataSource = dt;

            DgvDelete();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            //"Button"列ならば、ボタンがクリックされた
            if (dgv.Columns[e.ColumnIndex].Name == "Button")
            {
                DialogResult dr = MessageBox.Show("本当によろしいですか？", "確認", MessageBoxButtons.YesNo);

                if (dr == System.Windows.Forms.DialogResult.Yes)
                {
                    string id = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();

                    Delete(id);
                    MessageBox.Show("削除しました。");
                }
                else if (dr == System.Windows.Forms.DialogResult.No)
                {
                    MessageBox.Show("Noを押しました。");
                    return;
                }
                else
                {
                    MessageBox.Show("Yes,No以外の動作");
                    return;
                }
            }
        }

        /// <summary>
        /// DGV削除ボタン
        /// </summary>
        private void DgvDelete()
        {
            DataGridViewButtonColumn column = new DataGridViewButtonColumn();
            //列の名前を設定
            column.Name = "Button";
            //全てのボタンに"詳細閲覧"と表示する
            column.UseColumnTextForButtonValue = true;
            column.Text = "削除";
            //DataGridViewに追加する
            dataGridView1.Columns.Add(column);
            ///最終行非表示
            dataGridView1.AllowUserToAddRows = false;
        }

        /// <summary>
        /// ファイル選択
        /// </summary>
        /// <returns></returns>
        private string SelctFile()
        {
            DialogResult ret;
            string fileName;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "ダイアログボックスのサンプル";
                openFileDialog.CheckFileExists = true;

                ret = openFileDialog.ShowDialog();

                fileName = openFileDialog.FileName;
                //txtFile.Text = openFileDialog.SafeFileName;
                //txtFile.Text = fileName;

                return fileName;
            }
        }

        /// <summary>
        /// CSV読み込み
        /// </summary>
        private void CsvGet()
        {
            ///CSVファイル読み込み
            string file = SelctFile();
            // StreamReaderクラスをインスタンス化
            using (StreamReader sr = new StreamReader(file, Encoding.GetEncoding("shift_jis")))
            {
                ///ヘッダー読み込み
                string linebuf = sr.ReadLine();

                while (sr.Peek() >= 0)
                {
                    // 読み込んだ文字列をカンマ区切りで配列に格納
                    string[] cols = sr.ReadLine().Split(',');

                    string date = cols[0];
                    string category = cols[1];
                    string item = cols[2];
                    string money = cols[3];
                    string remarks = cols[4];

                    string sql;
                    sql = "";
                    sql += " insert";
                    sql += " into mst_household";
                    sql += " (date, category, item, money, emarks)";
                    sql += " values";
                    sql += " ('" + date + "', '" + category + "', '" + item + "'," + money + ", '" + remarks + "')";

                    //sql実行
                    tran.TranSpl(sql);
                }
                MessageBox.Show("読み込み完了");
            }
        }

        private void GetMonth()
        {
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "MM/yyyy";

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.F1State = this.WindowState;
            if (this.WindowState == FormWindowState.Normal)
            {
                // ウインドウステートがNormalな場合には位置（location）とサイズ（size）を記憶する。
                Properties.Settings.Default.F1Location = this.Location;
                Properties.Settings.Default.F1Size = this.Size;
            }
            else
            {
                // もし最小化（minimized）や最大化（maximized）の場合には、RestoreBoundsを記憶する。
                Properties.Settings.Default.F1Location = this.RestoreBounds.Location;
                Properties.Settings.Default.F1Size = this.RestoreBounds.Size;
            }

            // ここで設定を保存する
            Properties.Settings.Default.Save();
        }
    }
}
