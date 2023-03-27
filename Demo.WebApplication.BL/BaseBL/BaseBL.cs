using Dapper;
using Demo.WebApplication.Common.Entities;
using Demo.WebApplication.Common.Entities.DTO;
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
        public IEnumerable<dynamic> GetAllRecord()
        {
            return _baseDL.GetAllRecord();
        }

        /// <summary>
        /// Lấy ra thông tin bản ghi
        /// </summary>
        /// <param name="RecordId"></param>
        /// <returns>Thông tin của bản ghi đó</returns>
        /// Author: NVDUC (23/3/2023)
        public T GetRecordById(Guid RecordId)
        {
            return _baseDL.GetRecordById(RecordId);
        }

        /// <summary>
        /// Cập nhật thông tin bản ghi theo Id
        /// </summary>
        /// <param name="RecordId"></param>
        /// <param name="newRecord"></param>
        /// <returns>Trạng thái của hành động</returns>
        /// Author: NVDUC (23/3/2023)
        /// 
        public int UpdateRecordById(Guid RecordId, T newRecord)
        {
            return _baseDL.UpdateRecordById(RecordId, newRecord);
        }

        /// <summary>
        /// Thực hiện thêm mới bản ghi
        /// </summary>
        /// <param name="newRecord"></param>
        /// <returns>Trạng thái của hành động thêm mới</returns>
        /// Author: NVDUC (23/3/2023)
        public virtual int InsertRecord(T newRecord)
        {
   
            //var properties = typeof(T).GetProperties();
            //foreach ( var property in properties)
            //{
            //    var propertyName = property.Name;
            //    var propertyValue = property.GetValue(newRecord);   
            //    var requiredAttribute = (RequiredAttribute?)property.GetCustomAttributes(typeof (RequiredAttribute), false).FirstOrDefault();

            //    if (requiredAttribute != null && String.IsNullOrEmpty(propertyValue.ToString()))
            //    {
            //        validateFailures.Add(propertyName);
            //    }
            //}   
            return _baseDL.InsertRecord(newRecord);
        }

        /// <summary>
        /// Xoá bản ghi theo Id
        /// </summary>
        /// <param name="RecordId"></param>
        /// <returns>Mã trạng thái thành công hay thất bại</returns>
        /// Author: NVDUC (23/3/2023)
        public int DeleteRecordById(Guid RecordId)
        {
            return _baseDL.DeleteRecordById(RecordId);
        }

        #endregion
    }
}
