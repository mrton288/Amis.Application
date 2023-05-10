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
using static Npgsql.Replication.PgOutput.Messages.RelationMessage;

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
        public int InsertRecord(T newRecord)
        {
            //// Chuẩn bị tên stored
            //string functionName = $"func_{typeof(T).Name}_add"; ;

            //// Build query string 
            //var stringParam = new StringBuilder("");
            //Guid newRecordId = Guid.NewGuid();

            //var properties = typeof(T).GetProperties();

            //foreach (var property in properties)
            //{
            //    var propName = property.Name;
            //    var propValue = property.GetValue(newRecord);

            //    if (propName == $"{typeof(T).Name}_id".ToLower())
            //    {
            //        stringParam.Append($"'{newRecordId}'");
            //    }

            //    else if (property.PropertyType == typeof(Guid?) && propValue is null or (object)"")
            //    {
            //        stringParam.Append(",null");
            //    }
            //    else if (propName == "modified_date" || propName == "created_date")
            //    {
            //        stringParam.Append($",'{DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss")}'");
            //    }
            //    else
            //    {
            //        if (property.PropertyType == typeof(int?) || property.PropertyType == typeof(decimal?))
            //        {
            //            stringParam.Append($",{propValue}");
            //        }
            //        else
            //        {
            //            stringParam.Append($",'{propValue}'");
            //        }
            //    }
            //}

            //string stringQuery = $"select * from {functionName}({stringParam})";

            //// Khởi tạo kết nối với DB
            //using var postgreSQL = new NpgsqlConnection(DatabaseContext.ConnectionString);
            //postgreSQL.Open();
            //// Thực hiện câu lệnh sql
            //var record = postgreSQL.QueryFirstOrDefault<T>(stringQuery, commandType: System.Data.CommandType.Text);

            //postgreSQL.Close();

            //return record;



            //string queryString = $"insert into {typeof(T).Name}";
            //string columns = "(";
            //string colValue = "(";
            //var properties = typeof(T).GetProperties();
            //int i = 0;
            //foreach (var property in properties)
            //{
            //    if (i != properties.Length - 1)
            //    {
            //        columns += property.Name + ',';
            //        if (i == 0)
            //        {
            //            string newId = Guid.NewGuid().ToString();
            //            colValue += $"'{newId}',";
            //        }
            //        else
            //        {
            //            if (property.GetValue(newRecord) == null)
            //            {
            //                colValue += "null" + ',';
            //            }
            //            else
            //            {
            //                if (property.PropertyType.Name.CompareTo("Int32") != 0)
            //                    colValue += $"'{property.GetValue(newRecord).ToString()}'" + ',';
            //                else
            //                    colValue += property.GetValue(newRecord).ToString() + ',';
            //            }
            //        }
            //    }
            //    else
            //    {
            //        columns += property.Name + ')';
            //        if (property.GetValue(newRecord) == null)
            //        {
            //            colValue += "null" + ')';
            //        }
            //        else
            //        {
            //            if (property.PropertyType.Name.CompareTo("Int32") != 0)
            //                colValue += $"'{property.GetValue(newRecord).ToString()}'" + ')';
            //            else
            //                colValue += property.GetValue(newRecord).ToString() + ')';
            //        }
            //    }
            //    i++;
            //}

            //queryString += columns + " values " + colValue + ';';
            string tableName = typeof(T).Name;
            string columns = string.Join(", ", typeof(T).GetProperties().Select(p => p.Name));
            string values = string.Join(", ", typeof(T).GetProperties().Select(p =>
            {
                if (p.Name == $"{tableName}_id".ToLower())
                {
                    return $"'{Guid.NewGuid()}'";
                }
                else if (p.GetValue(newRecord) == null)
                {
                    return "null";
                }
                else
                {
                    return $"'{p.GetValue(newRecord)}'";
                }

            }));

            string queryString = $"INSERT INTO {tableName} ({columns}) VALUES ({values})";
            using var postgreSQL = new NpgsqlConnection(DatabaseContext.ConnectionString);
            postgreSQL.Open();
            // Thực hiện câu lệnh sql
            var result = postgreSQL.Execute(queryString, commandType: System.Data.CommandType.Text);
            return result;
        }


        /// <summary>
        /// Thực hiện thêm mới nhiều bản ghi
        /// </summary>
        /// <param name="recordList"></param>
        /// <returns></returns>
        public int InsertMultiple(IEnumerable<T> recordList)
        {
            string tableName = typeof(T).Name.ToLower();
            string columns = string.Join(", ", typeof(T).GetProperties().Select(p => p.Name));
            string values = string.Join(", ", recordList.Select(record =>
            {
                return "(" + string.Join(", ", typeof(T).GetProperties().Select(p =>
                {
                    if (p.Name == $"{tableName}_id".ToLower())
                    {
                        return $"'{Guid.NewGuid()}'";
                    }
                    else
                    {
                        object val = p.GetValue(record);
                        if (val == null)
                        {
                            return "null";
                        }
                        else
                        {
                            return $"'{val.ToString()}'";
                        }
                    }
                })) + ")";
            }));

            string queryString = $"INSERT INTO {tableName} ({columns}) VALUES {values}";
            using var postgreSQL = new NpgsqlConnection(DatabaseContext.ConnectionString);
            postgreSQL.Open();
            // Thực hiện câu lệnh sql
            var result = postgreSQL.Execute(queryString, commandType: System.Data.CommandType.Text);
            return result;
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
                    if (property.PropertyType == typeof(int?) || property.PropertyType == typeof(decimal?))
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

        /// <summary>
        /// Thực hiện tìm kiếm phân trang danh sách bản ghi
        /// </summary>
        /// <param name="search"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns>Các bản ghi trùng với điều kiện</returns>
        /// Author: NVDUC (23/3/2023)
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

            //int? offset = (pageSize * pageNumber) - pageSize;
            //search ??= "";
            //string selectOption = "*";
            //string joinOption = "";
            //string? optionalQuery = null;
            //string queryString = $"select {selectOption} from {typeof(T).Name} {joinOption} order by {typeof(T).Name}.created_date desc limit {pageSize} offset {offset};";
            //string getTotalRecord = $"select count(*) from {typeof(T).Name};";
            //using (var postgreSQL = new NpgsqlConnection(DatabaseContext.ConnectionString))
            //{
            //    postgreSQL.Open();
            //    // Thực hiện truy vấn
            //    string excuteQuery = queryString + getTotalRecord;
            //    if (optionalQuery != null)
            //    {
            //        excuteQuery += optionalQuery;
            //    }
            //    var resultSets = postgreSQL.QueryMultiple(excuteQuery, commandType: CommandType.Text);
            //    // Kiểm tra kết quả trả về
            //    var data = resultSets.Read();
            //    var totalRecord = resultSets.Read();
            //    //var optionResult = optionalQuery != null ? resultSets.Read() : null;

            //    var result = new PagingResult<T>
            //    {
            //        ListRecord = data,
            //        TotalRecord = totalRecord,
            //    };
            //    postgreSQL.Close();
            //    return result;
            //}
        }
     
        #endregion
    }
}