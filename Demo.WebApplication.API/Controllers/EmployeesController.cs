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
using Demo.WepApplication.DL.EmployeeDL;
using System.Reflection.Metadata.Ecma335;

namespace Demo.WebApplication.API.Controllers
{
    public class EmployeesController : BaseController<Employee>
    {
        #region Field
        private IPayDetailBL _employeeBL;
        #endregion


        #region Constructor
        public EmployeesController(IPayDetailBL employeeBL) : base(employeeBL)
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
        /// Thực hiện phân trang và tìm kiếm theo mã, tên, số điện thoại bản ghi
        /// </summary>
        /// <param name="search">Từ khoá tìm kiếm</param>
        /// <param name="pageSize">Số bản ghi trên trang</param>
        /// <param name="pageNumber">Số của trang hiện tại</param>
        /// <returns></returns>
        /// Created by: NVDUC (12/3/2023)
        [HttpGet("paging")]
        public ServiceResult GetPaging(
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
                    return new ServiceResult
                    {
                        IsSuccess = true,
                        DevMsg = ContentMessage.S_Get,
                        UserMsg = ContentMessage.S_Get,
                        Data = multipleResults,
                    };
                }
                else
                {
                    return new ServiceResult
                    {
                        IsSuccess = false,
                        DevMsg = ContentMessage.NotFound,
                        UserMsg = ContentMessage.NotFound,
                        ErrorCode = ErrorCode.NotFound,
                    };
                }
            }
            catch
            {
                return new ServiceResult
                {
                    IsSuccess = false,
                    DevMsg = ContentMessage.Exception,
                    UserMsg = ContentMessage.Exception,
                    ErrorCode = ErrorCode.Exception,
                };
            }
        }

        #endregion
    }
}
