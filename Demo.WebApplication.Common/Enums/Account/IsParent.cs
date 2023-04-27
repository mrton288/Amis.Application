using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.WebApplication.Common.Enums.Account
{
    public enum IsParent
    {
        /// <summary>
        /// Là tài khoản cha
        /// </summary>
        Parent = 1,

        /// <summary>
        /// Là tài khoản con
        /// </summary>
        Children = 0,
    }
}
