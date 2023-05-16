using Demo.WebApplication.Common.Entities;
using Demo.WepApplication.DL.BaseDL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.WepApplication.DL.SupplierDL
{
    public class SupplierDL : BaseDL<Supplier>, ISupplierDL
    {
        public override object BuildQueryCustom()
        {
            string whereClause = "where supplier.supplier_code ilike ('%' || @search  || '%') " +
            "or supplier.supplier_name ilike ('%' || @search  || '%') " +
            "or supplier.tax_code ilike ('%' || @search  || '%') " +
            "or supplier.address ilike ('%' || @search  || '%') " +
            "or supplier.phone_number ilike ('%' || @search  || '%') ";
            return new
            {
                selectOption = "*",
                joinOption = "",
                orderBy = "supplier.supplier_code",
                search = whereClause,
            };
        }
    }
}
