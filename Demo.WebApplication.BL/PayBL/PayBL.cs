using Demo.WebApplication.BL.BaseBL;
using Demo.WebApplication.Common.Entities;
using Demo.WebApplication.Common.Entities.DTO;
using Demo.WebApplication.Common.Enums;
using Demo.WebApplication.Common.Resources;
using Demo.WepApplication.DL.BaseDL;
using Demo.WepApplication.DL.PayDL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.WebApplication.BL.PayBL
{
    public class PayBL : BaseBL<Pay>, IPayBL
    {
        #region Field
        private IPayDL _payDL; 
        #endregion

        #region Constructor
        public PayBL(IPayDL payDL) : base(payDL)
        {
            _payDL = payDL;
        }
        #endregion

        /// <summary>
        /// Sinh ra số chứng từ mới
        /// </summary>
        /// <returns>Số chứng từ mới</returns>
        /// Author: NVDUC (04/05/2023)
        public string GetNewVoucherCode()
        {
            return _payDL.GetNewVoucherCode();
        }


        /// <summary>
        /// Thực hiện lấy ra danh sách theo keyword tìm kiếm
        /// </summary>
        /// <param name="search"></param>
        /// <returns>Danh sách</returns>
        /// Author: NVDUC (29/04/2023)
        public ServiceResult GetAllByKey(string? search)
        {
            var records = _payDL.GetAllByKey(search);
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
        /// Thực hiện chức năng xuất excel theo điều kiện tìm kiếm
        /// </summary>
        /// <param name="pays"></param>
        /// <returns>File excel chứa dữ liệu theo điều kiện tìm kiếm</returns>
        /// Author: NVDUC (04/05/2023)
        public Task<MemoryStream> ExportExcelPay(string? search)
        {
            return _payDL.ExportExcelPay(search);
        }

        /// <summary>
        /// Thực hiện xoá bản ghi ở Table Pay đồng thời 
        /// lấy ra và xoá bản ghi ở table Paydetail
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        /// Author: NVDUC (11/05/2023)
        public ServiceResult DeleteFullMultiple(Guid[]? ids)
        {
            var result = _payDL.DeleteFullMultiple(ids);
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

        /// <summary>
        /// Thực hiện xoá bản ghi ở Table Pay đồng thời 
        /// lấy ra và xoá bản ghi ở table Paydetail
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// Author: NVDUC (11/05/2023)
        public ServiceResult DeleteMultiple(Guid id)
        {
            var result = _payDL.DeleteMultiple(id);
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

        /// <summary>
        /// Kiểm tra các trường dữ liệu của Pay
        /// </summary>
        /// <param name="pay"></param>
        /// <returns></returns>
        /// Author: NVDUC (24/4/2023)
        public override List<string> ValidateRequestDataCustom(Pay pay)
        {
            // Mảng danh sách lỗi
            var validateFailuresPay = new List<string>();
            if (_payDL.CheckDuplicateVoucherNumber(pay.voucher_number, pay.pay_id) == true)
            {
                validateFailuresPay.Add($"Số phiếu chi <{pay.voucher_number}> đã tồn tại trong hệ thống, vui lòng kiểm tra lại.");
            }
            return validateFailuresPay;
        }

    }
}
