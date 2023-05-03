using Demo.WebApplication.BL.BaseBL;
using Demo.WebApplication.Common.Entities;
using Demo.WebApplication.Common.Entities.DTO;
using Demo.WebApplication.Common.Enums;
using Demo.WebApplication.Common.Resources;
using Demo.WepApplication.DL.AccountDL;
using Demo.WepApplication.DL.BaseDL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.WebApplication.BL.AccountBL
{
    public class AccountBL : BaseBL<Account>, IAccountBL
    {
        #region Field
        private IAccountDL _accountDL;
        #endregion

        #region Constructor
        public AccountBL(IAccountDL accountDL) : base(accountDL)
        {
            _accountDL = accountDL;
        }


        /// <summary>
        /// Thực hiện lấy ra danh sách tài khoản theo keyword tìm kiếm
        /// </summary>
        /// <param name="search"></param>
        /// <returns>Danh sách tài khoản</returns>
        /// Author: NVDUC (29/04/2023)
        public ServiceResult GetAllByKey(string? search)
        {
            var records = _accountDL.GetAllByKey(search);
            // Thành công
            if (records != null)
            {
                return new ServiceResult
                {
                    IsSuccess = true,
                    DevMsg = ContentMessage.S_Get,
                    UserMsg = ContentMessage.S_Get,
                    Data = records,
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



        /// <summary>
        /// Thực hiện cập nhật trạng thái của tài khoản
        /// </summary>
        /// <param name="id"></param>
        /// <param name="statusUpdate"></param>
        /// <returns>Trạng thái mới được cập nhật</returns>
        /// Author: NVDUC (29/04/2023)
        public ServiceResult UpdateStatus(Guid id, int statusUpdate)
        {
            if (_accountDL.UpdateStatus(id, statusUpdate) != 0)
            {
                return new ServiceResult
                {
                    IsSuccess = true,
                    UserMsg = Common.Resources.ContentMessage.S_Put,
                };
            }
            else
            {
                return new ServiceResult
                {
                    IsSuccess = false,
                    UserMsg = Common.Resources.ContentMessage.NotFound,
                    ErrorCode = Common.Enums.ErrorCode.NotFound,
                };
            }
        }

        /// <summary>
        /// Thực hiện cập nhật từ tài khoản thường lên tài khoản cha
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// Author: NVDUC (29/04/2023)
        public ServiceResult UpdateParent(Guid id, int isParent)
        {
            if (_accountDL.UpdateParent(id, isParent) != 0)
            {
                return new ServiceResult
                {
                    IsSuccess = true,
                    UserMsg = Common.Resources.ContentMessage.S_Put,
                };
            }
            else
            {
                return new ServiceResult
                {
                    IsSuccess = false,
                    UserMsg = Common.Resources.ContentMessage.NotFound,
                    ErrorCode = Common.Enums.ErrorCode.NotFound,
                };
            }
        }

        /// <summary>
        /// Trả về danh sách các tài khoản con của tài khoản
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// Author: NVDUC (29/04/2023)
        public ServiceResult GetListChild(Guid id)
        {
            var records = _accountDL.GetListChild(id);
            // Thành công
            if (records != null)
            {
                return new ServiceResult
                {
                    IsSuccess = true,
                    DevMsg = ContentMessage.S_Get,
                    UserMsg = ContentMessage.S_Get,
                    Data = records,
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

        /// <summary>
        /// Kiểm tra các trường dữ liệu của Account
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        /// Author: NVDUC (24/4/2023)
        public override List<string> ValidateRequestDataCustom(Account account)
        {
            // Mảng danh sách lỗi
            var validateFailuresEmployee = new List<string>();
            if (_accountDL.CheckDuplicateAccount(account.account_number, account.account_id) == true)
            {
                validateFailuresEmployee.Add(Common.Resources.ContentMessage.DuplicateAccount);
            }
            return validateFailuresEmployee;
        }
        #endregion
    }
}
