using Dapper;
using Demo.WebApplication.Common.Entities;
using Demo.WebApplication.Common.Entities.DTO;
using MySqlConnector;
using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static Npgsql.PostgresTypes.PostgresCompositeType;
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
            try
            {
                string queryString = $"select * from {typeof(T).Name}";

                // Khởi tạo kết nối với DB
                using var postgreSQL = new NpgsqlConnection(DatabaseContext.ConnectionString);
                postgreSQL.Open();

                // Thực hiện câu lệnh sql
                var records = postgreSQL.Query<T>(queryString, commandType: System.Data.CommandType.Text);

                postgreSQL.Close();
                return records.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Lấy ra thông tin bản ghi
        /// </summary>
        /// <param name="recordId"></param>
        /// <returns>Thông tin của bản ghi đó</returns>
        /// Author: NVDUC (23/3/2023)
        public dynamic GetRecordById(Guid recordId)
        {
            try
            {
                // Chuẩn bị tên stored
                string idField = $"{typeof(T).Name}_id";
                string joinOption = "";
                string selectQuery = "*";
                if (typeof(T).Name == "Pay")
                {
                    idField = "pay_id";
                    joinOption = "left join supplier on object_id = supplier_id left join employee on employee.employee_id = pay.employee_id";
                    selectQuery = "pay.pay_id, pay.ref_date, pay.voucher_date, pay.voucher_number, pay.description, pay.quantity, employee.employee_id, employee.full_name, pay.object_id , supplier.supplier_code, COALESCE(pay.object_name, supplier.supplier_name) AS object_name, COALESCE(pay.address, supplier.address) AS address, COALESCE(pay.receiver, supplier.supplier_name) AS receiver";
                }
                string queryString = $"Select {selectQuery} from {typeof(T).Name} {joinOption} where {idField} = '{recordId}'";

                // Khởi tạo kết nối tới database
                using var postgreSQL = new NpgsqlConnection(DatabaseContext.ConnectionString);
                postgreSQL.Open();

                // Thực hiện câu lệnh sql
                var record = postgreSQL.QueryFirstOrDefault<dynamic>(queryString, commandType: System.Data.CommandType.Text);

                postgreSQL.Close();
                return record;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// Thực hiện thêm mới bản ghi
        /// </summary>
        /// <param name="newRecord">entity</param>
        /// <returns>Id của bản ghi được thêm mới</returns>
        /// Author: NVDUC (23/3/2023)
        public InsertResult InsertRecord(T newRecord)
        {
            Guid id = Guid.NewGuid();
            try
            {
                string tableName = typeof(T).Name;
                string columns = string.Join(", ", typeof(T).GetProperties().Select(p => p.Name));
                string values = string.Join(", ", typeof(T).GetProperties().Select(p =>
                {
                    if (p.Name == $"{tableName}_id".ToLower())
                    {
                        return $"'{id}'";
                    }
                    else if (p.Name == "created_date")
                    {
                        return $"'{(string)DateTime.Now.ToString("yyyy-MM-dd")}'";
                    }
                    else if (p.PropertyType == typeof(DateTime?))
                    {
                        var dateValue = (DateTime?)p.GetValue(newRecord);
                        if (dateValue.HasValue)
                        {
                            return $"'{(string)dateValue.Value.ToString("yyyy-MM-dd")}'";
                        }
                        else
                        {
                            return "null";
                        }
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
                var rowEffected = postgreSQL.Execute(queryString, commandType: System.Data.CommandType.Text);
                postgreSQL.Close();
                return new InsertResult
                {
                    IdInsert = id,
                    NumberEffect = rowEffected,
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Thực hiện thêm mới nhiều bản ghi
        /// </summary>
        /// <param name="recordList"></param>
        /// <returns></returns>
        /// Author: NVDUC (09/05/2023)
        public int InsertMultiple(IEnumerable<T> recordList)
        {
            using (var postgreSQL = new NpgsqlConnection(DatabaseContext.ConnectionString))
            {
                postgreSQL.Open();
                using (NpgsqlTransaction transaction = postgreSQL.BeginTransaction())
                {
                    try
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
                                    else if (p.Name == "created_date")
                                    {
                                        return $"'{(string)DateTime.Now.ToString("yyyy-MM-dd")}'";
                                    }
                                    else if (p.PropertyType == typeof(DateTime?))
                                    {
                                        var dateValue = (DateTime?)val;
                                        if (dateValue.HasValue)
                                        {
                                            return $"'{(string)dateValue.Value.ToString("yyyy-MM-dd")}'";
                                        }
                                        else
                                        {
                                            return "null";
                                        }
                                    }
                                    else
                                    {
                                        return $"'{val.ToString()}'";
                                    }
                                }
                            })) + ")";
                        }));

                        string queryString = $"INSERT INTO {tableName} ({columns}) VALUES {values}";
                        // Thực hiện câu lệnh sql
                        int rowEffected = postgreSQL.Execute(queryString, commandType: System.Data.CommandType.Text);
                        transaction.Commit();
                        postgreSQL.Close();
                        return rowEffected;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Thực hiện sửa nhiều bản ghi
        /// </summary>
        /// <param name="recordList"></param>
        /// <returns></returns>
        /// Author: NVDUC (15/05/2023)
        public int UpdateMultiple(IEnumerable<T> recordList)
        {
            using (var postgreSQL = new NpgsqlConnection(DatabaseContext.ConnectionString))
            {
                postgreSQL.Open();
                using (NpgsqlTransaction transaction = postgreSQL.BeginTransaction())
                {
                    try
                    {
                        string tableName = typeof(T).Name.ToLower();
                        string idColumn = $"{tableName}_id".ToLower();
                        string columns = string.Join(", ", typeof(T).GetProperties().Where(p => p.Name != idColumn).Select(p => $"\"{p.Name}\""));

                        int rowEffected = 0;

                        foreach (var record in recordList)
                        {
                            string values = string.Join(", ", typeof(T).GetProperties().Where(p => p.Name != idColumn).Select(p =>
                            {
                                object val = p.GetValue(record);
                                if (val == null)
                                {
                                    return "null";
                                }
                                else if (p.PropertyType == typeof(DateTime?))
                                {
                                    var dateValue = (DateTime?)val;
                                    if (dateValue.HasValue)
                                    {
                                        return $"'{(string)dateValue.Value.ToString("yyyy-MM-dd")}'";
                                    }
                                    else
                                    {
                                        return "null";
                                    }
                                }
                                else
                                {
                                    return $"'{val.ToString().Replace("'", "''")}'";
                                }
                            }));

                            object idValue = record.GetType().GetProperty(idColumn).GetValue(record);
                            string updateQuery = $"UPDATE \"{tableName}\" SET ({columns}) = ({values}) WHERE \"{idColumn}\" = '{idValue.ToString()}'";

                            using (var command = new NpgsqlCommand(updateQuery, postgreSQL))
                            {
                                command.CommandType = System.Data.CommandType.Text;
                                rowEffected += command.ExecuteNonQuery();
                            }
                        }

                        transaction.Commit();
                        postgreSQL.Close();
                        return rowEffected;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        transaction.Rollback();
                        throw;
                    }
                }
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

            string tableName = typeof(T).Name;
            string setValues = string.Join(", ", typeof(T).GetProperties().Select(p =>
            {
                if (p.Name == $"{tableName}_id".ToLower())
                {
                    return $"{p.Name} = '{recordId}'";
                }
                else if (p.Name == "modified_date")
                {
                    return $"{p.Name} = '{(string)DateTime.Now.ToString("yyyy-MM-dd")}'";
                }
                else if (p.PropertyType == typeof(DateTime?))
                {
                    var dateValue = (DateTime?)p.GetValue(newRecord);
                    if (dateValue.HasValue)
                    {
                        return $"{p.Name} = '{(string)dateValue.Value.ToString("yyyy-MM-dd")}'";
                    }
                    else
                    {
                        return $"{p.Name} = null";
                    }
                }
                else if (p.GetValue(newRecord) == null)
                {
                    return $"{p.Name} = null";
                }
                else
                {
                    return $"{p.Name} = '{p.GetValue(newRecord)}'";
                }

            })); // Lọc bỏ các giá trị null
            string queryString = $"UPDATE {tableName} SET {setValues} WHERE {tableName}_id = '{recordId}'";
            try
            {
                using var postgreSQL = new NpgsqlConnection(DatabaseContext.ConnectionString);
                postgreSQL.Open();
                // Thực hiện câu lệnh sql
                int rowEffected = postgreSQL.Execute(queryString, commandType: System.Data.CommandType.Text);
                return rowEffected;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return -1;
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
            try
            {
                // Chuẩn bị query string
                string queryString = $"Delete from {typeof(T).Name} where {typeof(T).Name}.{typeof(T).Name}_id = '{recordId}'";
                // Kết nối tới db
                using var postgresql = new NpgsqlConnection(DatabaseContext.ConnectionString);
                postgresql.Open();
                // Thực hiện query
                int rowEffected = postgresql.Execute(queryString, commandType: System.Data.CommandType.Text);

                postgresql.Close();
                return rowEffected;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return -1;
            }
        }

        /// <summary>
        /// Thực hiện tìm kiếm phân trang danh sách bản ghi
        /// </summary>
        /// <param name="search"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns>Các bản ghi trùng với điều kiện</returns>
        /// Author: NVDUC (23/3/2023)
        public PagingResult<T> GetPagingRecord(string? search, int? pageNumber, int? pageSize)
        {
            int? offset = (pageSize * pageNumber) - pageSize;
            search ??= "";
            dynamic queryCustom = BuildQueryCustom();
            string queryString = $"select {queryCustom.selectOption} from {typeof(T).Name} {queryCustom.joinOption} {queryCustom.search} order by {queryCustom.orderBy} desc limit {pageSize} offset {offset};";
            string getTotalRecord = $"select count(*) from {typeof(T).Name} {queryCustom.search};";
            string? getTotalAmount = null;
            if (typeof(T).Name == "Pay")
            {
                getTotalAmount = "select sum(paydetail.amount_money) from paydetail inner join pay on paydetail.pay_id = pay.pay_id where pay.description ilike ('%' || @search  || '%') "
                    + "or cast(pay.ref_date as text) ilike ('%' || @search  || '%') " + "or cast(pay.ref_date as text) ilike ('%' || @search  || '%') " + "or pay.voucher_number ilike ('%' || @search  || '%') " + "or pay.description ilike ('%' || @search  || '%');";
                getTotalRecord = $"select count(*) from {typeof(T).Name} " + queryCustom.joinOption + " " + queryCustom.search + ";";
            }
            try
            {
                using (var postgreSQL = new NpgsqlConnection(DatabaseContext.ConnectionString))
                {
                    postgreSQL.Open();
                    // Thực hiện truy vấn
                    string excuteQuery = queryString + getTotalRecord + getTotalAmount;
                    var resultSets = postgreSQL.QueryMultiple(excuteQuery, new { search, limit = pageSize, offset = offset }, commandType: CommandType.Text);
                    // Kiểm tra kết quả trả về
                    var data = resultSets.Read();
                    var totalRecord = resultSets.Read();
                    var totalAmount = getTotalAmount != null ? resultSets.Read() : null;

                    var result = new PagingResult<T>
                    {
                        ListRecord = data,
                        TotalRecord = totalRecord,
                        OptionResult = totalAmount,
                    };
                    postgreSQL.Close();
                    return result;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// Hàm tạo câu lệnh sql chung
        /// </summary>
        /// <returns>Đối tượng chứa các thành phần câu lệnh sql</returns>
        /// Author: NVDUC (12/05/2023)
        public virtual object BuildQueryCustom()
        {
            string whereClause = "";
            PropertyInfo[] properties = typeof(T).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                if (property.PropertyType == typeof(string))
                {
                    if (string.IsNullOrEmpty(whereClause))
                    {
                        whereClause += "where ";
                    }
                    else
                    {
                        whereClause += " or ";
                    }
                    whereClause += $"{typeof(T).Name}.{property.Name} ilike '%' || @search || '%'";
                }
            }
            return new
            {
                selectOption = "*",
                joinOption = "",
                orderBy = $"{typeof(T).Name}.created_date",
                search = whereClause,
            };
        }
        #endregion
    }
}