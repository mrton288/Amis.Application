﻿using Demo.WebApplication.Common.Entities.DTO;
using Demo.WebApplication.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.WebApplication.BL.BaseBL
{
    public interface IBaseBL<T>
    {
        /// <summary>
        /// Trả về danh sách bản ghi
        /// </summary>
        /// <returns>Danh sách bản ghi</returns>
        /// Author: NVDUC (23/3/2023)
        public IEnumerable<T> GetAllRecord();

        /// <summary>
        /// Lấy ra thông tin bản ghi
        /// </summary>
        /// <param name="recordId"></param>
        /// <returns>Thông tin của bản ghi đó</returns>
        /// Author: NVDUC (23/3/2023)
        public T GetRecordById(Guid recordId);

        /// <summary>
        /// Cập nhật thông tin bản ghi theo Id
        /// </summary>
        /// <param name="recordId"></param>
        /// <param name="newRecord"></param>
        /// <returns>Trạng thái của hành động</returns>
        /// Author: NVDUC (23/3/2023)
        /// 
        public ServiceResult UpdateRecordById(Guid recordId, T newRecord);

        /// <summary>
        /// Thực hiện thêm mới bản ghi
        /// </summary>
        /// <param name="newRecord"></param>
        /// <returns>Trạng thái của hành động thêm mới</returns>
        /// Author: NVDUC (23/3/2023)
        public ServiceResult InsertRecord(T newRecord);

        /// <summary>
        /// Xoá bản ghi theo Id
        /// </summary>
        /// <param name="recordId"></param>
        /// <returns>Mã trạng thái thành công hay thất bại</returns>
        /// Author: NVDUC (23/3/2023)
        public ServiceResult DeleteRecordById(Guid recordId);

        /// <summary>
        /// Thực hiện tìm kiếm phân trang danh sách bản ghi
        /// </summary>
        /// <param name="search"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns>Các bản ghi trùng với điều kiện</returns>
        /// Author: NVDUC (23/3/2023)
        public object GetPagingRecord(
        string? search,
        int? pageNumber,
        int? pageSize
       );
    }
}
