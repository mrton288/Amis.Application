using Demo.WebApplication.BL.BaseBL;
using Demo.WebApplication.BL.PayBL;
using Demo.WebApplication.Common.Entities;
using Demo.WebApplication.Common.Entities.DTO;
using Demo.WebApplication.Common.Enums;
using Demo.WebApplication.Common.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Demo.WebApplication.API.Controllers
{
    public class PayController : BaseController<Pay>
    {
        #region Field
        private IPayBL _payBL;
        #endregion

        #region Constructor
        public PayController(IPayBL payBL) : base(payBL)
        {
            _payBL = payBL;
        } 
        #endregion

        /// <summary>
        /// Sinh ra số chứng từ mới
        /// </summary>
        /// <returns>Số chứng từ mới</returns>
        /// Author: NVDUC (04/05/2023)
        [HttpGet("newCode")]
        public string GetNewVoucherCode()
        {
            return _payBL.GetNewVoucherCode();
        }

        /// <summary>
        /// Thực hiện gọi api lấy ra danh sách theo keyword tìm kiếm
        /// </summary>
        /// <param name="search"></param>
        /// <returns>Danh sách</returns>
        /// Author: NVDUC (29/04/2023)
        [HttpGet("getallByKey")]
        public ServiceResult GetAllByKey(string? search)
        {
            try
            {
                return _payBL.GetAllByKey(search);
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
        /// Gọi api xuất excel theo điều kiện tìm kiếm
        /// </summary>
        /// <param name="key"></param>
        /// <returns>File excel chứa dữ liệu theo điều kiện tìm kiếm</returns>
        /// Author: NVDUC (04/05/2023)
        [HttpGet("exportExcel")]
        public async Task<IActionResult> ExportExcelPay(string? key)
        {
            List<Pay> pays = (List<Pay>)_payBL.GetAllByKey(key).Data;
            var stream = await _payBL.ExportExcelPay(pays);
            try
            {
                string excelName = "Danh_sach_chi_tien.xlsx";
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

}
