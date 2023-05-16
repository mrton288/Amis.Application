using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.WebApplication.Common.Entities
{
    public class PayDetail
    {
        /// <summary>
        /// Id chi tiền chi tiết
        /// </summary>
        public Guid paydetail_id { get; set; }

        /// <summary>
        /// Id chi tiền
        /// </summary>
        public Guid? pay_id { get; set; }

        /// <summary>
        /// Diễn giải
        /// </summary>
        public string? description { get; set; }

        /// <summary>
        /// Tài khoản nợ
        /// </summary>
        public string debit_account { get; set; }

        /// <summary>
        /// Tài khoản có
        /// </summary>
        public string credit_account { get; set; }

        /// <summary>
        /// Số tiền
        /// </summary>
        public decimal? amount_money { get; set; }

        /// <summary>
        /// Id đối tượng
        /// </summary>
        public Guid? object_id { get; set; }

        /// <summary>
        /// Tên đối tượng
        /// </summary>
        public string? object_code { get; set; }

        /// <summary>
        /// Tên đối tượng
        /// </summary>
        public string? object_name { get; set; }

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
