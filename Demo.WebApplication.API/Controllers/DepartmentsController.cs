using Demo.WebApplication.API.Entities.DTO;
using Demo.WebApplication.API.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using Dapper;
using Demo.WebApplication.API.Database;

namespace Demo.WebApplication.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        #region Field
        private IDepartmentRepository _departmentRepository;
        #endregion

        #region Constructor
        public DepartmentsController(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }
        #endregion

        /// <summary>
        /// Lấy thông tin của tất cả phòng ban
        /// </summary>
        /// <returns>Toàn bộ phòng ban</returns>
        /// Created by: NVDUC (9/3/2023)
        [HttpGet]
        public IActionResult GetAllDepartment()
        {
            try
            {
                // Validate dữ liệu

                // Chuẩn bị tên stored
                string storedProcedureName = "Proc_department_GetAll";
                // Chuẩn bị các tham số đầu vào

                //Khởi tạo kết nối tới database
                using (var dbConnection = _departmentRepository.GetOpenConnection())
                {
                    // Thực hiện câu lệnh sql
                    var departments = _departmentRepository.Query(dbConnection, storedProcedureName, commandType: System.Data.CommandType.StoredProcedure);
                    // Thành công
                    if (departments != null)
                    {
                        return Ok(departments);
                    }
                    // Thất bại
                    else
                    {
                        return NotFound();
                    }
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                return StatusCode(500, new
                {
                    ErrorCode = Enums.ErrorCode.Exception,
                    DevMsg = Resources.StatusMessage.DevMsg_Exception,
                    UserMsg = Resources.StatusMessage.UserMsg_Exception,
                    TraceId = HttpContext.TraceIdentifier,
                });
            }
        }

        /// <summary>
        /// Api lấy thông tin chi tiết của 1 nhân viên
        /// </summary>
        /// <param name="departmentId">ID nhân viên muốn lấy</param>
        /// <returns>Đối tượng nhân viên</returns>
        /// Created by: NVDUC (9/3/2023)
        [HttpGet("{departmentId}")]
        public IActionResult GetDeparmentById([FromRoute] Guid departmentId)
        {
            try
            {
                // Validate dữ liệu

                // Chuẩn bị tên stored
                string storedProcedureName = "Proc_department_GetById";

                // Chuẩn bị tham số đầu vào
                var parameters = new DynamicParameters();
                parameters.Add("@v_departmentId", departmentId);

                // Khởi tạo kết nối với DB
                using (var dbConnection = _departmentRepository.GetOpenConnection())
                {
                    // Thực hiện câu lệnh Sql 
                    var department = _departmentRepository.QueryFirstOrDefault(dbConnection, storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);
                    if (department != null)
                    {
                        return StatusCode(200, department);
                    }
                    else
                    {
                        return NotFound();
                    }
                }

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                return StatusCode(500, new
                {
                    ErrorCode = Enums.ErrorCode.Exception,
                    DevMsg = Resources.StatusMessage.DevMsg_Exception,
                    UserMsg = Resources.StatusMessage.UserMsg_Exception,
                    TraceId = HttpContext.TraceIdentifier,
                });
            }
        }

        /// <summary>
        /// Sửa thông tin phòng ban theo id
        /// </summary>
        /// <param name="departmentId"></param>
        /// <returns>Thông báo kết quả</returns>
        /// Created By: NVDUC (13/3/2023)
        [HttpPut("{departmentId}")]
        public IActionResult UpdateDepartmentById([FromRoute] Guid departmentId, Department newDepartment)
        {
            try
            {
                // Validate dữ liệu

                // Chuẩn bị tên stored
                string storedProcedureName = "Proc_department_UpdateById";

                // Chuẩn bị các tham số đầu vào
                var parameters = new DynamicParameters();
                parameters.Add("@v_departmentId", departmentId);
                parameters.Add("@v_departmentCode", newDepartment.DepartmentCode);
                parameters.Add("@v_departmentName", newDepartment.DepartmentName);

                //Khởi tạo kết nối tới database
                using (var dbConnection = _departmentRepository.GetOpenConnection())
                {
                    // Thực hiện câu lệnh sql
                    var result = _departmentRepository.Execute(dbConnection, storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);

                    return StatusCode(200, result);
                }
            }
            catch (Exception ex)
            {
                //transaction.Rollback();
                //throw;
                Console.WriteLine(ex.Message);
                return StatusCode(500, new
                {
                    ErrorCode = Enums.ErrorCode.Exception,
                    DevMsg = Resources.StatusMessage.DevMsg_Exception,
                    UserMsg = Resources.StatusMessage.UserMsg_Exception,
                    TraceId = HttpContext.TraceIdentifier,
                });
            }
        }

        /// <summary>
        /// Api thêm một phòng ban mới mới
        /// </summary>
        /// <param name="department"></param>
        /// <returns>Trả về trạng thái thêm mới</returns>
        /// Created By: NVDUC (13/3/2023)
        [HttpPost]
        public IActionResult InsertDepartment([FromBody] Department department)
        {
            try
            {
                // Validate dữ liệu

                // Chuẩn bị tên stored
                string storedProcedureName = "Proc_department_Add";

                // Chuẩn bị tham số đầu vào
                var parameters = new DynamicParameters();
                //Guid departmentId = Guid.NewGuid();
                //parameters.Add("@v_departmentId", departmentId);
                parameters.Add("@v_departmentCode", department.DepartmentCode);
                parameters.Add("@v_departmentName", department.DepartmentName);

                // Kết nối với database
                using (var dbConnection = _departmentRepository.GetOpenConnection())
                {
                    // Thực hiện câu lệnh sql
                    var result = _departmentRepository.Execute(dbConnection, storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);

                    return StatusCode(200, result);
                }

            }
            catch (Exception ex)
            {
                //transaction.Rollback();
                //throw;
                Console.WriteLine(ex.Message);
                return StatusCode(500, new
                {
                    ErrorCode = Enums.ErrorCode.Exception,
                    DevMsg = Resources.StatusMessage.DevMsg_Exception,
                    UserMsg = Resources.StatusMessage.UserMsg_Exception,
                    TraceId = HttpContext.TraceIdentifier,
                });
            }

        }

        /// <summary>
        /// Xoá một phòng ban theo Id
        /// </summary>
        /// <param name="departmentId"></param>
        /// <returns>Thông báo kết quả xoá</returns>
        /// Created By : NVDUC (13/3/2023)
        [HttpDelete("{departmentId}")]
        public IActionResult DeleteEmployeeById([FromRoute] Guid departmentId)
        {
            try
            {
                // Validate dữ liệu

                // Chuẩn bị tên stored
                string storedProcedureName = "Proc_department_DeleteById";

                // Chuẩn bị tham số đầu vào
                var parameters = new DynamicParameters();
                parameters.Add("@v_departmentId", departmentId);

                // Kết nối với database
                using (var dbConnection = _departmentRepository.GetOpenConnection())
                {
                    var departmment = _departmentRepository.Query(dbConnection ,storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);

                    if (departmment != null)
                    {
                        return StatusCode(200, departmment);
                    } else
                    {
                        return NotFound();
                    }
                }
            }
            catch (Exception ex)
            {
                //transaction.Rollback();
                //throw;
                Console.WriteLine(ex.Message);
                return StatusCode(500, new
                {
                    ErrorCode = Enums.ErrorCode.Exception,
                    DevMsg = Resources.StatusMessage.DevMsg_Exception,
                    UserMsg = Resources.StatusMessage.UserMsg_Exception,
                    TraceId = HttpContext.TraceIdentifier,
                });
            }
        }
    }
}
