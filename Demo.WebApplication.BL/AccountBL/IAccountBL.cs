using Demo.WebApplication.BL.BaseBL;
using Demo.WebApplication.Common.Entities;
using Demo.WebApplication.Common.Entities.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.WebApplication.BL.AccountBL
{
    public interface IAccountBL : IBaseBL<Account>
    {

        /// <summary>
        /// Thực hiện lấy ra danh sách tài khoản theo keyword tìm kiếm
        /// </summary>
        /// <param name="search"></param>
        /// <returns>Danh sách tài khoản</returns>
        /// Author: NVDUC (29/04/2023)
        public ServiceResult GetAllByKey(string? search);

        /// <summary>
        /// Thực hiện cập nhật trạng thái của tài khoản
        /// </summary>
        /// <param name="id"></param>
        /// <param name="statusUpdate"></param>
        /// <returns>Trạng thái mới được cập nhật</returns>
        /// Author: NVDUC (29/04/2023)
        public ServiceResult UpdateStatus(Guid id, int statusUpdate);

        /// <summary>
        /// Thực hiện cập nhật từ tài khoản thường lên tài khoản cha
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// Author: NVDUC (29/04/2023)
        public ServiceResult UpdateParent(Guid id ,int isParent);


        /// <summary>
        /// Trả về danh sách các tài khoản con của tài khoản
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// Author: NVDUC (29/04/2023)
        public ServiceResult GetListChild(Guid id);


        /// <summary>
        /// Kiểm tra các trường dữ liệu của Account
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        /// Author: NVDUC (24/4/2023)
        public List<string> ValidateRequestDataCustom(Account account);
    }
}
