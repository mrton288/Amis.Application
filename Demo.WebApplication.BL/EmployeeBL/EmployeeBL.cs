﻿using Demo.WebApplication.BL.BaseBL;
using Demo.WebApplication.Common.Entities;
using Demo.WebApplication.Common.Entities.DTO;
using Demo.WepApplication.DL.BaseDL;
using Demo.WepApplication.DL.EmployeeDL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.WebApplication.BL.EmployeeBL
{
    public class EmployeeBL : BaseBL<Employee>, IEmployeeBL
    {
        #region Field
        private IEmployeeDL _employeeDL;
        #endregion

        #region Constructor
        public EmployeeBL(IEmployeeDL employeeDL) : base(employeeDL)
        {
            _employeeDL = employeeDL;
        }

        public override int InsertRecord(Employee employeeRecord)
        {

            var validateFailures = new List<string>();
            if (_employeeDL.CheckDuplicateCode(employeeRecord.EmployeeCode) == true)
            {
                validateFailures.Add(Common.Resources.UserMessage.UserMsg_DuplicateCode);
            }
            if (validateFailures.Count > 0)
            {
                string errorContent = String.Join(" - ", validateFailures);
                return (int)Common.Enums.ErrorCode.DuplicateData;
            }
            return 1;

        }
        public int DeleteMultiple(Guid[] listEmployeeId)
        {
            return _employeeDL.DeleteMultiple(listEmployeeId);
        }

        public string GetNewCode()
        {
            return _employeeDL.GetNewCode();
        }

        public PagingResult<Employee> GetPaging(string? search, int? pageNumber, int? pageSize)
        {
            return _employeeDL.GetPaging(search, pageNumber, pageSize);
        }
        #endregion
    }
}
