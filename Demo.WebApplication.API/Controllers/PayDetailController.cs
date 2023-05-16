using Demo.WebApplication.BL.BaseBL;
using Demo.WebApplication.BL.PayDetailBL;
using Demo.WebApplication.Common.Entities;
using Demo.WebApplication.Common.Entities.DTO;
using Demo.WebApplication.Common.Enums;
using Demo.WebApplication.Common.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Demo.WebApplication.API.Controllers
{
    public class PayDetailController : BaseController<PayDetail>
    {
        #region Field
        private IPayDetailBL _paydetailBL;
        #endregion

        #region Constructor
        public PayDetailController(IPayDetailBL paydetailBL) : base(paydetailBL)
        {
            _paydetailBL = paydetailBL;
        }
        #endregion

        #region Method
        /// <summary>
        /// Xoá một bản ghi theo Id
        /// </summary>
        /// <param name="recordId"></param>
        /// <returns>Thông báo kết quả xoá</returns>
        /// Created By : NVDUC (13/3/2023)
        [HttpGet("listPayDetail")]
        public ServiceResult GetAllById(Guid payId)
        {
            try
            {
                return _paydetailBL.GetAllById(payId);
            }
            catch
            {
                return new ServiceResult
                {
                    IsSuccess = false,
                    DevMsg = ContentMessage.Exception,
                    UserMsg = ContentMessage.Exception,
                    ErrorCode = ErrorCode.Exception,
                };
            }
        }

        /// <summary>
        /// Xoá nhiều bản ghi theo danh sách id truyền vào
        /// </summary>
        /// <param name="recordList"></param>
        /// <returns>Số lượng Id xoá thành công</returns>
        /// Author: NVDUC (25/3/2023)
        [HttpDelete]
        public ServiceResult DeleteMultiple([FromBody]Guid[] recordList)
        {
            try
            {
                return _paydetailBL.DeleteMultiple(recordList);
            }
            catch
            {
                return new ServiceResult
                {
                    IsSuccess = false,
                    DevMsg = ContentMessage.Exception,
                    UserMsg = ContentMessage.Exception,
                    ErrorCode = ErrorCode.Exception,
                };
            }
        }
        #endregion

    }
}
