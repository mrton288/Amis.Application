using Demo.WebApplication.BL.BaseBL;
using Demo.WebApplication.Common.Entities;
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
    }
}
