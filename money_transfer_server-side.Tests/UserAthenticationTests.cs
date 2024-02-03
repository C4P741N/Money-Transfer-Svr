using System;
using System.Net;
using money_transfer_server_side.Controllers;
using money_transfer_server_side.Models;
using money_transfer_server_side.EnumsFactory;
using money_transfer_server_side.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using money_transfer_server_side.JsonExtractors;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace money_transfer_server_side.Tests
{
    public class UserAthenticationTests()
    {
        [Fact]
        public async Task AuthenticateUser_Returns_Token_If_Success()
        {
            //Arrange
            var config = A.Fake<IConfiguration>();
            var jwtGenerator = A.Fake<IJwtGenerator>();
            var authenticationManager = A.Fake<IUserRepository>();

            // instance
            var controller = new UserAthentication(config, jwtGenerator, authenticationManager);

            var fakeHttpContext = new FakeHttpContext();
            controller.ControllerContext = new ControllerContext { HttpContext = fakeHttpContext };

            // Set up for the authenticationManager to return HttpStatusCode.Found (simulating success)
            A.CallTo(() => authenticationManager.Begin(A<UserLogin>._, A<IConfiguration>._))
                .Returns(HttpStatusCode.Found);

            // Set up the jwtGenerator to return a mock JwtModel
            var fakeJwtModel = new JwtModel();

            A.CallTo(() => jwtGenerator.CreateRefreshToken(A<UserLogin>._)).Returns(fakeJwtModel);

            // Act
            var model = new UserLogin
            {
                user = "Dancan",
                email = "dancan@gmail.com",
                pwd = "Dancan123!",
                authType = EnumsAtLarge.AuthTypes.Authentication
            };
            var actionResult = await controller.Authenticate(model);

            // Assert
            Assert.IsType<OkObjectResult>(actionResult);
            var okObjectResult = (OkObjectResult)actionResult;

            Assert.NotNull(okObjectResult.Value);
            //var resultValue = (JsonElement)okObjectResult.Value;

            //Assert.True(resultValue.TryGetProperty("Token", out var tokenProperty));
            //Assert.NotNull(tokenProperty.GetString());  // You can further assert the properties of the token as needed
        }
    }
}