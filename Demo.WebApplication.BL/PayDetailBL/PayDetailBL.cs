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
        public IEnumerable<PayDetail> GetAllById(Guid payId)
        {
            return _payDetailDL.GetAllById(payId);
        }
    }
}
