using Dapper;
using Demo.WebApplication.Common.Entities;
using Demo.WepApplication.DL.BaseDL;
using Demo.WepApplication.DL.EmployeeDL;
using MySqlConnector;
using Npgsql;
using System;
using System.Collections.Generic;
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
        public IEnumerable<Account> GetAllByKey(string? search)
        {
            search ??= "";
            string queryString = $"select * from func_account_getall_by_key('{search}')";

            using var postgreSQL = new NpgsqlConnection(DatabaseContext.ConnectionString);
            postgreSQL.Open();

            var record = postgreSQL.Query<Account>(queryString, commandType: System.Data.CommandType.Text);
            postgreSQL.Close();
            return record.ToList();
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

            using var postgreSQL = new NpgsqlConnection(DatabaseContext.ConnectionString);
            postgreSQL.Open();

            var record = postgreSQL.Execute(queryString, commandType: System.Data.CommandType.Text);
            postgreSQL.Close();
            return record;
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

            using var postgreSQL = new NpgsqlConnection(DatabaseContext.ConnectionString);
            postgreSQL.Open();

            var record = postgreSQL.Query<Account>(queryString, commandType: System.Data.CommandType.Text);
            postgreSQL.Close();
            return record.ToList();
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

            using var postgreSQL = new NpgsqlConnection(DatabaseContext.ConnectionString);
            postgreSQL.Open();

            var record = postgreSQL.Execute(queryString, parameters, commandType: System.Data.CommandType.Text);
            postgreSQL.Close();
            return record;
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
    }
}
