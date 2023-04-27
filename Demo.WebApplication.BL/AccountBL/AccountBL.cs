using Demo.WebApplication.BL.BaseBL;
using Demo.WebApplication.Common.Entities;
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
        #endregion
    }
}
