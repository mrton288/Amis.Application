namespace Demo.WebApplication.Common.Enums
{
    public enum ErrorCode
    {
        /// <summary>
        /// Mã trả về nếu không có lỗi
        /// </summary>
        NoError = 0,

        /// <summary>
        /// Mã trả về khi ngoại lệ
        /// </summary>
        Exception = 1,

        /// <summary>
        /// Mã trả về khi dữ liệu nhập vào không hợp lệ
        /// </summary>
        InvalidData = 2,

        /// <summary>
        /// Mã trả về khi dữ liệu nhập vào bị trùng mã
        /// </summary>
        DuplicateData = 3,

        /// <summary>
        /// Mã trả về khi không tìm thấy dữ liệu
        /// </summary>
        NotFound = 4,
    }
}
