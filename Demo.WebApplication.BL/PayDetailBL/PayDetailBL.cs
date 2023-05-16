using Demo.WebApplication.BL.BaseBL;
using Demo.WebApplication.Common.Entities;
using Demo.WepApplication.DL.PayDetailDL;
using Demo.WepApplication.DL.BaseDL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.WepApplication.DL;
using Npgsql;
using Demo.WebApplication.Common.Entities.DTO;
using Demo.WebApplication.Common.Enums;
using Demo.WebApplication.Common.Resources;
using System.Drawing.Printing;

namespace Demo.WebApplication.BL.PayDetailBL
{
    public class PayDetailBL : BaseBL<PayDetail>, IPayDetailBL
    {
        #region Field
        private IPayDetailDL _payDetailDL;
        #endregion

        #region Constructor
        public PayDetailBL(IPayDetailDL payDetailDL) : base(payDetailDL)
        {
            _payDetailDL = payDetailDL;
        }
        #endregion

        /// <summary>
        /// Lấy ra danh sách chi tiền chi tiết khi
        /// </summary>
        /// <param name="payId">Id của bản ghi chi tiền cha</param>
        /// <returns>Danh sách chi tiền chi tiết</returns>
        /// Author: NVDUC (25/04/2023)
        public ServiceResult GetAllById(Guid payId)
        {
            var records = _payDetailDL.GetAllById(payId);
            // Thành công
            if (records != null)
            {
                return new ServiceResult
                {
                    IsSuccess = true,
                    DevMsg = ContentMessage.S_Get,
                    UserMsg = ContentMessage.S_Get,
                    Data = records,
                };
            }
            // Thất bại
            else
            {
                return new ServiceResult
                {
                    IsSuccess = false,
                    DevMsg = ContentMessage.NotFound,
                    UserMsg = ContentMessage.NotFound,
                    ErrorCode = ErrorCode.NotFound,
                };
            }

        }

        /// <summary>
        /// Xoá nhiều bản ghi theo danh sách id truyền vào
        /// </summary>
        /// <param name="recordList"></param>
        /// <returns>Số lượng Id xoá thành công</returns>
        /// Author: NVDUC (25/3/2023)
        public ServiceResult DeleteMultiple(Guid[] recordList)
        {
            var result = _payDetailDL.DeleteMultiple(recordList);
            if (result > 0)
            {
                return new ServiceResult
                {
                    IsSuccess = true,
                    DevMsg = ContentMessage.S_Delete,
                    UserMsg = ContentMessage.S_Delete,
                    Data = result,
                };
            }
            else
            {
                return new ServiceResult
                {
                    IsSuccess = false,
                    DevMsg = ContentMessage.NotFound,
                    UserMsg = ContentMessage.NotFound,
                    ErrorCode = ErrorCode.NotFound,
                };
            }
        }
    }
}
