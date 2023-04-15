using Dapper;
using Demo.WebApplication.Common.Entities;
using Demo.WebApplication.Common.Entities.DTO;
using Demo.WepApplication.DL.EmployeeDL;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.WepApplication.DL.BaseDL
{
    public class BaseDL<T> : IBaseDL<T>
    {
        #region Method
        /// <summary>
        /// Trả về danh sách bản ghi
        /// </summary>
        /// <returns>Danh sách bản ghi</returns>
        /// Author: NVDUC (23/3/2023)
        public IEnumerable<T> GetAllRecord()
        {
            // Chuẩn bị tên stored
            string storedProcedureName = $"Proc_{typeof(T).Name}_GetAll";

            // Chuẩn bị tham số đầu vào
            // Khởi tạo kết nối với DB
            using (var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                mySqlConnection.Open();

                // Thực hiện câu lệnh sql
                var records = mySqlConnection.Query<T>(storedProcedureName, commandType: System.Data.CommandType.StoredProcedure);

                mySqlConnection.Close();
                return records.ToList();
            }
        }

        /// <summary>
        /// Lấy ra thông tin bản ghi
        /// </summary>
        /// <param name="recordId"></param>
        /// <returns>Thông tin của bản ghi đó</returns>
        /// Author: NVDUC (23/3/2023)
        public T GetRecordById(Guid recordId)
        {
            // Chuẩn bị tên stored
            string storedProcedureName = $"Proc_{typeof(T).Name}_GetById";

            // Chuẩn bị tham số đầu vào
            var parameters = new DynamicParameters();
            parameters.Add($"@v_{typeof(T).Name}Id", recordId);

            // Khởi tạo kết nối tới database
            using (var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                mySqlConnection.Open();

                // Thực hiện câu lệnh sql
                var record = mySqlConnection.QueryFirstOrDefault<T>(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);

                mySqlConnection.Close();
                return record;
            }
        }

        /// <summary>
        /// Thực hiện thêm mới bản ghi
        /// </summary>
        /// <param name="newRecord">entity</param>
        /// <returns>Id của bản ghi được thêm mới</returns>
        /// Author: NVDUC (23/3/2023)
        public int InsertRecord(T newRecord)
        {
            // Validate dữ liệu 
            // Chuẩn bị tên stored
            string storedProcedureName = $"Proc_{typeof(T).Name}_Add";

            // Chuẩn bị tham số đầu vào
            var properties = typeof(T).GetProperties();
            var parameters = new DynamicParameters();
            foreach (var property in properties)
            {
                parameters.Add($"@v_{property.Name}", property.GetValue(newRecord));
            }

            // Khởi tạo kết nối với DB
            using (var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                mySqlConnection.Open();
                // Thực hiện câu lệnh sql

                var numberRecord = mySqlConnection.Execute(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);

                mySqlConnection.Close();
                return numberRecord;
            }
        }

        /// <summary>
        /// Cập nhật thông tin bản ghi theo Id
        /// </summary>
        /// <param name="recordId"></param>
        /// <param name="newRecord"></param>
        /// <returns>Trả về số bản ghi bị ảnh hưởng</returns>
        /// Author: NVDUC (23/3/2023)
        public int UpdateRecordById(Guid recordId, T newRecord)
        {
            // Chuẩn bị tên stored
            string storedProcedureName = $"Proc_{typeof(T).Name}_UpdateById";

            // Chuẩn bị tham số đầu vào
            var parameters = new DynamicParameters();
            var properties = typeof(T).GetProperties();

            foreach (var property in properties)
            {
                parameters.Add($"@v_{property.Name}", property.GetValue(newRecord));

            }
            // Khởi tạo kết nối với DB
            using (var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                mySqlConnection.Open();
                // Thực hiện câu lệnh sql
                var numberRecord = mySqlConnection.Execute(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);

                mySqlConnection.Close();

                return numberRecord;
            }
        }

        /// <summary>
        /// Xoá bản ghi theo Id
        /// </summary>
        /// <param name="recordId"></param>
        /// <returns>Mã trạng thái thành công hay thất bại</returns>
        /// Author: NVDUC (23/3/2023)
        public int DeleteRecordById(Guid recordId)
        {
            //Chuẩn bị tên stored
            string storedProcedureName = $"Proc_{typeof(T).Name}_DeleteById";

            // Chuẩn bị tham số đầu vào
            var parameters = new DynamicParameters();
            parameters.Add($"@v_{typeof(T).Name}Id", recordId);

            // Khởi tạo kết nối với DB
            using (var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                mySqlConnection.Open();

                var result = mySqlConnection.Execute(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);
                mySqlConnection.Close();
                return result;
            }
        }
        #endregion
    }
}