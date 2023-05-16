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
            var stream = await _payBL.ExportExcelPay(key);
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

        /// <summary>
        /// Thực hiện xoá bản ghi ở Table Pay đồng thời 
        /// lấy ra và xoá bản ghi ở table Paydetail
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        /// Author: NVDUC (11/05/2023)
        [HttpDelete("deleteFullMultiple")]
        public ServiceResult DeleteFullMultiple([FromBody] Guid[] ids)
        {
            try
            {
                return _payBL.DeleteFullMultiple(ids);
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
        /// Thực hiện xoá bản ghi ở Table Pay đồng thời 
        /// lấy ra và xoá bản ghi ở table Paydetail
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// Author: NVDUC (11/05/2023)
        [HttpDelete("deleteMultiple")]
        public ServiceResult DeleteMultiple([FromQuery]Guid id)
        {
            try
            {
                return _payBL.DeleteMultiple(id);
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
