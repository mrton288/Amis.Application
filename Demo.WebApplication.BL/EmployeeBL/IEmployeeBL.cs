using Demo.WebApplication.Common.Entities.DTO;
using Demo.WebApplication.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.WebApplication.BL.BaseBL;

namespace Demo.WebApplication.BL.EmployeeBL
{
    public interface IEmployeeBL : IBaseBL<Employee>
    {
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
        /// Xoá nhiều nhân viên theo danh sách Id
        /// </summary>
        /// <param name="listEmployeeId"></param>
        /// <returns>Số lượng Id trong danh sách</returns>
        /// Author: NVDUC (25/3/2023)
        public int DeleteMultiple(Guid[] listEmployeeId);
    }
}
