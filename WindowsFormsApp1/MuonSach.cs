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

namespace WindowsFormsApp1
{
    
    public partial class MuonSach: Form
    {
        DBMembers dBMembers;
        DataTable dtThanhVien;

        DBBooks dBBooks;
        DataTable dtSach;

        DBLoanAndReturn dBLAR;
        DataTable dtLAR;
        public MuonSach()
        {
            InitializeComponent();
            dBMembers = new DBMembers();
            dBBooks = new DBBooks();
            dBLAR = new DBLoanAndReturn();
            LoadData();
        }

        private void LoadData()
        {
            dtThanhVien = new DataTable();
            dtThanhVien.Clear();
            dtThanhVien = dBMembers.LayThanhVien().Tables[0];

            cboThanhVien.DataSource = dtThanhVien;
            cboThanhVien.DisplayMember = "HoTen";
            cboThanhVien.ValueMember = "MaThanhVien";

            dtSach = new DataTable();
            dtSach.Clear();
            dtSach = dBBooks.LaySach().Tables[0];

            cboSach.DataSource = dtSach;
            cboSach.DisplayMember = "TenSach";
            cboSach.ValueMember = "MaSach";

            dtpNgayMuon.Value = DateTime.Now;
            dtpHanTra.Value = DateTime.Now.AddDays(14);
            
        }

        private void MuonSach_Load(object sender, EventArgs e)
        {
            
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string thanhVien = cboThanhVien.SelectedValue.ToString();
                string Sach = cboSach.SelectedValue.ToString();
                int soLuong = Convert.ToInt32(nudSoLuong.Value);
                DateTime ngayMuon = dtpNgayMuon.Value;
                DateTime hanTra = dtpHanTra.Value;

                string err = "";
                bool success = dBLAR.MuonSach(ref err, thanhVien, Sach, soLuong, ngayMuon, hanTra);

                if (success)
                {
                    MessageBox.Show("Ghi nhận lượt mượn sách mới thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Lỗi khi ghi nhận lượt mượn sách: " + err, "Lỗi",
                         MessageBoxButtons.OK, MessageBoxIcon.Error);
                } 
                    
            }catch(Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


    }
}
