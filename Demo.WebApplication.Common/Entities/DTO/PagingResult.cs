namespace Demo.WebApplication.Common.Entities.DTO
{
    public class PagingResult
    {
        /// <summary>
        /// Danh sách nhân viên 
        /// </summary>
        public List<Employee> Data { get; set; }

        /// <summary>
        /// Số bản ghi trên 1 trang
        /// </summary>
        //public int TotalPages { get; set; }

        /// <summary>
        /// Tổng số bản ghi thoả mãn điều kiện
        /// </summary>
        /// Created By: NVDUC (13/3/2023)
        //public int TotalRecord { get; set; }

        /// <summary>
        /// Chỉ số của trang hiện tại
        /// </summary>
        /// Created By: NVDUC (12/3/2023)
        public int CurrentPage { get; set; }

        /// <summary>
        /// Số bản ghi trên trang hiện tại
        /// </summary>
        /// Created By: NVDUC (12/3/2023)
        public int CurrentPageRecords { get; set; }
    }
}
