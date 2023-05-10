namespace Demo.WebApplication.Common.Entities.DTO
{
    public class PagingResult<T>
    {
        /// <summary>
        /// Danh sách bản ghi
        /// </summary>
        /// Created By: NVDUC (13/3/2023)
        public dynamic? ListRecord { get; set; }

        /// <summary>
        /// Tổng số bản ghi thoả mãn điều kiện
        /// </summary>
        /// Created By: NVDUC (13/3/2023)
        public dynamic? TotalRecord { get; set; }

        /// <summary>
        /// Tổng số tiên
        /// </summary>
        public decimal? TotalAmount { get; set; }
    }
}
