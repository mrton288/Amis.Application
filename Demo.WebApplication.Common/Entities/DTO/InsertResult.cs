using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.WebApplication.Common.Entities.DTO
{
    public class InsertResult
    {
        /// <summary>
        /// Số bản ghi bị ảnh hưởng
        /// </summary>
        /// Created By: NVDUC (14/05/2023)
        public int NumberEffect { get; set; }

        /// <summary>
        /// Id bản ghi thêm mới
        /// </summary>
        /// Created By: NVDUC (13/3/2023)
        public Guid IdInsert { get; set; }
    }
}
