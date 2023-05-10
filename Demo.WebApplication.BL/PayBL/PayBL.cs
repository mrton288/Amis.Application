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
        public Task<MemoryStream> ExportExcelPay(List<Pay> pays)
        {
            return _payDL.ExportExcelPay(pays);
        }
    }
}
