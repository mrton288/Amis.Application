using Dapper;
using Demo.WebApplication.API.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySqlConnector;
using System.Data;
using System.Data.Common;

namespace Demo.WebApplication.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        #region Field

        private IEmployeeRepository _employeeRepository;

        #endregion

        #region Constructor

        public EmployeesController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        #endregion

        /// <summary>
        /// Lấy thông tin của tất cả nhân viên
        /// </summary>
        /// <returns>Toàn bộ nhân viên</returns>
        /// Created by: NVDUC (9/3/2023)
        /// 
        [HttpGet]
        public IActionResult GetAllEmployee()
        {
            try
            {
                // Chuẩn bị tên stored
                string storedProcedureName = "Proc_employee_GetAll";

                // Chuẩn bị tham số đầu vào

                // Khởi tạo kết nối với DB
                var dbConnection = _employeeRepository.GetOpenConnection();

                // Thực hiện câu lệnh sql
                var employees = _employeeRepository.Query(dbConnection, storedProcedureName, commandType: System.Data.CommandType.StoredProcedure);

                // Kết quả trả về
                // Thành công
                if (employees != null)
                {
                    return Ok(employees);
                }
                // Thất bại
                else
                {
                    return NotFound();
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
        /// <param name="employeeId">ID nhân viên muốn lấy</param>
        /// <returns>Đối tượng nhân viên</returns>
        /// Created by: NVDUC (9/3/2023)
        [HttpGet("{employeeId}")]
        public IActionResult GetEmployeeById([FromRoute] Guid employeeId)
        {

            try
            {
                // Thành công 
                if (employee != null)
                {
                    return StatusCode(200, employee);
                }
                // Thất bại
                else
                {
                    return NotFound();
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


        [HttpPut("{employeeId}")]
        public IActionResult UpdateEmployeeById(Guid employeeId, Employee newEmployee)
        {
            try
            {
                // Validate dữ liệu

                // Chuẩn bị tên stored
                string storedProcedureName = "Proc_employee_UpdateById";

                // Chuẩn bị tham số đầu vào
                var parameters = new DynamicParameters();
                parameters.Add("@v_employeeId", employeeId);
                parameters.Add("@v_employeeCode", newEmployee.EmployeeCode);
                parameters.Add("@v_fullName", newEmployee.FullName);
                parameters.Add("@v_departmentId", newEmployee.DepartmentId);
                parameters.Add("@v_positionName", newEmployee.PositionName);
                parameters.Add("@v_dateOfBirth", newEmployee.DateOfBirth);
                parameters.Add("@v_gender", newEmployee.Gender);
                parameters.Add("@v_identityNumber", newEmployee.IdentityNumber);
                parameters.Add("@v_identityDate", newEmployee.IdentityDate);
                parameters.Add("@v_identityPlace", newEmployee.IdentityPlace);
                parameters.Add("@v_address", newEmployee.Address);
                parameters.Add("@v_phoneNumber", newEmployee.PhoneNumber);
                parameters.Add("@v_landline", newEmployee.Landline);
                parameters.Add("@v_email", newEmployee.Email);
                parameters.Add("@v_bankAccount", newEmployee.BankAccount);
                parameters.Add("@v_bankName", newEmployee.BankName);
                parameters.Add("@v_bankBranch", newEmployee.BankBranch);

                var id = employeeId;

                // Khởi tạo kết nối với DB

                using (var dbConnection = _employeeRepository.GetOpenConnection())
                {
                    //using (MySqlTransaction transaction = mySqlConnection.BeginTransaction())
                    //{
                    // Thực hiện câu lệnh sql
                    var result = _employeeRepository.Execute(dbConnection, storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);
                    //transaction.Commit();
                    //return StatusCode(200, result);
                    //}
                    if (result > 0)
                    {
                        Console.WriteLine("Employee with Id {0} has been updated.", employeeId);
                        return StatusCode(200, result);
                    }
                    else
                    {
                        Console.WriteLine("Employee with Id {0} not found.", employeeId);
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

        /// <summary>
        /// Api thêm một nhân viên mới
        /// </summary>
        /// <param name="employee"></param>
        /// <returns>Trả về trạng thái thêm mới</returns>
        /// Created By: NVDUC (13/3/2023)
        [HttpPost]
        public IActionResult InsertEmployee([FromBody] Employee newEmployee)
        {
            try
            {
                // Validate dữ liệu 
                // Chuẩn bị tên stored
                string storedProcedureName = "Proc_employee_Add";

                // Chuẩn bị tham số đầu vào
                var parameters = new DynamicParameters();
                //Guid newEmployeeId = Guid.NewGuid();
                //parameters.Add("@v_employeeId", newEmployeeId);
                parameters.Add("@v_employeeCode", newEmployee.EmployeeCode);
                parameters.Add("@v_fullName", newEmployee.FullName);
                parameters.Add("@v_departmentId", newEmployee.DepartmentId);
                parameters.Add("@v_positionName", newEmployee.PositionName);
                parameters.Add("@v_dateOfBirth", newEmployee.DateOfBirth);
                parameters.Add("@v_gender", newEmployee.Gender);
                parameters.Add("@v_identityNumber", newEmployee.IdentityNumber);
                parameters.Add("@v_identityDate", newEmployee.IdentityDate);
                parameters.Add("@v_identityPlace", newEmployee.IdentityPlace);
                parameters.Add("@v_address", newEmployee.Address);
                parameters.Add("@v_phoneNumber", newEmployee.PhoneNumber);
                parameters.Add("@v_landline", newEmployee.Landline);
                parameters.Add("@v_email", newEmployee.Email);
                parameters.Add("@v_bankAccount", newEmployee.BankAccount);
                parameters.Add("@v_bankName", newEmployee.BankName);
                parameters.Add("@v_bankBranch", newEmployee.BankBranch);

                // Khởi tạo kết nối với DB
                using (var dbConnection = _employeeRepository.GetOpenConnection())
                {
                    //using (MySqlTransaction transaction = mySqlConnection.BeginTransaction())
                    //{
                    // Thực hiện câu lệnh sql
                    var result = _employeeRepository.Execute(dbConnection, storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);
                    // Thành công 
                    if (result > 0)
                    {
                        Console.WriteLine("Employee created");
                        return StatusCode(201, result);
                    }
                    else
                    {
                        Console.WriteLine("Employee code not valid");
                        return StatusCode(400, 0);
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
        /// Xoá một nhân viên theo Id
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns>Thông báo kết quả xoá</returns>
        /// Created By : NVDUC (13/3/2023)
        [HttpDelete("{employeeId}")]
        public IActionResult DeleteEmployeeById([FromRoute] Guid employeeId)
        {
            try
            {
                //Chuẩn bị tên stored
                string storedProcedureName = "Proc_employee_DeleteById";

                // Chuẩn bị tham số đầu vào
                var parameters = new DynamicParameters();
                parameters.Add("@v_employeeId", employeeId);

                // Khởi tạo kết nối với DB
                using (var dbConnection = _employeeRepository.GetOpenConnection())
                {
                    var result = _employeeRepository.Query(dbConnection, storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);
                    if (result != null)
                    {
                        return StatusCode(200, result);
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
        /// Thực hiện phân trang và tìm kiếm theo mã, tên, số điện thoại nhân viên
        /// </summary>
        /// <param name="search">Từ khoá tìm kiếm</param>
        /// <param name="pageSize">Số bản ghi trên trang</param>
        /// <param name="pageNumber">Số của trang hiện tại</param>
        /// <returns></returns>
        /// Created by: NVDUC (12/3/2023)
        [HttpGet("filter")]
        public object FilterEmployee(
            [FromQuery] string? search,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 50
            )
        {
            try
            {
                // Validate dữ liệu

                // Chuẩn bị tên storeProcedure
                string storedProcedureName = "Proc_employee_GetPaging";

                // Chuẩn bị tham số đầu vào
                var parameters = new DynamicParameters();
                parameters.Add("@v_pageNumber", pageNumber);
                parameters.Add("@v_pageSize", pageSize);
                parameters.Add("@v_search", search);
                parameters.Add("@v_totalRecord", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
                parameters.Add("@v_totalPage", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);


                // Khởi tạo kết nối với DB
                using (var dbConnection = _employeeRepository.GetOpenConnection())
                {
                    // Thưc hiện câu lệnh sql
                    var multipleResults = _employeeRepository.QueryMultiple(dbConnection, storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);

                    // Lấy danh sách nhân viên
                    var employeeList = multipleResults.Read<Employee>().ToList();
                    // Lấy tổng số bản ghi
                    //int totalRecord = parameters.Get<int>("@totalRecord");
                    // Lấy tổng số trang
                    //int totalPages = parameters.Get<int>("@totalPage");

                    var result = new PagingResult
                    {
                        Data = employeeList,
                        //TotalRecord = totalRecord,
                        //TotalPages = totalPages,
                        CurrentPage = pageNumber,
                        CurrentPageRecords = pageSize,
                    };
                    return result;

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
    }
}
