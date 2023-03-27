using Dapper;
using Demo.WebApplication.Common.Entities;
using Demo.WepApplication.DL.BaseDL;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.WepApplication.DL.DepartmentDL
{
    public class DepartmentDL : BaseDL<Department>, IDepartmentDL
    {

    }
}
