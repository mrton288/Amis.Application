using Dapper;
using Demo.WebApplication.Common.Attributes;
using Demo.WebApplication.Common.Entities;
using Demo.WebApplication.Common.Entities.DTO;
using Demo.WebApplication.Common.Enums;
using Demo.WebApplication.Common.Resources;
using Demo.WepApplication.DL.BaseDL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace Demo.WebApplication.BL.BaseBL
{
    public class BaseBL<T> : IBaseBL<T>
    {
        #region Field

        private IBaseDL<T> _baseDL;

        #endregion

        #region Constructor

        public BaseBL(IBaseDL<T> baseDL)
        {
            _baseDL = baseDL;
        }

        #endregion

        #region Method
        /// <summary>
        /// Trả về danh sách bản ghi
        /// </summary>
        /// <returns>Danh sách bản ghi</returns>
        /// Author: NVDUC (23/3/2023)
        public IEnumerable<T> GetAllRecord()
        {
            return _baseDL.GetAllRecord();
        }

        /// <summary>
        /// Lấy ra thông tin bản ghi
        /// </summary>
        /// <param name="recordId"></param>
        /// <returns>Thông tin của bản ghi đó</returns>
        /// Author: NVDUC (23/3/2023)
        public dynamic GetRecordById(Guid recordId)
        {
            return _baseDL.GetRecordById(recordId);
        }

        /// <summary>
        /// Cập nhật thông tin bản ghi theo Id
        /// </summary>
        /// <param name="recordId"></param>
        /// <param name="newRecord"></param>
        /// <returns>Trạng thái của hành động</returns>
        /// Author: NVDUC (23/3/2023)
        public ServiceResult UpdateRecordById(Guid recordId, T newRecord)
        {
            var validateFailures = ValidateRequestData(newRecord);
            if (validateFailures.Count > 0)
            {
                return new ServiceResult
                {
                    IsSuccess = false,
                    DevMsg = Common.Resources.ContentMessage.InValid,
                    UserMsg = Common.Resources.ContentMessage.InValid,
                    ErrorCode = Common.Enums.ErrorCode.InvalidData,
                    Data = validateFailures
                };
            }

            var validateFailuresCustom = ValidateRequestDataCustom(newRecord);
            if (validateFailuresCustom.Count > 0)
            {
                return new ServiceResult
                {
                    IsSuccess = false,
                    DevMsg = Common.Resources.ContentMessage.InValid,
                    UserMsg = Common.Resources.ContentMessage.InValid,
                    ErrorCode = Common.Enums.ErrorCode.InvalidData,
                    Data = validateFailuresCustom
                };
            }
            else
            {
                if (_baseDL.UpdateRecordById(recordId, newRecord) > 0)
                {
                    return new ServiceResult
                    {
                        IsSuccess = true,
                        UserMsg = Common.Resources.ContentMessage.S_Put,
                        Data = newRecord
                    };
                }
                else
                {
                    return new ServiceResult
                    {
                        IsSuccess = false,
                        DevMsg = Common.Resources.ContentMessage.NotFound,
                        UserMsg = Common.Resources.ContentMessage.E_Put,
                        ErrorCode = Common.Enums.ErrorCode.NotFound,
                    };
                }
            }
        }

        /// <summary>
        /// Thực hiện thêm mới bản ghi
        /// </summary>
        /// <param name="newRecord"></param>
        /// <returns>Trạng thái của hành động thêm mới</returns>
        /// Author: NVDUC (23/3/2023)
        public ServiceResult InsertRecord(T newRecord)
        {
            var validateFailures = ValidateRequestData(newRecord);
            if (validateFailures.Count > 0)
            {
                return new ServiceResult
                {
                    IsSuccess = false,
                    DevMsg = Common.Resources.ContentMessage.InValid,
                    UserMsg = Common.Resources.ContentMessage.InValid,
                    ErrorCode = Common.Enums.ErrorCode.InvalidData,
                    Data = validateFailures
                };
            }

            var validateFailuresCustom = ValidateRequestDataCustom(newRecord);
            if (validateFailuresCustom.Count > 0)
            {
                return new ServiceResult
                {
                    IsSuccess = false,
                    DevMsg = Common.Resources.ContentMessage.InValid,
                    UserMsg = Common.Resources.ContentMessage.InValid,
                    ErrorCode = Common.Enums.ErrorCode.InvalidData,
                    Data = validateFailuresCustom
                };
            }
            else
            {
                var result = _baseDL.InsertRecord(newRecord);
                if (result.NumberEffect > 0)
                {
                    return new ServiceResult
                    {
                        IsSuccess = true,
                        UserMsg = Common.Resources.ContentMessage.S_Post,
                        Data = result.IdInsert,
                    };
                }
                else
                {
                    return new ServiceResult
                    {
                        IsSuccess = false,
                        DevMsg = Common.Resources.ContentMessage.NotFound,
                        UserMsg = Common.Resources.ContentMessage.E_Post,
                        ErrorCode = Common.Enums.ErrorCode.NotFound,
                    };
                }
            }
        }

        /// <summary>
        /// Thực hiện thêm mới nhiều bản ghi
        /// </summary>
        /// <param name="records"></param>
        /// <returns></returns>
        public ServiceResult InsertMultiple(IEnumerable<T> records)
        {
            //var validateFailures = ValidateRequestData(newRecord);
            //if (validateFailures.Count > 0)
            //{
            //    return new ServiceResult
            //    {
            //        IsSuccess = false,
            //        DevMsg = Common.Resources.ContentMessage.InValid,
            //        UserMsg = Common.Resources.ContentMessage.InValid,
            //        ErrorCode = Common.Enums.ErrorCode.InvalidData,
            //        Data = validateFailures
            //    };
            //}

            //var validateFailuresCustom = ValidateRequestDataCustom(newRecord);
            //if (validateFailuresCustom.Count > 0)
            //{
            //    return new ServiceResult
            //    {
            //        IsSuccess = false,
            //        DevMsg = Common.Resources.ContentMessage.InValid,
            //        UserMsg = Common.Resources.ContentMessage.InValid,
            //        ErrorCode = Common.Enums.ErrorCode.InvalidData,
            //        Data = validateFailuresCustom
            //    };
            //}
            //else
            //{
            if (_baseDL.InsertMultiple(records) > 0)
            {
                return new ServiceResult
                {
                    IsSuccess = true,
                    UserMsg = Common.Resources.ContentMessage.S_Post,
                    Data = records
                };
            }
            else
            {
                return new ServiceResult
                {
                    IsSuccess = false,
                    DevMsg = Common.Resources.ContentMessage.NotFound,
                    UserMsg = Common.Resources.ContentMessage.E_Post,
                    ErrorCode = Common.Enums.ErrorCode.NotFound,
                };
                //}
            }
        }

        /// <summary>
        /// Thực hiện sửa nhiều bản ghi
        /// </summary>
        /// <param name="recordList"></param>
        /// <returns></returns>
        /// Author: NVDUC (15/05/2023)
        public ServiceResult UpdateMultiple(IEnumerable<T> recordList)
        {
            if (_baseDL.UpdateMultiple(recordList) > 0)
            {
                return new ServiceResult
                {
                    IsSuccess = true,
                    UserMsg = Common.Resources.ContentMessage.S_Put,
                    Data = recordList
                };
            }
            else
            {
                return new ServiceResult
                {
                    IsSuccess = false,
                    DevMsg = Common.Resources.ContentMessage.NotFound,
                    UserMsg = Common.Resources.ContentMessage.E_Put,
                    ErrorCode = Common.Enums.ErrorCode.NotFound,
                };
            }
        }

        /// <summary>
        /// Xoá bản ghi theo Id
        /// </summary>
        /// <param name="recordId"></param>
        /// <returns>Mã trạng thái thành công hay thất bại</returns>
        /// Author: NVDUC (23/3/2023)
        public ServiceResult DeleteRecordById(Guid recordId)
        {
            if (_baseDL.DeleteRecordById(recordId) > 0)
            {
                return new ServiceResult
                {
                    IsSuccess = true,
                    DevMsg = ContentMessage.S_Delete,
                    UserMsg = ContentMessage.S_Delete,
                    Data = recordId,
                };
            }
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
        /// Hàm validate chung kiểm tra những trường bắt buộc nhập dữ liệu
        /// </summary>
        /// <param name="record"></param>
        /// <returns>Danh sách các trường để trống</returns>
        /// Author: NVDUC (8/4/2023)
        public List<string> ValidateRequestData(T? record)
        {
            var validateFailures = new List<string>();
            var properties = typeof(T).GetProperties();
            foreach (var property in properties)
            {
                var propertyName = property.Name;
                var propertyValue = property.GetValue(record);

                // Kiểm tra attribute required
                var requiredAttribute = (RequiredAttribute?)property.GetCustomAttributes(typeof(RequiredAttribute), false).FirstOrDefault();
                if (requiredAttribute != null && String.IsNullOrEmpty(propertyValue?.ToString()))
                {
                    validateFailures.Add(propertyName + Common.Resources.ContentMessage.Required);
                }

                // Kiểm tra attribute MaxLength
                var maxLengthAttribute = (MaxLengthAttribute?)property.GetCustomAttributes(typeof(MaxLengthAttribute), false).FirstOrDefault();
                if (maxLengthAttribute != null && propertyValue?.ToString().Length > maxLengthAttribute.Length)
                {
                    validateFailures.Add(propertyName + Common.Resources.ContentMessage.MaxLength);
                }

                // Kiểm tra attribute Date
                var dateAttribute = (DateValidAttribute?)property.GetCustomAttributes(typeof(DateValidAttribute), false).FirstOrDefault();
                if (dateAttribute != null && propertyValue is DateTime dateValue && dateValue > DateTime.Now)
                {
                    validateFailures.Add(propertyName + Common.Resources.ContentMessage.DateValidte);
                }
            }
            return validateFailures;
        }

        /// <summary>
        /// Hàm validate cho từng đối tượng 
        /// </summary>
        /// <param name="record">đối tượng cần kiểm tra</param>
        /// <returns>Danh sách các lỗi</returns>
        /// Author: NVDUC (8/4/2023)
        public virtual List<string> ValidateRequestDataCustom(T record)
        {
            return new List<string>();
        }

        /// <summary>
        /// Thực hiện tìm kiếm phân trang danh sách bản ghi
        /// </summary>
        /// <param name="search"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns>Các bản ghi trùng với điều kiện</returns>
        /// Author: NVDUC (23/3/2023)
        public PagingResult<T> GetPagingRecord(
        string? search,
        int? pageNumber,
        int? pageSize
       )
        {
            return _baseDL.GetPagingRecord(search, pageNumber, pageSize);
        }

        #endregion
    }
}
