using Demo.WebApplication.BL.BaseBL;
using Demo.WebApplication.Common.Entities;
using Demo.WebApplication.Common.Entities.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.WebApplication.BL.PayDetailBL
{
    public interface IPayDetailBL : IBaseBL<PayDetail>
    {
        /// <summary>
        /// Lấy ra danh sách chi tiền chi tiết khi
        /// </summary>
        /// <param name="payId">Id của bản ghi chi tiền cha</param>
        /// <returns>Danh sách chi tiền chi tiết</returns>
        /// Author: NVDUC (25/04/2023)
        public ServiceResult GetAllById(Guid payId);

        /// <summary>
        /// Xoá nhiều bản ghi theo danh sách id truyền vào
        /// </summary>
        /// <param name="recordList"></param>
        /// <returns>Số lượng Id xoá thành công</returns>
        /// Author: NVDUC (25/3/2023)
        public ServiceResult DeleteMultiple(Guid[] recordList);
    }
}
