namespace Demo.WebApplication.Common.Enums
{
    public enum ErrorCode
    {
        /// <summary>
        /// Mã trả về khi ngoại lệ
        /// </summary>
        Exception = 1,

        /// <summary>
        /// Mã trả về khi dữ liệu không hợp lệ
        /// </summary>
        InvalidData = 2,

        /// <summary>
        /// Mã trả về khi dữ liệu trùng
        /// </summary>
        DuplicateData = 3,
    }
}
