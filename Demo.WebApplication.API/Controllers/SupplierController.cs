using Demo.WebApplication.BL.BaseBL;
using Demo.WebApplication.BL.SupplierBL;
using Demo.WebApplication.Common.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Demo.WebApplication.API.Controllers
{
    public class SupplierController : BaseController<Supplier>
    {
        #region Field
        private ISupplierBL _supplierBL;
        #endregion

        #region Constructor
        public SupplierController(ISupplierBL supplierBL) : base(supplierBL)
        {
            _supplierBL = supplierBL;
        }
        #endregion
    }
}
