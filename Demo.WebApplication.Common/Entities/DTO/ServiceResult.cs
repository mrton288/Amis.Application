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
        /// Thông báo chi tiết
        /// </summary>
        /// Author: NVDUC (25/3/2023)
        public string Message { get; set; }

        /// <summary>
        /// Dữ liệu trả về
        /// </summary>
        /// Author: NVDUC (25/3/2023)
        public object Data { get; set; }
    }
}
