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
    public class EmployeeBL : BaseBL<Employee>, IEmployeeBL
    {
        #region Field
        private IEmployeeDL _employeeDL;
        #endregion

        #region Method
        public EmployeeBL(IEmployeeDL employeeDL) : base(employeeDL)
        {
            _employeeDL = employeeDL;
        }
   
        /// <summary>
        /// Kiểm tra các trường dữ liệu của Employee
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        /// Author: NVDUC (4/4/2023)
        public override List<string> ValidateRequestDataCustom(Employee employee)
        {
            // Mảng danh sách lỗi
            var validateFailuresEmployee = new List<string>();
            if (_employeeDL.CheckDuplicateCode(employee.EmployeeCode, employee.EmployeeId) == true)
            {
                validateFailuresEmployee.Add(Common.Resources.ContentMessage.DuplicateCode);
            }
            return validateFailuresEmployee;
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
        /// Xoá nhiều nhân viên theo danh sách Id
        /// </summary>
        /// <param name="listEmployeeId"></param>
        /// <returns>Số lượng Id trong danh sách</returns>
        /// Author: NVDUC (25/3/2023)
        public int DeleteMultiple(Guid[] listEmployeeId)
        {
            return _employeeDL.DeleteMultiple(listEmployeeId);
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

        /// <summary>
        /// Thực hiện chức năng xuất excel toàn bộ dữ liệu 
        /// </summary>
        /// <param name="employees"></param>
        /// <returns>File excel chứa toàn bộ dữ liệu</returns>
        /// Author: NVDUC (1/4/2023)
        public Task<MemoryStream> ExportExcelEmployee(List<Employee> employees)
        {
            return _employeeDL.ExportExcelEmployee(employees);
        }
        #endregion
    }
}
