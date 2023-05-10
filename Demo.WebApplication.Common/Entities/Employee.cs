using Demo.WebApplication.Common.Attributes;
using Demo.WebApplication.Common.Enums;
using Demo.WebApplication.Common.Resources;
using System.ComponentModel.DataAnnotations;

namespace Demo.WebApplication.Common.Entities
{
    /// <summary>
    /// Thông tin nhân viên
    /// Author: NVDUC (13/3/2023)
    /// </summary>
    public class Employee
    {
        /// <summary>
        /// Khoá chính
        /// </summary>
        public Guid employee_id { get; set; }

        [Required]
        [MaxLength(20)]
        /// <summary>
        /// Mã nhân viên
        /// </summary>
        public string employee_code { get; set; }

        [Required]
        [MaxLength(100)]
        /// <summary>
        /// Tên nhân viên
        /// </summary>
        public string full_name { get; set; }

        /// <summary>
        /// Giới tính
        /// </summary>
        public Gender? gender { get; set; }

        [DateValid]
        /// <summary>
        /// Ngày sinh
        /// </summary>
        public DateTime? date_of_birth { get; set; }

        /// <summary>
        /// Id đơn vị
        /// </summary>
        [Required]
        public Guid? department_id { get; set; }

        /// <summary>
        /// Mã đơn vị
        /// </summary>
        public string? department_code { get; set; }

        /// <summary>
        /// Tên phòng ban
        /// </summary>
        public string? department_name { get; set; }

        /// <summary>
        /// Chức danh
        /// </summary>
        public string? position_name { get; set; }

        /// <summary>
        /// Số chứng minh thư nhân dân
        /// </summary>
        public string? identity_number { get; set; }

        [DateValid]
        /// <summary>
        /// Ngày cấp
        /// </summary>
        public DateTime? identity_date { get; set; }

        /// <summary>
        /// Nơi cấp
        /// </summary>
        public string? identity_place { get; set; }

        /// <summary>
        /// Địa chỉ
        /// </summary>
        public string? address { get; set; }

        /// <summary>
        /// Điện thoại di động
        /// </summary>
        public string? phone_number { get; set; }

        /// <summary>
        /// Điện thoại cố định
        /// </summary>
        public string? landline { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public string? email { get; set; }

        /// <summary>
        /// Tài khoản ngân hàng
        /// </summary>
        public string? bank_account { get; set; }

        /// <summary>
        /// Tên ngân hàng
        /// </summary>
        public string? bank_name { get; set; }

        /// <summary>
        /// Chi nhánh
        /// </summary>
        public string? bank_branch { get; set; }

        /// <summary>
        /// Ngày tạo
        /// </summary>
        public DateTime? created_date { get; set; }

        /// <summary>
        /// Người tạo
        /// </summary>
        public string? created_by { get; set; }

        /// <summary>
        /// Ngày chỉnh sửa
        /// </summary>
        public DateTime? modified_date { get; set; }

        /// <summary>
        /// Người chỉnh sửa
        /// </summary>
        public string? modified_by { get; set; }
    }
}
