using Dapper;
using Demo.WebApplication.Common.Entities.DTO;
using Demo.WebApplication.Common.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.WepApplication.DL.BaseDL
{
    public interface IBaseDL<T>
    {
        /// <summary>
        /// Thực hiên mở kết nối tới database
        /// </summary>
        /// <returns></returns>
        /// Author: NVDUC (13/3/2023)
        public IDbConnection GetOpenConnection();

        /// <summary>
        /// Trả về danh sách bản ghi
        /// </summary>
        /// <returns>Danh sách bản ghi</returns>
        /// Author: NVDUC (23/3/2023)
        public IEnumerable<dynamic> GetAllRecord();

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
        public int UpdateRecordById(Guid recordId, T newRecord);

        /// <summary>
        /// Thực hiện thêm mới bản ghi
        /// </summary>
        /// <param name="newRecord"></param>
        /// <returns>Trạng thái của hành động thêm mới</returns>
        /// Author: NVDUC (23/3/2023)
        public int InsertRecord(T newRecord);

        /// <summary>
        /// Xoá bản ghi theo Id
        /// </summary>
        /// <param name="recordId"></param>
        /// <returns>Mã trạng thái thành công hay thất bại</returns>
        /// Author: NVDUC (23/3/2023)
        public int DeleteRecordById(Guid recordId);

        public bool CheckDuplicateCode(string recordCode, Guid recordId);


        /* /// <summary>
    /// Thay thế cho việc khởi tạo Record
    /// </summary>
    /// <returns></returns>
    /// Author: NVDUC (20/3/2023)
    public T QueryFirstOrDefault(IDbConnection cnn, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null);

    /// <summary>
    /// Thực thi câu lệnh sql và thay thế cho việc khởi tạo Record
    /// </summary>
    /// <param name="cnn"></param>
    /// <param name="sql"></param>
    /// <param name="param"></param>
    /// <param name="transaction"></param>
    /// <param name="commandTimeout"></param>
    /// <param name="commandType"></param>
    /// <returns></returns>
    /// Author: NVDUC (20/3/2023)
    public IEnumerable<T> Query(IDbConnection cnn, string sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null);

    /// <summary>
    /// Thực thi câu lệnh sql và thay thế cho việc khởi tạo Record
    /// </summary>
    /// <param name="cnn"></param>
    /// <param name="sql"></param>
    /// <param name="param"></param>
    /// <param name="transaction"></param>
    /// <param name="commandTimeout"></param>
    /// <param name="commandType"></param>
    /// <returns></returns>
    /// Author: NVDUC (20/3/2023)
    public int Execute(IDbConnection cnn, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null);

    /// <summary>
    /// Thực thi câu lệnh sql, trả về các đầu dữ liệu
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="param"></param>
    /// <param name="transaction"></param>
    /// <param name="commandTimeout"></param>
    /// <param name="commandType"></param>
    /// <returns></returns>
    /// Author: NVDUC (20/3/2023)
    public SqlMapper.GridReader QueryMultiple(IDbConnection cnn, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null);*/
    }
}
