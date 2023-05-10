using Demo.WebApplication.Common.Entities;
using Demo.WepApplication.DL.BaseDL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.WepApplication.DL.AccountDL
{
    public interface IAccountDL : IBaseDL<Account>
    {
        /// <summary>
        /// Thực hiện lấy ra danh sách tài khoản theo keyword tìm kiếm
        /// </summary>
        /// <param name="search"></param>
        /// <returns>Danh sách tài khoản</returns>
        /// Author: NVDUC (29/04/2023)
        public IEnumerable<Account> GetAllByKey(string? search);

        /// <summary>
        /// Thực hiện cập nhật trạng thái của tài khoản
        /// </summary>
        /// <param name="id"></param>
        /// <param name="statusUpdate"></param>
        /// <returns>Trạng thái mới được cập nhật</returns>
        /// Author: NVDUC (29/04/2023)
        public int UpdateStatus(Guid id, int statusUpdate);

        /// <summary>
        /// Thực hiện cập nhật từ tài khoản thường lên tài khoản cha
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// Author: NVDUC (29/04/2023)
        public int UpdateParent(Guid id, int isParent);


        /// <summary>
        /// Trả về danh sách các tài khoản con của tài khoản
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// Author: NVDUC (29/04/2023)
        public IEnumerable<Account> GetListChild(Guid id);


        /// <summary>
        /// Check trùng số tài khoản
        /// </summary>
        /// <param name="accountNumber">Số tài khoản</param>
        /// <param name="accountId">Id tài khoản</param>
        /// <returns>Trả về true - trùng mã, false - không trùng</returns>
        /// Author: NVDUC (29/4/2023)
        public bool CheckDuplicateAccount(string accountNumber, Guid accountId);


        /// <summary>
        /// Cập nhật trạng thái nhiều tài khoản
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="newStatus"></param>
        /// <returns></returns>
        /// Author: NVDUC (05/05/2023)
        public int UpdateMultipleStatus(Guid[] ids, int newStatus);
      
    }
}
