using Demo.WebApplication.Common.Entities.DTO;
using Demo.WebApplication.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.WebApplication.BL.EmployeeBL
{
    public interface IEmployeeBL
    {
        /// <summary>
        /// Trả về danh sách nhân viên
        /// </summary>
        /// <returns>Danh sách nhân viên</returns>
        /// Author: NVDUC (23/3/2023)
        public List<Employee> GetAllEmployee();

        /// <summary>
        /// Lấy ra thông tin nhân viên
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns>Thông tin của nhân viên đó</returns>
        /// Author: NVDUC (23/3/2023)
        public Employee GetEmployeeById(Guid employeeId);

        /// <summary>
        /// Cập nhật thông tin nhân viên theo Id
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="newEmployee"></param>
        /// <returns>Trạng thái của hành động</returns>
        /// Author: NVDUC (23/3/2023)
        /// 
        public int UpdateEmployeeById(Guid employeeId, Employee newEmployee);

        /// <summary>
        /// Thực hiện thêm mới nhân viên
        /// </summary>
        /// <param name="newEmployee"></param>
        /// <returns>Trạng thái của hành động thêm mới</returns>
        /// Author: NVDUC (23/3/2023)
        public int InsertEmployee(Employee newEmployee);

        /// <summary>
        /// Xoá nhân viên theo Id
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns>Mã trạng thái thành công hay thất bại</returns>
        /// Author: NVDUC (23/3/2023)
        public int DeleteEmployeeById(Guid employeeId);

        /// <summary>
        /// Thực hiện tìm kiếm phân trang danh sách nhân viên
        /// </summary>
        /// <param name="search"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns>Các bản ghi trùng với điều kiện</returns>
        /// Author: NVDUC (23/3/2023)
        public PagingResult FilterEmployee(
        string? search,
        int pageNumber = 1,
        int pageSize = 50
       );
    }
}
