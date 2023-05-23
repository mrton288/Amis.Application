using Dapper;
using Demo.WebApplication.Common.Entities;
using Demo.WebApplication.Common.Entities.DTO;
using Demo.WepApplication.DL.BaseDL;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.WepApplication.DL.PayDetailDL
{
    public class PayDetailDL : BaseDL<PayDetail>, IPayDetailDL
    {
        /// <summary>
        /// Lấy ra danh sách chi tiền chi tiết khi
        /// </summary>
        /// <param name="payId">Id của bản ghi chi tiền cha</param>
        /// <returns>Danh sách chi tiền chi tiết</returns>
        /// Author: NVDUC (25/04/2023)
        public PagingResult<PayDetail> GetAllById(Guid payId)
        {
            string queryWhere = $"where pd.pay_id = '{payId}'";
            string queryString = $"select pd.paydetail_id, pd.pay_id, pd.description, pd.credit_account, pd.debit_account, pd.amount_money, pd.object_id, COALESCE(pd.object_name, s.supplier_name) AS object_name, COALESCE(pd.object_code, s.supplier_code) AS object_code  from paydetail pd left join supplier s on pd.object_id = s.supplier_id {queryWhere};";
            string getTotalAmount = $"select sum(pd.amount_money) from paydetail pd {queryWhere};";
            string getTotalRecord = $"select count(*) from paydetail pd {queryWhere};";

            string excuteQuery = queryString + getTotalAmount + getTotalRecord;
            try
            {
                using var postgreSQL = new NpgsqlConnection(DatabaseContext.ConnectionString);
                postgreSQL.Open();
                var resultSets = postgreSQL.QueryMultiple(excuteQuery, commandType: CommandType.Text);
                // Kiểm tra kết quả trả về
                var data = resultSets.Read();
                var totalAmount = resultSets.Read();
                var totalRecord = resultSets.Read();

                var result = new PagingResult<PayDetail>
                {
                    ListRecord = data,
                    OptionResult = totalAmount,
                    TotalRecord = totalRecord,
                };
                postgreSQL.Close();
                return result;
            }
            catch (Exception)
            {
                throw;
            }
            
        }

        /// <summary>
        /// Xoá nhiều bản ghi theo danh sách id truyền vào
        /// </summary>
        /// <param name="recordList"></param>
        /// <returns>Số lượng Id xoá thành công</returns>
        /// Author: NVDUC (25/3/2023)
        public int DeleteMultiple(Guid[]? recordList)
        {
            int numberEffect = 0;
            using (var postgreSQL = new NpgsqlConnection(DatabaseContext.ConnectionString))
            {
                postgreSQL.Open();
                using (var transaction = postgreSQL.BeginTransaction())
                {
                    try
                    {
                        string sql = "DELETE FROM paydetail WHERE paydetail_id = ANY(@Ids)";
                        numberEffect = postgreSQL.Execute(sql, new { Ids = recordList }, transaction);

                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            return numberEffect;
        }
    }
}
