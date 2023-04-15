using Demo.WebApplication.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.WebApplication.Common.Entities.DTO
{
    public class ServiceResult
    {
        /// <summary>
        /// Trả về trạng thái có thành công hay không
        /// </summary>
        /// Author: NVDUC (25/3/2023)
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Trả về thông tin lỗi cho devloper
        /// </summary>
        public String DevMsg { get; set; }

        /// <summary>
        /// Trả về thông tin lỗi cho người dùng
        /// </summary>
        public String UserMsg { get; set; }

        /// <summary>
        /// Trả về mã lỗi
        /// </summary>
        public ErrorCode ErrorCode { get; set; }

        /// <summary>
        /// Dữ liệu trả về
        /// </summary>
        /// Author: NVDUC (25/3/2023)
        public object Data { get; set; }
    }
}
