namespace Demo.WebApplication.Common.Entities.DTO
{
    public class PagingResult<T>
    {
        /// <summary>
        /// Danh sách bản ghi
        /// </summary>
        /// Created By: NVDUC (13/3/2023)
        public IEnumerable<T> ListRecord { get; set; }

        /// <summary>
        /// Lấy ra tổng số trang
        /// </summary>
        /// Created By: NVDUC (13/3/2023)
        public int TotalPage { get; set; }

        /// <summary>
        /// Tổng số bản ghi thoả mãn điều kiện
        /// </summary>
        /// Created By: NVDUC (13/3/2023)
        public int TotalRecord { get; set; }

        /// <summary>
        /// Chỉ số của trang hiện tại
        /// </summary>
        /// Created By: NVDUC (12/3/2023)
        public int? CurrentPage { get; set; }

        /// <summary>
        /// Số bản ghi trên trang hiện tại
        /// </summary>
        /// Created By: NVDUC (12/3/2023)
        public int? CurrentPageRecords { get; set; }
    }
}
