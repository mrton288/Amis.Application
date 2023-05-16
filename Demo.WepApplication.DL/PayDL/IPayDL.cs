using Demo.WebApplication.Common.Entities;
using Demo.WebApplication.Common.Entities.DTO;
using Demo.WepApplication.DL.BaseDL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.WepApplication.DL.PayDL
{
    public interface IPayDL : IBaseDL<Pay>
    {
        /// <summary>
        /// Sinh ra số chứng từ mới
        /// </summary>
        /// <returns>Số chứng từ mới</returns>
        /// Author: NVDUC (04/05/2023)
        public string GetNewVoucherCode();

        /// <summary>
        /// Thực hiện chức năng xuất excel theo điều kiện tìm kiếm
        /// </summary>
        /// <param name="pays"></param>
        /// <returns>File excel chứa dữ liệu theo điều kiện tìm kiếm</returns>
        /// Author: NVDUC (04/05/2023)
        public Task<MemoryStream> ExportExcelPay(string? search);

        /// <summary>
        /// Thực hiện lấy ra danh sách theo keyword tìm kiếm
        /// </summary>
        /// <param name="search"></param>
        /// <returns>Danh sách</returns>
        /// Author: NVDUC (29/04/2023)
        public IEnumerable<dynamic> GetAllByKey(string? search);

        /// <summary>
        /// Thực hiện xoá bản ghi ở Table Pay đồng thời 
        /// lấy ra và xoá bản ghi ở table Paydetail
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        /// Author: NVDUC (11/05/2023)
        public int DeleteFullMultiple(Guid[]? ids);

        /// <summary>
        /// Thực hiện xoá bản ghi ở Table Pay đồng thời 
        /// lấy ra và xoá bản ghi ở table Paydetail
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// Author: NVDUC (11/05/2023)
        public int DeleteMultiple(Guid id);

        /// <summary>
        /// Check trùng số phiếu chi
        /// </summary>
        /// <param name="voucherNumber">Số phiếu chi</param>
        /// <param name="payId">Id pay</param>
        /// <returns>Trả về true - trùng mã, false - không trùng</returns>
        /// Author: NVDUC (29/4/2023)
        public bool CheckDuplicateVoucherNumber(string voucherNumber, Guid payId);

    }
}
