using Demo.WebApplication.BL.BaseBL;
using Demo.WebApplication.Common.Entities;
using Demo.WepApplication.DL.SupplierDL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.WebApplication.BL.SupplierBL
{
    public class SupplierBL : BaseBL<Supplier>, ISupplierBL
    {
        #region Field
        private readonly ISupplierDL _supplierDL;
        #endregion

        #region Constructor
        public SupplierBL(ISupplierDL supplierDL) : base(supplierDL)
        {
            _supplierDL = supplierDL;
        }
        #endregion
    }
}
