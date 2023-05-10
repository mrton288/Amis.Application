using Demo.WebApplication.BL.BaseBL;
using Demo.WebApplication.Common.Entities;
using Demo.WebApplication.Common.Entities.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.WebApplication.BL.PayBL
{
    public interface IPayBL : IBaseBL<Pay>
    {
        /// <summary>
        /// Sinh ra số chứng từ mới
        /// </summary>
        /// <returns>Số chứng từ mới</returns>
        /// Author: NVDUC (04/05/2023)
        public string GetNewVoucherCode();



        /// <summary>
        /// Thực hiện lấy ra danh sách theo keyword tìm kiếm
        /// </summary>
        /// <param name="search"></param>
        /// <returns>Danh sách</returns>
        /// Author: NVDUC (29/04/2023)
        public ServiceResult GetAllByKey(string? search);


        /// <summary>
        /// Thực hiện chức năng xuất excel theo điều kiện tìm kiếm
        /// </summary>
        /// <param name="pays"></param>
        /// <returns>File excel chứa dữ liệu theo điều kiện tìm kiếm</returns>
        /// Author: NVDUC (04/05/2023)
        public Task<MemoryStream> ExportExcelPay(List<Pay> pays);
    }
}
