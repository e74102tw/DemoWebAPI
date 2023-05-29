using DemoWebAPI.Enums;
using DemoWebAPI.Library;
using DemoWebAPI.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace DemoWebAPI.Tests
{
    [TestClass]
    public class TestCustomerController
    {
        [TestMethod]
        public void TestCipher()
        {
            string sText = "TestCipherText";
            string sEn = Cipher.Encrypt(sText);
            string sDe = Cipher.Decrypt(sEn);
            Assert.AreEqual(sText, sDe);
        }
        [TestMethod]
        public void TestPostParameterSuccess()
        {
            // Arrange
            var mockRequest = new CustomerPostRequestModel
            {
                CustID = "TestID",
                CustFirstName = "FirstName",
                CustLastName = "LastName",
                CustFullName = "FullName",
                Sex = "M",
                TelNo = "12345678",
                BirthDate = "1985/11/11"
            };

            var controller = new CustomerController
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };
            controller.Request.Content = new ObjectContent<CustomerPostRequestModel>(
                mockRequest, new JsonMediaTypeFormatter(), "application/json");
            controller.Validate(mockRequest);
            ResponseModel responseModelTemp = new ResponseModel();
            Methods.BaseValidation(controller.ModelState, mockRequest, responseModelTemp);
            // Assert
            Assert.AreEqual((int)ResultCode.Success, responseModelTemp.ResultCode);
        }
        [TestMethod]
        public void TestPostParameterFailure()
        {
            // Arrange
            var mockRequest = new CustomerPostRequestModel();

            var controller = new CustomerController
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };
            controller.Request.Content = new ObjectContent<CustomerPostRequestModel>(
                mockRequest, new JsonMediaTypeFormatter(), "application/json");
            controller.Validate(mockRequest);
            ResponseModel responseModelTemp = new ResponseModel();
            Methods.BaseValidation(controller.ModelState, mockRequest, responseModelTemp);
            // Assert
            Assert.AreEqual((int)ResultCode.WrongArgument, responseModelTemp.ResultCode);
        }
        [TestMethod]
        public void TestPutParameterSuccess()
        {
            // Arrange
            var mockRequest = new CustomerPutRequestModel
            {
                C_Seq = 1,
                CustID = "TestID",
                CustFirstName = "FirstName",
                CustLastName = "LastName",
                CustFullName = "FullName",
                Sex = "M",
                TelNo = "12345678",
                BirthDate = "1985/11/11"
            };

            var controller = new CustomerController
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };
            controller.Request.Content = new ObjectContent<CustomerPutRequestModel>(
                mockRequest, new JsonMediaTypeFormatter(), "application/json");
            controller.Validate(mockRequest);
            ResponseModel responseModelTemp = new ResponseModel();
            Methods.BaseValidation(controller.ModelState, mockRequest, responseModelTemp);
            // Assert
            Assert.AreEqual((int)ResultCode.Success, responseModelTemp.ResultCode);
        }
        [TestMethod]
        public void TestPutParameterFailure()
        {
            // Arrange
            var mockRequest = new CustomerPutRequestModel();

            var controller = new CustomerController
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };
            controller.Request.Content = new ObjectContent<CustomerPutRequestModel>(
                mockRequest, new JsonMediaTypeFormatter(), "application/json");
            controller.Validate(mockRequest);
            ResponseModel responseModelTemp = new ResponseModel();
            Methods.BaseValidation(controller.ModelState, mockRequest, responseModelTemp);
            // Assert
            Assert.AreEqual((int)ResultCode.WrongArgument, responseModelTemp.ResultCode);
        }
    }
}
