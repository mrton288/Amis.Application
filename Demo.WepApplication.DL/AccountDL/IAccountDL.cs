﻿using Demo.WebApplication.Common.Entities;
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
        //public IEnumerable<Account> GetAccountByCondition(string? search);
    }
}
