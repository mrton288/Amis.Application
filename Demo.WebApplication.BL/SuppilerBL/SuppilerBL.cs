using Demo.WebApplication.BL.BaseBL;
using Demo.WebApplication.Common.Entities;
using Demo.WepApplication.DL.SuppilerDL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.WebApplication.BL.SuppilerBL
{
    public class SuppilerBL : BaseBL<Supplier>, ISuppilerBL
    {
        #region Field
        private readonly ISuppilerDL _supplierDL;
        #endregion

        #region Constructor
        public SuppilerBL(ISuppilerDL suppilerDL) : base(suppilerDL)
        {
            _supplierDL = suppilerDL;
        } 
        #endregion
    }
}
