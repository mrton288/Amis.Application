
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
        public ServiceResult GetAllRecord()
        {
            try
            {
                var employees = _baseBL.GetAllRecord();
                // Thành công
                if (employees != null)
                {
                    return new ServiceResult
                    {
                        IsSuccess = true,
                        DevMsg = ContentMessage.S_Get,
                        UserMsg = ContentMessage.S_GetEmployee,
                        Data = employees,
                    };
                }
                // Thất bại
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

        /// <summary>
        /// Api lấy thông tin chi tiết của 1 bản ghi
        /// </summary>
        /// <param name="recordId">ID bản ghi muốn lấy</param>
        /// <returns>Đối tượng bản ghi</returns>
        /// Created by: NVDUC (9/3/2023)
        [HttpGet("{recordId}")]
        public ServiceResult GetRecordById([FromRoute] Guid recordId)

        {
            try
            {
                var record = _baseBL.GetRecordById(recordId);
                // Thành công 
                if (record != null)
                {
                    return new ServiceResult
                    {
                        IsSuccess = true,
                        DevMsg = ContentMessage.S_Get,
                        UserMsg = ContentMessage.S_GetEmployee,
                        Data = record,
                    };
                }
                // Thất bại
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

        /// <summary>
        /// Thực hiện sửa thông tin bản ghi theo Id
        /// </summary>
        /// <param name="recordId"></param>
        /// <param name="newRecord"></param>
        /// <returns></returns>
        /// Author: NVDUC (13/3/2023)
        [HttpPut("{recordId}")]
        public ServiceResult UpdateRecordById(Guid recordId, T newRecord)
        {
            try
            {
                return _baseBL.UpdateRecordById(recordId, newRecord);
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

        /// <summary>
        /// Api thêm một bản ghi mới
        /// </summary>
        /// <param name="newRecord"></param>
        /// <returns>Trả về trạng thái thêm mới</returns>
        /// Created By: NVDUC (13/3/2023)
        [HttpPost]
        public ServiceResult InsertRecord([FromBody] T newRecord)
        {
            try
            {
                //TODO: Hàm insert 
                return _baseBL.InsertRecord(newRecord);
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

        /// <summary>
        /// Xoá một bản ghi theo Id
        /// </summary>
        /// <param name="recordId"></param>
        /// <returns>Thông báo kết quả xoá</returns>
        /// Created By : NVDUC (13/3/2023)
        [HttpDelete("{recordId}")]
        public ServiceResult DeleteRecordById([FromRoute] Guid recordId)
        {   
            try
            {
                return _baseBL.DeleteRecordById(recordId);
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
    }
}
