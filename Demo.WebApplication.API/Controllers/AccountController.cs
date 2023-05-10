using Demo.WebApplication.BL.AccountBL;
using Demo.WebApplication.BL.BaseBL;
using Demo.WebApplication.Common.Entities;
using Demo.WebApplication.Common.Entities.DTO;
using Demo.WebApplication.Common.Enums;
using Demo.WebApplication.Common.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Demo.WebApplication.API.Controllers
{
    public class AccountController : BaseController<Account>
    {

        #region Field
        public IAccountBL _accountBL;
        #endregion

        public AccountController(IAccountBL accountBL) : base(accountBL)
        {
            _accountBL = accountBL;
        }

        /// <summary>
        /// Thực hiện gọi api cập nhật trạng thái của tài khoản
        /// </summary>
        /// <param name="id"></param>
        /// <param name="statusUpdate"></param>
        /// <returns>Trạng thái mới được cập nhật</returns>
        /// Author: NVDUC (29/04/2023)
        [HttpPatch]
        public ServiceResult UpdateStatus([FromQuery]Guid id, [FromQuery]int statusUpdate)
        {
            try
            {
                return _accountBL.UpdateStatus(id, statusUpdate);
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
        /// Thực hiện gọi api lấy ra danh sách tài khoản theo keyword tìm kiếm
        /// </summary>
        /// <param name="search"></param>
        /// <returns>Danh sách tài khoản</returns>
        /// Author: NVDUC (29/04/2023)
        [HttpGet("getallByKey")]
        public ServiceResult GetAllByKey(string? search)
        {
            try
            {
                return _accountBL.GetAllByKey(search);
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
        /// Thực hiện cập nhật từ tài khoản thường lên tài khoản cha
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// Author: NVDUC (29/04/2023)
        [HttpPatch("updateParent")]
        public ServiceResult UpdateParent([FromQuery] Guid id, [FromQuery] int isParent = 1)
        {
            try
            {
                return _accountBL.UpdateParent(id, isParent);
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
        /// Trả về danh sách các tài khoản con của tài khoản
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// Author: NVDUC (29/04/2023)
        [HttpGet("getListChild")]
        public ServiceResult GetListChild(Guid id)
        {
            try
            {
                return _accountBL.GetListChild(id);
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
        /// Cập nhật trạng thái nhiều tài khoản
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="newStatus"></param>
        /// <returns></returns>
        /// Author: NVDUC (05/05/2023)
        [HttpPut("updateMultipleStatus")]
        public ServiceResult UpdateMultipleStatus([FromBody] Guid[] ids, [FromQuery] int newStatus)
        {
            try
            {
                return _accountBL.UpdateMultipleStatus(ids, newStatus);
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
