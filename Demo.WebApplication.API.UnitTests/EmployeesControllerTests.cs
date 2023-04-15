using Demo.WebApplication.API.Controllers;
using Demo.WebApplication.BL.BaseBL;
using Demo.WebApplication.BL.EmployeeBL;
using Demo.WebApplication.Common.Entities;
using Demo.WebApplication.Common.Entities.DTO;
using Demo.WebApplication.Common.Enums;
using Demo.WepApplication.DL.EmployeeDL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
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
    public class EmployeesControllerTests
    {
        private IEmployeeDL fakeEmployeeDL;
        private IEmployeeBL fakeEmployeeBL;
        private EmployeesController employeesController;

        [SetUp]
        public void Setup()
        {
            fakeEmployeeDL = Substitute.For<IEmployeeDL>();
            fakeEmployeeBL = Substitute.For<IEmployeeBL>();
            employeesController = Substitute.For<EmployeesController>(fakeEmployeeBL);
        }

        /// <summary>
        /// Test xoá nhiều trong trường hợp thành công có danh sách
        /// </summary>
        /// Author: NVDUC (20/3/2023)
        [Test]
        public void DeleteMultiple_ReturnSuccess()
        {
            // Arrange: chuẩn bị dữ liệu đầu vào
            Guid[] listId = new Guid[] {
                Guid.Parse("9346c9f2-14da-46db-815a-0a2bd559e5d6"),
                Guid.Parse("c14e0250-6316-4625-9da0-4902329c62ad"),
                Guid.Parse("eb35c915-762e-4062-b317-f6be0abd421d")
            };

            fakeEmployeeBL.DeleteMultiple(listId).Returns(1);

            // Act
            ServiceResult actualResult = employeesController.DeleteMultiple(listId);

            // Assert
            Assert.AreEqual(actualResult.IsSuccess, true);
        }


        /// <summary>
        /// Test xoá nhiều trong trường hợp thất bại không có danh sách
        /// </summary>
        /// Author: NVDUC (20/3/2023)
        [Test]
        public void DeleteMultiple_ReturnNotFound()
        {
            // Arrange: chuẩn bị dữ liệu đầu vào
            Guid[] listId = new Guid[] { };
            fakeEmployeeBL.DeleteMultiple(listId).Returns(0);

            // Act
            ServiceResult actualResult = employeesController.DeleteMultiple(listId);

            // Assert
            Assert.AreEqual(actualResult.IsSuccess, false);
        }


        /// <summary>
        /// Test xoá nhiều trong trường hợp thất bại lỗi xảy ra bên phía backend
        /// </summary>
        /// Author: NVDUC (20/3/2023)
        [Test]
        public void DeleteMultiple_ReturnException()
        {
            // Arrange: chuẩn bị dữ liệu đầu vào
            Guid[] listId = new Guid[] { 
                Guid.Parse("9346c9f2-14da-46db-815a-0a2bd559e5d6"),
                Guid.Parse("c14e0250-6316-4625-9da0-4902329c62ad"),
                Guid.Parse("eb35c915-762e-4062-b317-f6be0abd421d") };
            fakeEmployeeBL.DeleteMultiple(listId).Throws(new Exception("1"));

            // Act
            ServiceResult actualResult = employeesController.DeleteMultiple(listId);

            // Assert
            Assert.AreEqual(actualResult.ErrorCode, Common.Enums.ErrorCode.Exception);
        }
    }
}
