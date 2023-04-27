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
        public PayDetailController(IPayDetailBL paydetailBL) : base(paydetailBL)
        {
            _paydetailBL = paydetailBL;
        }
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
                var records = _paydetailBL.GetAllById(payId);
                return new ServiceResult
                {
                    IsSuccess = true,
                    Data = records,
                    DevMsg = ContentMessage.S_Get,
                    UserMsg = ContentMessage.S_Get,
                };
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

    }
}
