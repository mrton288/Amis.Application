using Dapper;
using Demo.WebApplication.Common.Entities;
using Demo.WebApplication.Common.Entities.DTO;
using Demo.WepApplication.DL.BaseDL;
using Demo.WepApplication.DL.EmployeeDL;
using MySqlConnector;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.WepApplication.DL.AccountDL
{
    public class AccountDL : BaseDL<Account>, IAccountDL
    {
        /// <summary>
        /// Thực hiện lấy ra danh sách tài khoản theo keyword tìm kiếm
        /// </summary>
        /// <param name="search"></param>
        /// <returns>Danh sách tài khoản</returns>
        /// Author: NVDUC (29/04/2023)
        public PagingResult<Account> GetAllByKey(string? search)
        {
            search ??= "";
            string querySearch = "where account.account_number ilike ('%' || @search || '%') or account.account_name ilike ('%' || @search || '%')";
            string queryString = $"select * from account {querySearch} order by account.account_number asc;";
            string getTotalRecord = $"select count(*) from account {querySearch};";
            string excuteQuery = queryString + getTotalRecord;
            try
            {
                using var postgreSQL = new NpgsqlConnection(DatabaseContext.ConnectionString);
                postgreSQL.Open();
                var resultSets = postgreSQL.QueryMultiple(excuteQuery, new { search }, commandType: CommandType.Text);
                // Kiểm tra kết quả trả về
                var data = resultSets.Read();
                var totalRecord = resultSets.Read();

                var result = new PagingResult<Account>
                {
                    ListRecord = data,
                    TotalRecord = totalRecord,
                };
                postgreSQL.Close();
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

        }

        /// <summary>
        /// Thực hiện cập nhật trạng thái của tài khoản
        /// </summary>
        /// <param name="id"></param>
        /// <param name="statusUpdate"></param>
        /// <returns>Trạng thái mới được cập nhật</returns>
        /// Author: NVDUC (29/04/2023)
        public int UpdateStatus(Guid id, int statusUpdate)
        {
            string queryString = $"select * from func_account_update_status('{id}',{statusUpdate})";
            try
            {
                using var postgreSQL = new NpgsqlConnection(DatabaseContext.ConnectionString);
                postgreSQL.Open();

                var record = postgreSQL.Execute(queryString, commandType: System.Data.CommandType.Text);
                postgreSQL.Close();
                return record;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

        }

        /// <summary>
        /// Trả về danh sách các tài khoản con của tài khoản
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// Author: NVDUC (29/04/2023)
        public IEnumerable<Account> GetListChild(Guid id)
        {
            string queryString = $"select * from func_account_update_children('{id}')";
            try
            {
                using var postgreSQL = new NpgsqlConnection(DatabaseContext.ConnectionString);
                postgreSQL.Open();

                var record = postgreSQL.Query<Account>(queryString, commandType: System.Data.CommandType.Text);
                postgreSQL.Close();
                return record.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }

        }

        /// <summary>
        /// Thực hiện cập nhật từ tài khoản thường lên tài khoản cha
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// Author: NVDUC (29/04/2023)
        public int UpdateParent(Guid id, int isParent)
        {
            string queryString = $"select * from func_account_update_parent(@id, @isParent)";

            // Chuẩn bị các tham số đầu vào
            var parameters = new DynamicParameters();
            parameters.Add($"@id", id);
            parameters.Add($"@isParent", isParent);
            try
            {
                using var postgreSQL = new NpgsqlConnection(DatabaseContext.ConnectionString);
                postgreSQL.Open();

                var record = postgreSQL.Execute(queryString, parameters, commandType: System.Data.CommandType.Text);
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
        /// Check trùng số tài khoản
        /// </summary>
        /// <param name="accountNumber">Số tài khoản</param>
        /// <param name="accountId">Id tài khoản</param>
        /// <returns>Trả về true - trùng mã, false - không trùng</returns>
        /// Author: NVDUC (29/4/2023)
        public bool CheckDuplicateAccount(string accountNumber, Guid accountId)
        {
            // Chuẩn bị store
            string stringFunction = $"select account_number, account_id from account  WHERE account_number = @AccountNumber AND account_id != @AccountId";
            // Chuẩn bị các tham số đầu vào
            var parameters = new DynamicParameters();
            parameters.Add($"@AccountNumber", accountNumber);
            parameters.Add($"@AccountId", accountId);
            try
            {
                // Khởi tạo kết nối tới database
                using var postgreSql = new NpgsqlConnection(DatabaseContext.ConnectionString);
                postgreSql.Open();
                var result = postgreSql.QueryFirstOrDefault<string>(stringFunction, parameters, commandType: System.Data.CommandType.Text);
                if (result != null)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex) 
            { 
                Console.WriteLine(ex.Message); 
                throw; 
            }

        }

        /// <summary>
        /// Cập nhật trạng thái nhiều tài khoản
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="newStatus"></param>
        /// <returns></returns>
        /// Author: NVDUC (05/05/2023)
        public int UpdateMultipleStatus(Guid[] ids, int newStatus)
        {
            using var postgreSQL = new NpgsqlConnection(DatabaseContext.ConnectionString);
            postgreSQL.Open();

            using var transaction = postgreSQL.BeginTransaction();
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@listAccountId", ids);
                parameters.Add("@newStatus", newStatus);

                string queryString = $"select * from func_account_update_status_multiple(@listAccountId, @newStatus);";

                int result = postgreSQL.QueryFirstOrDefault<int>(queryString, parameters, transaction, commandType: System.Data.CommandType.Text);

                transaction.Commit();
                postgreSQL.Close();
                return result;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}
