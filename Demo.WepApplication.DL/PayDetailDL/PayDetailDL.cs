using Dapper;
using Demo.WebApplication.Common.Entities;
using Demo.WepApplication.DL.BaseDL;
using Npgsql;
using System;
using System.Collections.Generic;
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
        public IEnumerable<PayDetail> GetAllById(Guid payId)
        {
            // Chuẩn bị tên stored
            string queryString = $"select * from func_paydetail_getallbyid('{payId}')";
            // Khởi tạo kết nối với DB

            using (var postgreSQL = new NpgsqlConnection(DatabaseContext.ConnectionString))
            {
                postgreSQL.Open();

                // Thực hiện câu lệnh sql
                var records = postgreSQL.Query<PayDetail>(queryString, commandType: System.Data.CommandType.Text);

                postgreSQL.Close();
                return records.ToList();
            }
        }
    }
}
