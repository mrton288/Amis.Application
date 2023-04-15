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
        public Guid  EmployeeId { get; set; }

        [Required]
        [MaxLength(20)]
        /// <summary>
        /// Mã nhân viên
        /// </summary>
        public string EmployeeCode { get; set; }

        [Required]
        [MaxLength(100)]
        /// <summary>
        /// Tên nhân viên
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Giới tính
        /// </summary>
        public Gender? Gender { get; set; }

        [DateValid]
        /// <summary>
        /// Ngày sinh
        /// </summary>
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// Id đơn vị
        /// </summary>
        [Required]
        public Guid? DepartmentId { get; set; }

        /// <summary>
        /// Mã đơn vị
        /// </summary>
        public string? DepartmentCode { get; set; }

        /// <summary>
        /// Tên phòng ban
        /// </summary>
        public string? DepartmentName { get; set; }

        /// <summary>
        /// Chức danh
        /// </summary>
        public string? PositionName { get; set; }

        /// <summary>
        /// Số chứng minh thư nhân dân
        /// </summary>
        public string? IdentityNumber { get; set; }

        [DateValid]
        /// <summary>
        /// Ngày cấp
        /// </summary>
        public DateTime? IdentityDate { get; set; }

        /// <summary>
        /// Nơi cấp
        /// </summary>
        public string? IdentityPlace { get; set; }

        /// <summary>
        /// Địa chỉ
        /// </summary>
        public string? Address { get; set; }

        /// <summary>
        /// Điện thoại di động
        /// </summary>
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// Điện thoại cố định
        /// </summary>
        public string? Landline { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Tài khoản ngân hàng
        /// </summary>
        public string? BankAccount { get; set; }

        /// <summary>
        /// Tên ngân hàng
        /// </summary>
        public string? BankName { get; set; }

        /// <summary>
        /// Chi nhánh
        /// </summary>
        public string? BankBranch { get; set; }

        /// <summary>
        /// Ngày tạo
        /// </summary>
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Người tạo
        /// </summary>
        public string? CreatedBy { get; set; }

        /// <summary>
        /// Ngày sửa
        /// </summary>
        public DateTime? ModifiedDate {get; set; }

        /// <summary>
        /// Người sửa
        /// </summary>
        public string? ModifiedBy { get; set; }
    }
}
