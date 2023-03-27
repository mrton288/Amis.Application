
using Demo.WebApplication.Common.Entities.DTO;
using Demo.WebApplication.Common.Entities;
using Demo.WebApplication.Common.Enums;
using Demo.WebApplication.Common.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Demo.WebApplication.BL.BaseBL;

namespace Demo.WebApplication.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BaseController<T> : ControllerBase
    {
        #region Field

        private IBaseBL<T> _baseBL;

        #endregion

        #region Constructor

        public BaseController(IBaseBL<T> baseBL)
        {
            _baseBL = baseBL;
        }

        #endregion

        /// <summary>
        /// Lấy thông tin của tất cả bản ghi
        /// </summary>
        /// <returns>Toàn bộ bản ghi</returns>
        /// Created by: NVDUC (9/3/2023)
        [HttpGet]
        public IActionResult GetAllRecord()
        {
            try
            {
                var employees = _baseBL.GetAllRecord();
                // Thành công
                if (employees != null)
                {
                    return Ok(employees);
                }
                // Thất bại
                else
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
            catch
            {
                return StatusCode(500, new ErrorResult
                {
                    ErrorCode = ErrorCode.Exception,
                    DevMsg = DevMessage.DevMsg_Exception,
                    UserMsg = UserMessage.UserMsg_Exception,
                    TraceId = HttpContext.TraceIdentifier
                });
            }
        }

        /// <summary>
        /// Api lấy thông tin chi tiết của 1 bản ghi
        /// </summary>
        /// <param name="recordId">ID bản ghi muốn lấy</param>
        /// <returns>Đối tượng bản ghi</returns>
        /// Created by: NVDUC (9/3/2023)
        [HttpGet("{recordId}")]
        public IActionResult GetRecordById([FromRoute] Guid recordId)
        {
            try
            {
                var record = _baseBL.GetRecordById(recordId);
                // Thành công 
                if (record != null)
                {
                    return StatusCode(StatusCodes.Status200OK, record);
                }
                // Thất bại
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ErrorResult
                    {
                        ErrorCode = ErrorCode.InvalidData,
                        DevMsg = DevMessage.DevMsg_NotFound,
                        UserMsg = UserMessage.UserMsg_NotFound,
                        TraceId = HttpContext.TraceIdentifier
                    });
                }
            }
            catch
            {

                return StatusCode(500, new ErrorResult
                {
                    ErrorCode = ErrorCode.Exception,
                    DevMsg = DevMessage.DevMsg_Exception,
                    UserMsg = UserMessage.UserMsg_Exception,
                    TraceId = HttpContext.TraceIdentifier
                });

            }
        }

        /// <summary>
        /// Thực hiện sửa thông tin bản ghi theo Id
        /// </summary>
        /// <param name="recordId"></param>
        /// <param name="newRecord"></param>
        /// <returns></returns>
        /// Author: NVDUC (13/3/2023)
        [HttpPut("{recordId}")]
        public IActionResult UpdateRecordById(Guid recordId, T newRecord)
        {
            try
            {

                var result = _baseBL.UpdateRecordById(recordId, newRecord);

                if (result > 0)
                {
                    Console.WriteLine($"Record with Id {recordId} has been updated.", recordId);
                    return StatusCode(StatusCodes.Status200OK, new ServiceResult
                    {
                        IsSuccess = true,
                        Message = StatusMessage.S_Put,
                        Data = result,
                    });
                }
                else
                {
                    Console.WriteLine($"Record with Id {recordId} not found.", recordId);
                    return StatusCode(StatusCodes.Status404NotFound, new ErrorResult
                    {
                        ErrorCode = ErrorCode.InvalidData,
                        DevMsg = DevMessage.DevMsg_NotFound,
                        UserMsg = UserMessage.UserMsg_NotFound,
                        TraceId = HttpContext.TraceIdentifier
                    });
                }

            }
            catch
            {
                return StatusCode(500, new ErrorResult
                {
                    ErrorCode = ErrorCode.Exception,
                    DevMsg = DevMessage.DevMsg_Exception,
                    UserMsg = UserMessage.UserMsg_Exception,
                    TraceId = HttpContext.TraceIdentifier
                });
            }
        }

        /// <summary>
        /// Api thêm một bản ghi mới
        /// </summary>
        /// <param name="newRecord"></param>
        /// <returns>Trả về trạng thái thêm mới</returns>
        /// Created By: NVDUC (13/3/2023)
        [HttpPost]
        public IActionResult InsertRecord([FromBody] T newRecord)
        {
            try
            {
                var result = _baseBL.InsertRecord(newRecord);
                // Thành công 
                if (result > 0)
                {
                    Console.WriteLine("Record created");
                    return StatusCode(201, new ServiceResult
                    {
                        IsSuccess = true,
                        Message = StatusMessage.S_Post,
                        Data = result,
                    });
                }
                else
                {
                    Console.WriteLine("Record code not valid");
                    return StatusCode(400, new ErrorResult
                    {
                        ErrorCode = ErrorCode.InvalidData,
                        DevMsg = DevMessage.DevMsg_InValid,
                        UserMsg = UserMessage.UserMsg_InValid,
                        TraceId = HttpContext.TraceIdentifier
                    });
                }
            }
            catch
            {
                return StatusCode(500, new ErrorResult
                {
                    ErrorCode = ErrorCode.Exception,
                    DevMsg = DevMessage.DevMsg_Exception,
                    UserMsg = UserMessage.UserMsg_Exception,
                    TraceId = HttpContext.TraceIdentifier
                });
            }
        }

        /// <summary>
        /// Xoá một bản ghi theo Id
        /// </summary>
        /// <param name="recordId"></param>
        /// <returns>Thông báo kết quả xoá</returns>
        /// Created By : NVDUC (13/3/2023)
        [HttpDelete("{recordId}")]
        public IActionResult DeleteRecordById([FromRoute] Guid recordId)
        {
            try
            {
                var result = _baseBL.DeleteRecordById(recordId);
                if (result > 0)
                {
                    return StatusCode(200, new ServiceResult
                    {
                        IsSuccess = true,
                        Message = StatusMessage.S_Delete,
                        Data = result,
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ErrorResult
                    {
                        ErrorCode = ErrorCode.InvalidData,
                        DevMsg = DevMessage.DevMsg_NotFound,
                        UserMsg = UserMessage.UserMsg_NotFound,
                        TraceId = HttpContext.TraceIdentifier
                    });
                }
            }
            catch
            {

                return StatusCode(500, new ErrorResult
                {
                    ErrorCode = ErrorCode.Exception,
                    DevMsg = DevMessage.DevMsg_Exception,
                    UserMsg = UserMessage.UserMsg_Exception,
                    TraceId = HttpContext.TraceIdentifier
                });
            }
        }
    }
}
