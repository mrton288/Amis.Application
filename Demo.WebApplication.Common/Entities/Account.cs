using Demo.WebApplication.Common.Enums.Account;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.WebApplication.Common.Entities
{
    public class Account
    {
        /// <summary>
        /// Id tài khoản
        /// </summary>
        public Guid account_id { get; set; }

        /// <summary>
        /// Số tài khoản
        /// </summary>
        [Required]
        [MaxLength(20)]
        public string account_number { get; set; }

        /// <summary>
        /// Tên tài khoản
        /// </summary>
        [Required]
        [MaxLength(255)]
        public string account_name { get; set; }

        /// <summary>
        /// Tên tài khoản
        /// </summary>
        [MaxLength(255)]
        public string account_english_name { get; set; }

        /// <summary>
        /// Id tài khoản cha
        /// </summary>
        public Guid? parent_id { get; set; }

        /// <summary>
        /// Kiểm tra trạng thái tài khoản có phải cha không
        /// </summary>
        public int? is_parent { get; set; }

        /// <summary>
        /// Tính chất
        /// </summary>
        [Required]
        public int? property{ get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        [MaxLength(255)]
        public string? description { get; set; }

        /// <summary>
        /// Trạng thái
        /// </summary>
        public int? status { get; set; }

        /// <summary>
        /// Có hạch toán ngoại lệ
        /// </summary>
        public int? is_postable_in_foreign_currency { get; set; }

        /// <summary>
        /// Đối tượng
        /// </summary>
        public int? detail_by_account_object { get; set; }

        /// <summary>
        /// Loại đối tượng
        /// </summary>
        public int? account_object_type { get; set; }

        /// <summary>
        /// Tài khoản ngân hàng
        /// </summary>
        public int? detail_by_bank_account { get; set; }

        /// <summary>
        /// Hợp đồng bán
        /// </summary>
        public int? detail_by_contract { get; set; }

        /// <summary>
        /// Loại Hợp đồng bán
        /// </summary>
        public int? detail_by_contract_kind { get; set; }

        /// <summary>
        /// Đơn vị
        /// </summary>
        public int? detail_by_department { get; set; }

        /// <summary>
        /// Loại Đơn vị
        /// </summary>
        public int? detail_by_department_kind { get; set; }

        /// <summary>
        /// Khoản mục CP
        /// </summary>
        public int? detail_by_expense_item { get; set; }

        /// <summary>
        /// Loại Khoản mục CP
        /// </summary>
        public int? detail_by_expense_item_kind { get; set; }

        /// <summary>
        /// Đối tượng THCP
        /// </summary>
        public int? detail_by_job { get; set; }

        /// <summary>
        /// Loại Đối tượng THCP
        /// </summary>
        public int? detail_by_job_kind { get; set; }

        /// <summary>
        /// Mã thống kê
        /// </summary>
        public int? detail_by_list_item { get; set; }

        /// <summary>
        /// Loại Mã thống kê
        /// </summary>
        public int? detail_by_list_item_kind { get; set; }

        /// <summary>
        /// Đơn đặt hàng
        /// </summary>
        public int? detail_by_order { get; set; }

        /// <summary>
        /// Loại Đơn đặt hàng
        /// </summary>
        public int? detail_by_order_kind { get; set; }

        /// <summary>
        /// Công trình
        /// </summary>
        public int? detail_by_project_work { get; set; }

        /// <summary>
        /// Loại công trình
        /// </summary>
        public int? detail_by_project_work_kind { get; set; }

        /// <summary>
        /// Hợp đồng mua
        /// </summary>
        public int? detail_by_pu_contract { get; set; }

        /// <summary>
        /// Loại Hợp đồng mua
        /// </summary>
        public int? detail_by_pu_contract_kind { get; set; }

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
