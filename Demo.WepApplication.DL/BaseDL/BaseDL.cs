using Dapper;
using Demo.WebApplication.Common.Entities;
using Demo.WebApplication.Common.Entities.DTO;
using Demo.WepApplication.DL.EmployeeDL;
using MySqlConnector;
using Npgsql;
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
            string storedFunctionName = $"Func_{typeof(T).Name}_getall()";

            string queryString = $"select * from {storedFunctionName}";

            // Khởi tạo kết nối với DB
            using var postgreSQL = new NpgsqlConnection(DatabaseContext.ConnectionString);
            postgreSQL.Open();

            // Thực hiện câu lệnh sql
            var records = postgreSQL.Query<T>(queryString, commandType: System.Data.CommandType.Text);

            postgreSQL.Close();
            return records.ToList();
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
            string queryString = $"select * from func_{typeof(T).Name}_get_by_id(@id_{typeof(T).Name}_select)";

            // Chuẩn bị tham số đầu vào
            var parameters = new DynamicParameters();
            parameters.Add($"@id_{typeof(T).Name}_select", recordId);

            // Khởi tạo kết nối tới database
            using var postgreSQL = new NpgsqlConnection(DatabaseContext.ConnectionString);
            postgreSQL.Open();

            // Thực hiện câu lệnh sql
            var record = postgreSQL.QueryFirstOrDefault<T>(queryString, parameters, commandType: System.Data.CommandType.Text);

            postgreSQL.Close();
            return record;
        }

        /// <summary>
        /// Thực hiện thêm mới bản ghi
        /// </summary>
        /// <param name="newRecord">entity</param>
        /// <returns>Id của bản ghi được thêm mới</returns>
        /// Author: NVDUC (23/3/2023)
        public T InsertRecord(T newRecord)
        {
            // Chuẩn bị tên stored
            string functionName = $"func_{typeof(T).Name}_add";

            // Build query string 
            var stringParam = new StringBuilder("");
            //Guid newRecordId = Guid.NewGuid();

            // Chuẩn bị tham số đầu vào
            var parameters = new DynamicParameters();
            var properties = typeof(T).GetProperties();

            foreach (var property in properties)
            {
                var propName = property.Name;
                var propValue = property.GetValue(newRecord);
                parameters.Add($"@v_{propName}", propValue);

                //if (propName == $"{typeof(T).Name}_id".ToLower())
                //{
                //    stringParam.Append($"'{newRecordId}'");
                //}

                //else if (property.PropertyType == typeof(Guid?) && propValue is null or (object)"")
                //{
                //    stringParam.Append(",null");
                //}
                //else if (propName == "modified_date" || propName == "created_date")
                //{
                //    stringParam.Append($",'{DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss")}'");
                //}
                //else
                //{
                //    if (property.PropertyType == typeof(int?))
                //    {
                //        stringParam.Append($",{propValue}");
                //    }
                //    else
                //    {
                //        stringParam.Append($",'{propValue}'");
                //    }
                //}
                if (propName == "modified_by")
                {
                    stringParam.Append($"@v_{propName}");
                }
                else
                {
                    stringParam.Append($"@v_{propName},");
                }
            }

            string stringQuery = $"select * from {functionName}({stringParam})";
            // Khởi tạo kết nối với DB
            using var postgreSQL = new NpgsqlConnection(DatabaseContext.ConnectionString);
            postgreSQL.Open();
            // Thực hiện câu lệnh sql
            var record = postgreSQL.QueryFirstOrDefault<T>(stringQuery, parameters, commandType: System.Data.CommandType.Text);

            postgreSQL.Close();

            return record;
        }

        /// <summary>
        /// Cập nhật thông tin bản ghi theo Id
        /// </summary>
        /// <param name="recordId"></param>
        /// <param name="newRecord"></param>
        /// <returns>Trả về số bản ghi bị ảnh hưởng</returns>
        /// Author: NVDUC (23/3/2023)
        public T UpdateRecordById(Guid recordId, T newRecord)
        {
            // Chuẩn bị tên stored
            string functionName = $"func_{typeof(T).Name}_update_by_id";

            // Build query string 
            var stringParam = new StringBuilder("");

            var properties = typeof(T).GetProperties();

            foreach (var property in properties)
            {
                var propName = property.Name;
                var propValue = property.GetValue(newRecord);

                if (propName == $"{typeof(T).Name}_id".ToLower())
                {
                    stringParam.Append($"'{recordId}'");
                }

                else if (property.PropertyType == typeof(Guid?) && propValue is null or (object)"")
                {
                    stringParam.Append(",null");
                }
                else if (propName == "modified_date" || propName == "created_date")
                {
                    stringParam.Append($",'{DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss")}'");
                }
                else
                {
                    if (property.PropertyType == typeof(int?))
                    {
                        stringParam.Append($",{propValue}");
                    }
                    else
                    {
                        stringParam.Append($",'{propValue}'");
                    }
                }
            }

            string stringQuery = $"select * from {functionName}({stringParam})";

            // Khởi tạo kết nối với DB
            using var postgreSQL = new NpgsqlConnection(DatabaseContext.ConnectionString);
            postgreSQL.Open();
            // Thực hiện câu lệnh sql
            var record = postgreSQL.QueryFirstOrDefault<T>(stringQuery, commandType: System.Data.CommandType.Text);

            postgreSQL.Close();

            return record;
        }

        /// <summary>
        /// Xoá bản ghi theo Id
        /// </summary>
        /// <param name="recordId"></param>
        /// <returns>Mã trạng thái thành công hay thất bại</returns>
        /// Author: NVDUC (23/3/2023)
        public T DeleteRecordById(Guid recordId)
        {
            //Chuẩn bị tên stored
            string queryString = $"SELECT * FROM func_{typeof(T).Name}_delete_by_id(@id_{typeof(T).Name}_select)";

            // Chuẩn bị tham số đầu vào
            var parameters = new DynamicParameters();
            parameters.Add($"@id_{typeof(T).Name}_select", recordId);

            // Khởi tạo kết nối với DB
            using var postgreSQL = new NpgsqlConnection(DatabaseContext.ConnectionString);
            postgreSQL.Open();

            var result = postgreSQL.QueryFirstOrDefault<T>(queryString, parameters, commandType: System.Data.CommandType.Text);
            postgreSQL.Close();
            return result;
        }
        public object GetPagingRecord(string? search, int? pageNumber, int? pageSize)
        {
            //Chuẩn bị tên stored
            string storedFunctionName = $"func_{typeof(T).Name}_getpaging";

            search ??= "";

            string queryString = $"select * from {storedFunctionName}('{search}', {pageNumber}, {pageSize})";

            // Khởi tạo kết nối với DB
            using (var postgreSQL = new NpgsqlConnection(DatabaseContext.ConnectionString))

            {
                postgreSQL.Open();

                // Thưc hiện câu lệnh sql
                var records = postgreSQL.Query(queryString, commandType: System.Data.CommandType.Text);

                postgreSQL.Close();
                return records;
            }
        }
        #endregion
    }
}