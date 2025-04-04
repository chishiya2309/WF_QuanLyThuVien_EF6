using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BusinessAccessLayer;
using System.Net.NetworkInformation;

namespace WindowsFormsApp1
{
    public partial class AdminControl_Dashboard: UserControl
    {
        private System.Windows.Forms.Timer refreshTimer;
        public AdminControl_Dashboard()
        {
            InitializeComponent();
            statsPanel.Width = dashboardPanel.Width - 40;
            statusPanel.Width = dashboardPanel.Width - 40;

            // Khởi tạo Timer
            refreshTimer = new System.Windows.Forms.Timer();
            refreshTimer.Interval = 60000; // 1 phút
            refreshTimer.Tick += new EventHandler(refreshTimer_Tick);

            // Bật timer
            refreshTimer.Start();

            LoadDashboardData();


        }

        private void refreshTimer_Tick(object sender, EventArgs e)
        {
            LoadDashboardData();
        }

        private void dashboardPanel_Resize(object sender, EventArgs e)
        {
            statsPanel.Width = dashboardPanel.Width - 40;
            statusPanel.Width = dashboardPanel.Width - 40;
        }

        // Phương thức tải dữ liệu cho dashboard
        private void LoadDashboardData()
        {
            try
            {
                // Tạo đối tượng thống kê
                DBThongKe dbThongKe = new DBThongKe();

                // Lấy dữ liệu thống kê dưới dạng DataRow
                DataRow thongKeRow = dbThongKe.LayThongKeTongQuan();

                // Hiển thị dữ liệu trên giao diện
                lblSachKhaDung.Text = thongKeRow["TongSachKhaDung"].ToString();
                lblThanhVien.Text = thongKeRow["TongThanhVien"].ToString();
                lblNhanVien.Text = thongKeRow["TongNhanVien"].ToString();
                lblSachMuonHomNay.Text = thongKeRow["SachMuonHomNay"].ToString();
                lblSachTraHomNay.Text = thongKeRow["SachTraHomNay"].ToString();
                lblSachQuaHan.Text = thongKeRow["SachQuaHan"].ToString();

                // Cập nhật thời gian
                backupLabel.Text = $"Cập nhật cuối: {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}";

                // Hiển thị trạng thái kết nối cơ sở dữ liệu
                databaseLabel.Text = "Cơ sở dữ liệu: Đang hoạt động";
                databaseLabel.ForeColor = Color.Green;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu thống kê: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                // Hiển thị trạng thái lỗi kết nối
                databaseLabel.Text = "Cơ sở dữ liệu: Lỗi kết nối";
                databaseLabel.ForeColor = Color.Red;
            }
        }

        private void dashboardPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dashboardPanel_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                LoadDashboardData();
            }
        }
    }
}
