using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.WebApplication.Common.Entities
{
    public class Pay
    {
        /// <summary>
        /// Id Chi tiền
        /// </summary>
        public Guid pay_id { get; set; }

        /// <summary>
        /// Ngày hạch toán
        /// </summary>
        public DateTime? ref_date { get; set; }

        /// <summary>
        /// Ngày chứng từ
        /// </summary>
        public DateTime? voucher_date { get; set; }

        /// <summary>
        /// Số chứng từ
        /// </summary>
        public string? voucher_number { get; set; }

        /// <summary>
        /// Diễn giải
        /// </summary>
        public string? description { get; set; }

        /// <summary>
        /// Số tiền
        /// </summary>
        public decimal? amount_money { get; set; }

        /// <summary>
        /// Tên đối tượng
        /// </summary>
        public string? object_name { get; set; }

        /// <summary>
        /// Mã đối tượng
        /// </summary>
        public string? object_code { get; set; }

        /// <summary>
        /// Địa chỉ
        /// </summary>
        public string? address { get; set; }

        /// <summary>
        /// Người nhận
        /// </summary>
        public string? receiver { get; set; }

        /// <summary>
        /// Tên nhân viên
        /// </summary>
        public string? employee_name { get; set; }

        /// <summary>
        /// Kèm theo (số lượng)
        /// </summary>
        public int? quantity { get; set; }

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
