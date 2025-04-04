using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using BusinessAccessLayer;
using DataAccessLayer;

namespace WindowsFormsApp1
{
    public partial class AdminControl_Member: UserControl
    {
        DBMembers dbtv;
        // Đối tượng đưa dữ liệu vào DataTable dtDanhMuc
        DataTable dtThanhVien = null;
        public AdminControl_Member()
        {
            InitializeComponent();
            Adjust();
            dbtv = new DBMembers();
            LoadData();
        }

        private void btnAddMember_Click(object sender, EventArgs e)
        {
            using(FormAddMember form = new FormAddMember())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    // Nếu thêm thành công, cập nhật lại dữ liệu
                    LoadData();
                }
            }    
        }


        private void btnPrintCards_Click(object sender, EventArgs e)
        {
            PrintMemberCards();
        }

        private void PrintMemberCards()
        {
            MessageBox.Show("Chức năng in thẻ thành viên vẫn đang trong quá trình phát triển. Xin lỗi vì sự bất tiện này.", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Adjust()
        {
            searchPanel.Width = panel.Width - 40;
            MenuButton.Width = panel.Width - 40;
            membersGridView.Size = new Size(panel.Width - 40, panel.Height - 280);
            lblNoData.Location = new Point(
       membersGridView.Location.X + (membersGridView.Width - lblNoData.Width) / 2,
      membersGridView.Location.Y + (membersGridView.Height - lblNoData.Height) / 2);
        }

        private void panel_Resize(object sender, EventArgs e)
        {
            Adjust();
        }

        private void xemChiTiếtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ViewMemberDetails();
        }

        private void ViewMemberDetails()
        {
            MessageBox.Show("Chức năng xem chi tiết thành viên sẽ được triển khai sau.", "Thông báo",
               MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void chỉnhSửaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditSelectedMember();
        }

        private void EditSelectedMember()
        {
            MessageBox.Show("Chức năng chỉnh sửa thành viên sẽ được triển khai sau.", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void xóaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteSelectedMember();
        }

        private void DeleteSelectedMember()
        {
            DialogResult result = MessageBox.Show("Bạn có chắc muốn xóa thành viên này?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                MessageBox.Show("Chức năng xóa thành viên sẽ được triển khai sau.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void giaHạnThẻToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RenewMembership();
        }

        private void RenewMembership()
        {
            MessageBox.Show("Chức năng gia hạn thẻ thành viên sẽ được triển khai sau.", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void sáchĐangMượnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ViewMemberLoans();
        }

        private void ViewMemberLoans()
        {
            MessageBox.Show("Chức năng xem sách đang mượn sẽ được triển khai sau.", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void inThẻThànhViênToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PrintMemberCard();
        }

        private void PrintMemberCard()
        {
            MessageBox.Show("Chức năng in thẻ thành viên sẽ được triển khai sau.", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void membersGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            ViewMemberDetails();
        }

        private void LoadData()
        {
            dtThanhVien = new DataTable();
            dtThanhVien.Clear();
            dtThanhVien = dbtv.LayThanhVien().Tables[0];

            // Đưa dữ liệu lên DataGridView  
            membersGridView.DataSource = dtThanhVien;
        }

        private void panel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void membersGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Kiểm tra xem cột hiện tại có phải là cột "TrangThai" không
            if (membersGridView.Columns[e.ColumnIndex].Name == "TrangThai" && e.Value is string trangThai)
            {
                switch (trangThai)
                {
                    case "Hoạt động":
                        e.CellStyle.ForeColor = Color.Green;
                        break;
                    case "Hết hạn":
                        e.CellStyle.ForeColor = Color.Orange; // Đổi màu từ Yellow sang Orange để dễ nhìn hơn
                        break;
                    case "Khóa":
                        e.CellStyle.ForeColor = Color.Red;
                        break;
                    default:
                        e.CellStyle.ForeColor = Color.Black; // Mặc định nếu có giá trị khác
                        break;
                }
            }
        }

        private void btnEditMember_Click(object sender, EventArgs e)
        {
            if(membersGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn một thành viên để chỉnh sửa", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Lấy dòng đã chọn
            DataGridViewRow row = membersGridView.SelectedRows[0];

            // Lấy MaThanhVien từ dòng đã chọn
            string maThanhVien = row.Cells["MaThanhVien"].Value.ToString();

            // Mở form chỉnh sửa với thông tin thành viên đã chọn
            using (FormEditMember form = new FormEditMember(maThanhVien))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    // Nếu chỉnh sửa thành công, cập nhật lại dữ liệu
                    LoadData();
                }
            }


        }

        private void btnDeleteMember_Click(object sender, EventArgs e)
        {
            if(membersGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn thành viên cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }


            try
            {
                // Lấy dòng đã chọn
                DataGridViewRow selectedRow = membersGridView.SelectedRows[0];
                // Lấy mã thành viên cần xóa
                string maThanhVien = selectedRow.Cells["MaThanhVien"].Value.ToString();
                string tenThanhVien = selectedRow.Cells["HoTen"].Value.ToString();

                DialogResult result = MessageBox.Show($"Bạn có chắc muốn xóa thành viên '{tenThanhVien}'?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    string err = "";
                    // Thực hiện xóa thành viên từ cơ sở dữ liệu
                    bool success = dbtv.XoaThanhVien(ref err, maThanhVien);

                    if (success)
                    {
                        // Cập nhật dữ liệu trong DataGridView sau khi xóa
                        LoadData();

                        MessageBox.Show($"Đã xóa thành viên '{tenThanhVien}' thành công!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Không thể xóa thành viên này. Thành viên này có thể đang mượn sách hoặc ràng buộc khác!", "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa thành viên: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchTerm = txtSearch.Text.Trim();
            LoadMember(searchTerm);
        }

        private void LoadMember(string searchTerm)
        {
            try
            {
                dtThanhVien = dbtv.LayThanhVien().Tables[0];

                var filteredMembers = dtThanhVien.AsEnumerable();

                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    searchTerm = searchTerm.ToLower();
                    if (!string.IsNullOrWhiteSpace(searchTerm))
                    {
                        searchTerm = searchTerm.ToLower();
                        filteredMembers = filteredMembers.Where(row =>
                            row.Field<string>("MaThanhVien")?.ToLower().Contains(searchTerm) == true ||
                            row.Field<string>("HoTen")?.ToLower().Contains(searchTerm) == true ||
                            row.Field<string>("SoDienThoai")?.ToLower().Contains(searchTerm) == true ||
                            row.Field<string>("Email")?.ToLower().Contains(searchTerm) == true);
                    }
                }

                // Nếu không có dữ liệu, tạo DataTable rỗng thay vì CopyToDataTable() gây lỗi
                DataTable filteredMember = filteredMembers.Any() ? filteredMembers.CopyToDataTable() : dtThanhVien.Clone();

                // Kiểm tra nếu không có dữ liệu thì ẩn DataGridView, hiển thị Label
                if (filteredMember.Rows.Count == 0)
                {
                    lblNoData.Visible = true;
                    membersGridView.Visible = false;
                }
                else
                {
                    lblNoData.Visible = false;
                    membersGridView.Visible = true;
                }
                membersGridView.DataSource = filteredMember;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu danh mục: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {

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
