using Demo.WebApplication.API.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static NUnit.Framework.Constraints.Tolerance;

namespace Demo.WebApplication.API.UnitTests
{
    /// <summary>
    /// Viết unittest sử dụng dữ liệu fake NSubstitute
    /// </summary>
    internal class EmployeesControllerTests
    {
        /// <summary>
        /// Fake dữ liệu các tham số đầu vào để 
        /// kiểm tra hàm xử lý thành công có đúng hay không
        /// </summary>
        /// Author: NVDUC (20/3/2023)
        //[Test]
        //public void GetEmployeeById_ExistsEmployee_ReturnSuccess()
        //{
        //    // Arrange: chuẩn bị dữ liệu đầu vào
        //    var employeeId = new Guid("9346c9f2-14da-46db-815a-0a2bd559e5d6");
        //    var employee = new Employee
        //    {
        //        EmployeeId = employeeId,
        //        EmployeeCode = "NV-0011",
        //        FullName = "Nguyễn Văn Đức",
        //        DepartmentId = new Guid("4e272fc4-7875-78d6-7d32-6a1673ffca7c"),

        //    };
        //    var expectedResult = new ObjectResult(employee);
        //    expectedResult.StatusCode = 200;
        //    var fakeEmployeeRepository = Substitute.For<IEmployeeRepository>();
        //    fakeEmployeeRepository.QueryFirstOrDefault(
        //        Arg.Any<IDbConnection>(),
        //        Arg.Any<string>(),
        //        Arg.Any<object>(),
        //        Arg.Any<IDbTransaction>(),
        //        Arg.Any<int>(),
        //        Arg.Any<CommandType?>()).Returns(employee);
        //    var employeeController = new EmployeesController(fakeEmployeeRepository);

        //    // Act
        //    var actualResult = (ObjectResult)employeeController.GetEmployeeById(employeeId);

        //    // Assert
        //    Assert.AreEqual(expectedResult.StatusCode, actualResult.StatusCode);
        //    Assert.AreEqual(employee.FullName, ((Employee)actualResult.Value).FullName);
        //    Assert.AreEqual(employee.EmployeeCode, ((Employee)actualResult.Value).EmployeeCode);
        //    Assert.AreEqual(employee.DepartmentId, ((Employee)actualResult.Value).DepartmentId);

        //}

        /// <summary>
        /// Fake dữ liệu các tham số đầu vào để 
        /// kiểm tra hàm xử lý không tìm thấy có đúng hay không
        /// </summary>
        //[Test]
        //public void GetEmployeeById_ExistsEmployee_ReturnNotFound()
        //{
        //    // Arrange: chuẩn bị dữ liệu đầu vào
        //    var employeeId = new Guid("9346c9f2-14da-46db-815a-0a2bd559e5d6");
        //    var expectedResult = new NotFoundResult();
        //    var fakeEmployeeRepository = Substitute.For<IEmployeeRepository>();
        //    fakeEmployeeRepository.QueryFirstOrDefault(
        //        Arg.Any<IDbConnection>(),
        //        Arg.Any<string>(),
        //        Arg.Any<object>(),
        //        Arg.Any<IDbTransaction>(),
        //        Arg.Any<int>(),
        //        Arg.Any<CommandType?>()).Returns((Employee)null);
        //    var employeeController = new EmployeesController(fakeEmployeeRepository);

        //    // Act

        //    var actualResult = (NotFoundResult)employeeController.GetEmployeeById(employeeId);

        //    // Assert
        //    Assert.AreEqual(expectedResult.StatusCode, actualResult.StatusCode);
        //}


        /// <summary>
        /// Unit test cho hàm thêm mới nhân viên khi thành công
        /// </summary>
        /// Author: NVDUC (20/3/2023)
        //[Test]
        //public void AddEmployee_ValidEmployee_ReturnSuccess()
        //{
        //    // Arrange: chuẩn bị dữ liệu đầu vào
        //    var employee = new Employee
        //    {
        //        EmployeeId = new Guid("23471b1e-cdb6-4a8e-ae69-f37fe3934cff"),
        //        EmployeeCode = "NV-8899",
        //        FullName = "Nguyễn Văn Đức",
        //        DepartmentId = new Guid("4e272fc4-7875-78d6-7d32-6a1673ffca7c"),
        //    };

        //    // Kết quả mong đợi của hàm
        //    var expectedResult = new ObjectResult(employee);
        //    expectedResult.StatusCode = 201;
        //    // Tạo ra một đối tượng fake implement IEmployeeRepository
        //    var fakeEmployeeRepository = Substitute.For<IEmployeeRepository>();
        //    fakeEmployeeRepository.Execute(
        //        Arg.Any<IDbConnection>(),
        //        Arg.Any<string>(),
        //        Arg.Any<object>(),
        //        Arg.Any<IDbTransaction>(),
        //        Arg.Any<int>(),
        //        Arg.Any<CommandType?>()).Returns(201);

        //    // Tạo ra đối tượng employeeController với đối tượng fake ở trên
        //    var employeeController = new EmployeesController(fakeEmployeeRepository);

        //    // Act
        //    // Thực thi hàm InsertEmployee và nhân kết quả về biến actualResult
        //    var actualResult = (ObjectResult)employeeController.InsertEmployee(employee);

        //    // Assert
        //    // So sánh kiểm tra kết quả
        //    Assert.AreEqual(expectedResult.StatusCode, actualResult.StatusCode);
        //    // Additional checks for returned object type and expected value
        //    Assert.AreEqual(employee.FullName, ((Employee)actualResult.Value).FullName);
        //    Assert.AreEqual(employee.EmployeeCode, ((Employee)actualResult.Value).EmployeeCode);
        //    Assert.AreEqual(employee.DepartmentId, ((Employee)actualResult.Value).DepartmentId);

        //}



        /// <summary>
        /// Unit test cho hàm thêm mới nhân viên khi thất bại
        /// </summary>
        /// Author: NVDUC (20/3/2023)
        //[Test]
        //public void AddEmployee_InvalidInput_ReturnsBadRequest()
        //{
        //    //Arrange: chuẩn bị dữ liệu đầu vào
        //    var employee = new Employee
        //    {
        //        EmployeeId = new Guid("23471b1e-cdb6-4a8e-ae69-f37fe3934cff"),
        //        EmployeeCode = "NV-8899",
        //        FullName = "",
        //        DepartmentId = new Guid("4e272fc4-7875-78d6-7d32-6a1673ffca7c"),    
        //    };
        //    var expectedResult = new ObjectResult(employee);
        //    expectedResult.StatusCode = 400;
        //    var fakeEmployeeRepository = Substitute.For<IEmployeeRepository>();
        //    fakeEmployeeRepository.Execute(
        //        Arg.Any<IDbConnection>(),
        //        Arg.Any<string>(),
        //        Arg.Any<object>(),
        //        Arg.Any<IDbTransaction>(),
        //        Arg.Any<int>(),
        //        Arg.Any<CommandType?>()).Returns(0); // Trả về giá trị 0 tương ứng với lỗi thêm mới nhân viên không thành công
        //    var employeeController = new EmployeesController(fakeEmployeeRepository);

        //    // Act
        //    var actualResult = (ObjectResult)employeeController.InsertEmployee(employee);

        //    // Assert
        //    Assert.AreEqual(expectedResult.StatusCode, actualResult.StatusCode);
        //}


        ///// <summary>
        ///// Unit test khi trùng mã nhân viên
        ///// </summary>
        ///// Author: NVDUC (20/3/2023)
        //[Test]
        //public void AddNewEmployee_DuplicateEmployee_ReturnConflict()
        //{
        //    // Arrange: chuẩn bị dữ liệu đầu vào
        //    var employee = new Employee
        //    {
        //        EmployeeId = Guid.NewGuid(),
        //        EmployeeCode = "NV-0011",
        //        FullName = "Trần Thị Hồng Hạnh",
        //        DepartmentId = new Guid("142cb08f-7c31-21fa-8e90-67245e8b283e")
        //    };
        //    var expectedResult = new ConflictObjectResult("Employee already exists.");
        //    var fakeEmployeeRepository = Substitute.For<IEmployeeRepository>();
        //    fakeEmployeeRepository.Execute(
        //    Arg.Any<IDbConnection>(),
        //    Arg.Any<string>(),
        //    Arg.Any<object>(),
        //    Arg.Any<IDbTransaction>(),
        //    Arg.Any<int>(),
        //    Arg.Any<CommandType?>()).Returns(0);

        //    fakeEmployeeRepository.QueryFirstOrDefault(
        //    Arg.Any<IDbConnection>(),
        //    Arg.Any<string>(),
        //    Arg.Any<object>(),
        //    Arg.Any<IDbTransaction>(),
        //    Arg.Any<int>(),
        //    Arg.Any<CommandType?>()).Returns(new Employee
        //    {
        //        EmployeeCode = "NV-0011"
        //    });
        //    var employeeController = new EmployeesController(fakeEmployeeRepository);

        //    // Act
        //    var actualResult = (ConflictObjectResult)employeeController.InsertEmployee(employee);

        //    // Assert
        //    Assert.AreEqual(expectedResult.StatusCode, actualResult.StatusCode);

        //}
    }
}
