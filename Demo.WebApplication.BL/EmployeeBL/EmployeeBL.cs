using Demo.WebApplication.BL.BaseBL;
using Demo.WebApplication.Common.Attributes;
using Demo.WebApplication.Common.Entities;
using Demo.WebApplication.Common.Entities.DTO;
using Demo.WebApplication.Common.Resources;
using Demo.WepApplication.DL.BaseDL;
using Demo.WepApplication.DL.EmployeeDL;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace Demo.WebApplication.BL.EmployeeBL
{
    public class EmployeeBL : BaseBL<Employee>, IPayDetailBL
    {
        #region Field
        private IPayDetailDL _employeeDL;
        #endregion

        #region Method
        public EmployeeBL(IPayDetailDL employeeDL) : base(employeeDL)
        {
            _employeeDL = employeeDL;
        }
   
        /// <summary>
        /// Sinh ra mã nhân viên mới
        /// </summary>
        /// <returns>Mã nhân viên mới</returns>
        /// Author: NVDUC (24/3/2023)
        public string GetNewCode()
        {
            return _employeeDL.GetNewCode();
        }

        /// <summary>
        /// Thực hiện tìm kiếm phân trang danh sách bản ghi
        /// </summary>
        /// <param name="search"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns>Các bản ghi trùng với điều kiện</returns>
        /// Author: NVDUC (23/3/2023)
        public PagingResult<Employee> GetPaging(string? search, int? pageNumber, int? pageSize)
        {
            return _employeeDL.GetPaging(search, pageNumber, pageSize);
        }

        #endregion
    }
}
