using Demo.WebApplication.Common.Enums;

namespace Demo.WebApplication.Common.Entities.DTO
{
    public class ErrorResult
    {
        /// <summary>
        /// Trả về mã lỗi
        /// </summary>
        public ErrorCode ErrorCode { get; set; }

        /// <summary>
        /// Trả về thông tin lỗi cho devloper
        /// </summary>
        public String DevMsg { get; set; }

        /// <summary>
        /// Trả về thông tin lỗi cho người dùng
        /// </summary>
        public String UserMsg { get; set; }

        /// <summary>
        /// Thông tin lỗi bổ sung
        /// </summary>
        public object MoreInfo { get; set; }

        /// <summary>
        /// Mã bổ sung khả năng theo dõi lịch sử của yêu cầu một cách chi tiết
        /// </summary>
        public string TraceId { get; set; }
    }
}
