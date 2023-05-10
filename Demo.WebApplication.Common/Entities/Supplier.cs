using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.WebApplication.Common.Entities
{
    public class Supplier
    {
        /// <summary>
        /// Id Nhà cung cấp
        /// </summary>
        public Guid supplier_id { get; set; }

        /// <summary>
        /// Mã số thuế
        /// </summary>
        public string? tax_code { get; set; }

        /// <summary>
        /// Mã nhà cung cấp
        /// </summary>
        public string? supplier_code { get; set; }

        /// <summary>
        /// Tên nhà cung cấp
        /// </summary>
        public string? supplier_name { get; set; }

        /// <summary>
        /// Địa chỉ
        /// </summary>
        public string? address { get; set; }

        /// <summary>
        /// Số điện thoại
        /// </summary>
        public string? phone_number { get; set; }

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
