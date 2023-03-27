using Dapper;
using Demo.WebApplication.BL.BaseBL;
using Demo.WebApplication.BL.EmployeeBL;
using Demo.WebApplication.Common.Entities;
using Demo.WebApplication.Common.Entities.DTO;
using Demo.WebApplication.Common.Enums;
using Demo.WebApplication.Common.Resources;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using System.Data;
using System.Data.Common;

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
        [HttpGet("newCode")]
        public string GetNewCode()
        {
            return _employeeBL.GetNewCode();
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
