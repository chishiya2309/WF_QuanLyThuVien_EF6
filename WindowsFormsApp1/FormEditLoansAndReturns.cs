using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BusinessAccessLayer;
using DataAccessLayer;

namespace WindowsFormsApp1
{
    public partial class FormEditLoansAndReturns: Form
    {
        DBLoanAndReturn dblar;
        DataTable dtlar;
        public FormEditLoansAndReturns(DataRow larData)
        {
            InitializeComponent();
            dblar = new DBLoanAndReturn();
            LoadLarData(larData);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void LoadLarData(DataRow larData)
        {
            txtMaPhieu.Text = larData["MaPhieu"].ToString();
            txtMaPhieu.ReadOnly = true;
            dtpNgayMuon.Focus();
            if (larData["NgayMuon"] != DBNull.Value)
            {
                dtpNgayMuon.Value = Convert.ToDateTime(larData["NgayMuon"]);
            }
            if (larData["HanTra"] != DBNull.Value)
            {
                dtpHanTra.Value = Convert.ToDateTime(larData["HanTra"]);
            }

            if (larData["NgayTraThucTe"] != DBNull.Value)
            {
                dtpNgayTraThucTe.Value = Convert.ToDateTime(larData["NgayTraThucTe"]);
                dtpNgayTraThucTe.Checked = true;
            }
            else
            {
                dtpNgayTraThucTe.Value = DateTime.Now;
                dtpNgayTraThucTe.Checked = false;
            }

            string trangThai = larData["TrangThai"].ToString();
            cmbTrangThai.SelectedItem = trangThai;

            nudSoLuong.Value = Convert.ToDecimal(larData["SoLuong"]);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                int maPhieu = Convert.ToInt32(txtMaPhieu.Text.ToString());
                DateTime NgayMuon = dtpNgayMuon.Value;
                DateTime HanTra = dtpHanTra.Value;
                int soLuong = Convert.ToInt32(nudSoLuong.Value);
                string trangThai = cmbTrangThai.SelectedItem.ToString();
                DateTime? ngayTraThucTe = null;
                if (trangThai == "Đang mượn" || trangThai == "Quá hạn")
                {
                    ngayTraThucTe = null;
                }else
                {
                    if (dtpNgayTraThucTe.Checked)
                    {
                        ngayTraThucTe = dtpNgayTraThucTe.Value;
                    }
                }
                

                string err = "";
                bool success = dblar.SuaPhieuMuon(ref err, maPhieu, NgayMuon, HanTra,
                    ngayTraThucTe, trangThai, soLuong);

                if (success)
                {
                    MessageBox.Show("Cập nhật thông tin phiếu mượn thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Lỗi khi cập nhật thông tin phiếu mượn: " + err, "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
