using Dapper;
using Demo.WebApplication.BL.BaseBL;
using Demo.WebApplication.BL.EmployeeBL;
using Demo.WebApplication.Common.Entities;
using Demo.WebApplication.Common.Entities.DTO;
using Demo.WebApplication.Common.Enums;
using Demo.WebApplication.Common.Resources;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using System.Data;
using System.Data.Common;
using System.IO;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;
using OfficeOpenXml.Table;
using System.Drawing;
using System;
using System.Reflection;

namespace Demo.WebApplication.API.Controllers
{
    public class EmployeesController : BaseController<Employee>
    {
        #region Field
        private IEmployeeBL _employeeBL;
        #endregion


        #region Constructor
        public EmployeesController(IEmployeeBL employeeBL) : base(employeeBL)
        {
            _employeeBL = employeeBL;
        }
        #endregion

        #region Method
        /// <summary>
        /// Gọi api tự động tạo ra mã nhân viên mới
        /// </summary>
        /// <returns>Mã nhân viên mới</returns>
        /// Author : NVDUC (29/3/2023)
        [HttpGet("newCode")]
        public string GetNewCode()
        {
            return _employeeBL.GetNewCode();
        }

        /// <summary>
        /// Gọi api xuất ra file excel các nhân viên hiện tại
        /// </summary>
        /// <param name="search">Từ khoá tìm kiếm</param>
        /// <param name="pageNumber">Số trang hiện tại</param>
        /// <param name="pageSize">Kích thước trang hiện tại</param>
        /// <returns>File excel thông tin nhân viên</returns>
        /// Author: NVDUC (29/3/2023)
        [HttpGet("exportExcel")]
        public IActionResult ExportExcelEmployee([FromQuery] string? search,
            [FromQuery] int? pageNumber,
            [FromQuery] int? pageSize)
        {

            PagingResult<Employee> result = _employeeBL.GetPaging(search, pageNumber, pageSize);
            var employees = (List<Employee>)result.Data;
            var stream = new MemoryStream();
            using (var package = new ExcelPackage(stream))
            {
                // Tạo 1 sheet excel
                var workSheet = package.Workbook.Worksheets.Add("DANH SÁCH NHÂN VIÊN");
                workSheet.TabColor = System.Drawing.Color.Black;

                workSheet.DefaultRowHeight = 12;

                var rowStart = 3;
                // Biến lưu giá trị cột số thứ tự
                workSheet.Cells[rowStart, 1].Value = "STT";
                workSheet.Cells[rowStart, 2].Value = "Mã nhân viên";
                workSheet.Cells[rowStart, 3].Value = "Tên nhân viên";
                workSheet.Cells[rowStart, 4].Value = "Giới tính";
                workSheet.Cells[rowStart, 5].Value = "Ngày sinh";
                workSheet.Cells[rowStart, 6].Value = "Số CMND";
                workSheet.Cells[rowStart, 7].Value = "Ngày cấp";
                workSheet.Cells[rowStart, 8].Value = "Nơi cấp";
                workSheet.Cells[rowStart, 9].Value = "Tên đơn vị";
                workSheet.Cells[rowStart, 10].Value = "Chức danh";
                workSheet.Cells[rowStart, 11].Value = "Địa chỉ";
                workSheet.Cells[rowStart, 12].Value = "Điện thoại di động";
                workSheet.Cells[rowStart, 13].Value = "Điện thoại cố định";
                workSheet.Cells[rowStart, 14].Value = "Email";
                workSheet.Cells[rowStart, 15].Value = "Tài khoản ngân hàng";
                workSheet.Cells[rowStart, 16].Value = "Tên ngân hàng";
                workSheet.Cells[rowStart, 17].Value = "Chi nhánh";

                // set nội dung (thông tin tất cả nhân viên) cho bảng của file excel (+1 vào rowStart để bỏ qua header)
                var forLoopIndex = rowStart + 1;
                var tableIndex = 1;
                foreach (var employee in employees)
                {
                    workSheet.Cells[forLoopIndex, 1].Value = tableIndex;
                    workSheet.Cells[forLoopIndex, 2].Value = employee.EmployeeCode;
                    workSheet.Cells[forLoopIndex, 3].Value = employee.FullName;
                    if (employee.Gender == Gender.MALE)
                    {
                        workSheet.Cells[forLoopIndex, 4].Value = "Nam";
                    }
                    else if (employee.Gender == Gender.FEMALE)
                    {
                        workSheet.Cells[forLoopIndex, 4].Value = "Nữ";
                    }
                    else
                    {
                        workSheet.Cells[forLoopIndex, 4].Value = "Khác";
                    }
                    workSheet.Cells[forLoopIndex, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Cells[forLoopIndex, 5].Style.Numberformat.Format = "dd/mm/yyyy";
                    workSheet.Cells[forLoopIndex, 5].Value = employee.DateOfBirth;

                    workSheet.Cells[forLoopIndex, 6].Value = employee.IdentityNumber;

                    workSheet.Cells[forLoopIndex, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Cells[forLoopIndex, 7].Style.Numberformat.Format = "dd/mm/yyyy";
                    workSheet.Cells[forLoopIndex, 7].Value = employee.IdentityDate;

                    workSheet.Cells[forLoopIndex, 8].Value = employee.IdentityPlace;
                    workSheet.Cells[forLoopIndex, 9].Value = employee.DepartmentName;
                    workSheet.Cells[forLoopIndex, 10].Value = employee.PositionName;
                    workSheet.Cells[forLoopIndex, 11].Value = employee.Address;
                    workSheet.Cells[forLoopIndex, 12].Value = employee.PhoneNumber;
                    workSheet.Cells[forLoopIndex, 13].Value = employee.Landline;
                    workSheet.Cells[forLoopIndex, 14].Value = employee.Email;
                    workSheet.Cells[forLoopIndex, 15].Value = employee.BankAccount;
                    workSheet.Cells[forLoopIndex, 16].Value = employee.BankName;
                    workSheet.Cells[forLoopIndex, 17].Value = employee.BankBranch;

                    forLoopIndex++;
                    tableIndex++;
                    for (var i = 1; i <= 17; i++)
                    {
                        workSheet.Column(i).Style.Font.Name = "Times New Roman";
                        workSheet.Column(i).Style.Font.Size = 11;
                        workSheet.Cells[tableIndex + 1, i].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        workSheet.Cells[4, i].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    }
                }

                // Set style cho Title
                workSheet.Row(1).Height = 20;
                workSheet.Row(1).Style.Font.Bold = true;
                workSheet.Cells["A1:Q1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells["A1:Q1"].Merge = true;
                workSheet.Cells["A1:Q1"].Value = "DANH SÁCH NHÂN VIÊN";
                workSheet.Cells["A1:Q1"].Style.Font.Size = 16;
                workSheet.Cells["A1:Q1"].Style.Font.Name = "Arial";

                // Set style cho Header
                workSheet.Row(3).Height = 15;
                workSheet.Row(3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Row(3).Style.Font.Bold = true;
                workSheet.Cells["A3:Q3"].Style.Font.Size = 10;
                workSheet.Cells["A3:Q3"].Style.Fill.SetBackground(System.Drawing.Color.LightGray);
                workSheet.Cells["A3:Q3"].Style.Font.Name = "Arial";
                for (var i = 1; i <= 17; i++)
                {
                    workSheet.Column(i).AutoFit();
                }

                package.Save();
            }
            stream.Position = 0;
            string excelName = "Danh_sach_nhan_vien.xlsx";
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

        [HttpDelete]
        public IActionResult DeleteMultiple([FromBody] Guid[] listEmployeeId)
        {
            try
            {
                var result = _employeeBL.DeleteMultiple(listEmployeeId);
                return StatusCode(StatusCodes.Status200OK, new ServiceResult
                {
                    IsSuccess = true,
                    Message = StatusMessage.S_Delete,
                    Data = result,
                });
            }
            catch
            {
                return StatusCode(400, new ErrorResult
                {
                    ErrorCode = ErrorCode.InvalidData,
                    DevMsg = DevMessage.DevMsg_InValid,
                    UserMsg = UserMessage.UserMsg_InValid,
                    TraceId = HttpContext.TraceIdentifier
                });
            }
        }


        /// <summary>
        /// Thực hiện phân trang và tìm kiếm theo mã, tên, số điện thoại bản ghi
        /// </summary>
        /// <param name="search">Từ khoá tìm kiếm</param>
        /// <param name="pageSize">Số bản ghi trên trang</param>
        /// <param name="pageNumber">Số của trang hiện tại</param>
        /// <returns></returns>
        /// Created by: NVDUC (12/3/2023)
        [HttpGet("paging")]
        public object GetPaging(
            [FromQuery] string? search,
            [FromQuery] int? pageNumber,
            [FromQuery] int? pageSize
            )
        {
            try
            {
                var multipleResults = _employeeBL.GetPaging(search, pageNumber, pageSize);
                if (multipleResults != null)
                {
                    return StatusCode(StatusCodes.Status200OK, multipleResults);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ErrorResult
                    {
                        ErrorCode = ErrorCode.InvalidData,
                        DevMsg = DevMessage.DevMsg_InValid,
                        UserMsg = UserMessage.UserMsg_InValid,
                        TraceId = HttpContext.TraceIdentifier
                    });
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                return StatusCode(500, new ErrorResult
                {
                    ErrorCode = ErrorCode.Exception,
                    DevMsg = DevMessage.DevMsg_Exception,
                    UserMsg = UserMessage.UserMsg_Exception,
                    TraceId = HttpContext.TraceIdentifier,
                });
            }
        }
        #endregion
    }
}
