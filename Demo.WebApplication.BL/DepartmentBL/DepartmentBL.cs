using Demo.WebApplication.BL.BaseBL;
using Demo.WebApplication.Common.Entities;
using Demo.WepApplication.DL.DepartmentDL;
using Demo.WepApplication.DL.EmployeeDL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.WebApplication.BL.DepartmentBL
{
    public class DepartmentBL : BaseBL<Department>,IDepartmentBL
    {
        #region Field
        private IDepartmentDL _departmentDL;
        #endregion

        #region Constructor
        public DepartmentBL(IDepartmentDL departmentDL) : base(departmentDL)
        {
            _departmentDL = departmentDL;
        }
        #endregion
    }
}
