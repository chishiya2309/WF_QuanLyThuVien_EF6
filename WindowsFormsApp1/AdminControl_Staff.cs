using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DataAccessLayer;
using BusinessAccessLayer;

namespace WindowsFormsApp1
{
    public partial class AdminControl_Staff: UserControl
    {
        DBStaff dbst;
        DataTable dtNhanVien;
        public AdminControl_Staff()
        {
            InitializeComponent();
            Adjust();
            dbst = new DBStaff();
            LoadData();
        }

        private void staffPanel_Resize(object sender, EventArgs e)
        {
            Adjust();
        }

        private void Adjust()
        {
            searchPanel.Width = staffPanel.Width - 40;
            MenuButton.Width = staffPanel.Width - 40;
            staffGridView.Size = new Size(staffPanel.Width - 40, staffPanel.Height - 270);
            lblNoData.Location = new Point(
       staffGridView.Location.X + (staffGridView.Width - lblNoData.Width) / 2,
      staffGridView.Location.Y + (staffGridView.Height - lblNoData.Height) / 2);
        }

        private void staffGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Kiểm tra xem cột hiện tại có phải là cột "TrangThai" không
            if (staffGridView.Columns[e.ColumnIndex].Name == "TrangThai" && e.Value is string trangThai)
            {
                switch (trangThai)
                {
                    case "Đang làm":
                        e.CellStyle.ForeColor = Color.Green;
                        break;
                    case "Tạm nghỉ":
                        e.CellStyle.ForeColor = Color.Orange; 
                        break;
                    default:
                        e.CellStyle.ForeColor = Color.Black; // Mặc định nếu có giá trị khác
                        break;
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchTerm = txtSearch.Text.Trim();
            LoadStaff(searchTerm);
        }

        private void LoadStaff(string searchTerm)
        {
            try
            {
                dtNhanVien = dbst.LayNhanVien().Tables[0];

                var filteredStaffs = dtNhanVien.AsEnumerable();

                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    searchTerm = searchTerm.ToLower();
                    if (!string.IsNullOrWhiteSpace(searchTerm))
                    {
                        searchTerm = searchTerm.ToLower();
                        filteredStaffs = filteredStaffs.Where(row =>
                            row.Field<string>("ID")?.ToLower().Contains(searchTerm) == true ||
                            row.Field<string>("HoTen")?.ToLower().Contains(searchTerm) == true ||
                            row.Field<string>("SoDienThoai")?.ToLower().Contains(searchTerm) == true ||
                            row.Field<string>("Email")?.ToLower().Contains(searchTerm) == true);
                    }
                }

                // Nếu không có dữ liệu, tạo DataTable rỗng thay vì CopyToDataTable() gây lỗi
                DataTable filteredStaff = filteredStaffs.Any() ? filteredStaffs.CopyToDataTable() : dtNhanVien.Clone();
                // Kiểm tra nếu không có dữ liệu thì ẩn DataGridView, hiển thị Label
                if (filteredStaff.Rows.Count == 0)
                {
                    lblNoData.Visible = true;
                    staffGridView.Visible = false;
                }
                else
                {
                    lblNoData.Visible = false;
                    staffGridView.Visible = true;
                }
                staffGridView.DataSource = filteredStaff;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu danh mục: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadData()
        {
            dtNhanVien = new DataTable();
            dtNhanVien.Clear();
            dtNhanVien = dbst.LayNhanVien().Tables[0];

            // Đưa dữ liệu lên DataGridView  
            staffGridView.DataSource = dtNhanVien;
        }

        private void btnAddStaff_Click(object sender, EventArgs e)
        {
            using (FormAddStaff form = new FormAddStaff())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    // Nếu thêm thành công, cập nhật lại dữ liệu
                    LoadData();
                }
            }    
        }

        private void staffPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnEditStaff_Click(object sender, EventArgs e)
        {
            if (staffGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn nhân viên cần chỉnh sửa thông tin!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                // Lấy dòng đã chọn
                DataGridViewRow selectedRow = staffGridView.SelectedRows[0];

                // Lấy mã sách để tìm kiếm trong DataTable
                string maNhanVien = selectedRow.Cells["ID"].Value.ToString();

                // Tìm dòng dữ liệu trong DataTable
                DataRow[] rows = dtNhanVien.Select($"ID = '{maNhanVien}'");
                if (rows.Length > 0)
                {
                    // Tạo và hiển thị form chỉnh sửa với dữ liệu sách đã chọn
                    FormEditStaff formEditStaff = new FormEditStaff(rows[0]);

                    if (formEditStaff.ShowDialog() == DialogResult.OK)
                    {
                        // Cập nhật dữ liệu trong DataGridView
                        LoadData();
                    }
                }
                else
                {
                    MessageBox.Show("Không tìm thấy thông tin sách!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi chỉnh sửa thông tin sách: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDeleteStaff_Click(object sender, EventArgs e)
        {
            if (staffGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn nhân viên cần xóa!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                // Lấy dòng đã chọn
                DataGridViewRow selectedRow = staffGridView.SelectedRows[0];
                // Lấy mã nhân viên cần xóa
                string maNhanVien = selectedRow.Cells["ID"].Value.ToString();
                string hoTen = selectedRow.Cells["HoTen"].Value.ToString();

                DialogResult result = MessageBox.Show($"Bạn có chắc muốn xóa nhân viên {hoTen} có mã là {maNhanVien}?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    string err = "";
                    // Thực hiện xóa sách từ cơ sở dữ liệu
                    bool success = dbst.XoaNhanVien(ref err, maNhanVien);

                    if (success)
                    {
                        // Cập nhật dữ liệu trong DataGridView sau khi xóa
                        LoadData();

                        MessageBox.Show($"Đã xóa nhân viên '{hoTen}' thành công!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Không thể xóa nhân viên. Nhân viên này có thể đang có ràng buộc dữ liệu khác!", "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa nhân viên: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnSearch_Click(sender, e);
            }
        }
    }
}
