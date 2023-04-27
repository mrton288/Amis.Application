using Demo.WebApplication.Common.Entities;
using Demo.WepApplication.DL.BaseDL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.WepApplication.DL.PayDetailDL
{
    public interface IPayDetailDL : IBaseDL<PayDetail>
    {
        /// <summary>
        /// Lấy ra danh sách chi tiền chi tiết khi
        /// </summary>
        /// <param name="payId">Id của bản ghi chi tiền cha</param>
        /// <returns>Danh sách chi tiền chi tiết</returns>
        /// Author: NVDUC (25/04/2023)
        public IEnumerable<PayDetail> GetAllById(Guid payId);
    }
}
