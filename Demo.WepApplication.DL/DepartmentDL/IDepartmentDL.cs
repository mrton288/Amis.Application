using Dapper;
using Demo.WebApplication.Common.Entities;
using Demo.WepApplication.DL.BaseDL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.WepApplication.DL.DepartmentDL
{
    public interface IDepartmentDL : IBaseDL<Department>
    {
       
    }
}
