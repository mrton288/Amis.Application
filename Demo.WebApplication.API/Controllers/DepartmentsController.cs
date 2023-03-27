using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using Dapper;
using Demo.WebApplication.BL.DepartmentBL;
using Demo.WebApplication.Common.Enums;
using Demo.WebApplication.Common.Resources;
using Demo.WebApplication.Common.Entities.DTO;
using Demo.WebApplication.Common.Entities;
using Demo.WebApplication.BL.BaseBL;

namespace Demo.WebApplication.API.Controllers
{
    public class DepartmentsController : BaseController<Department>
    {
        public DepartmentsController(IBaseBL<Department> baseBL) : base(baseBL)
        {
        }
    }
}
