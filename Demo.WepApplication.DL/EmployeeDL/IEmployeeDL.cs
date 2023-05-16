using Demo.WebApplication.Common.Entities.DTO;
using Demo.WebApplication.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Dapper;
using Demo.WepApplication.DL.BaseDL;
using Microsoft.AspNetCore.Http;

namespace Demo.WepApplication.DL.EmployeeDL
{
    public interface IPayDetailDL : IBaseDL<Employee>
    {
        #region Method
        /// <summary>
        /// Sinh ra mã nhân viên mới
        /// </summary>
        /// <returns>Mã nhân viên mới</returns>
        /// Author: NVDUC (24/3/2023)
        public string GetNewCode();

        /// <summary>
        /// Thực hiện tìm kiếm phân trang danh sách bản ghi
        /// </summary>
        /// <param name="search"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns>Các bản ghi trùng với điều kiện</returns>
        /// Author: NVDUC (23/3/2023)
        public PagingResult<Employee> GetPaging(
        string? search,
        int? pageNumber,
        int? pageSize
       );

        /// <summary>
        /// Check trùng mã nhân viên
        /// </summary>
        /// <param name="employeeCode">Mã của bản ghi</param>
        /// <param name="employeeId">Id của bản ghi</param>
        /// <returns>Trả về true - trùng mã, false - không trùng</returns>
        /// Author: NVDUC (26/3/2023)
        public bool CheckDuplicateCode(string employeeCode, Guid employeeId);
        #endregion
    }
}
