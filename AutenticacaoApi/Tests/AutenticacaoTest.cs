using System.Net;
using AutenticacaoApi.Controllers;
using AutenticacaoApi.Models;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace AutenticacaoApi.Tests
{
    public class AutenticacaoTest
    {
        [Fact]
        public void CadastrarUsuarioValido()
        {
            // Arrange
            var controller = new AutenticacaoController();
            var novoUsuario = new UserCreateDTO
            {
                Username = "usera",
                Password = "adasd"
            };

            // Act
            var response = controller.CreateUser(novoUsuario) as OkResult;
            var expected = (int)HttpStatusCode.OK;

            // Assert
            Assert.NotNull(response);
            Assert.Equal(expected, response.StatusCode);
        }

        [Fact]
        public void CadastrarUsuarioInvalido()
        {
            // Arrange
            var controller = new AutenticacaoController();
            Startup.createdUsers.Add(new User { Username = "user", Password = "asdasd" });
            var novoUsuario = new UserCreateDTO
            {
                Username = "user",
                Password = ""
            };

            // Act
            var response = controller.CreateUser(novoUsuario) as BadRequestResult;
            var expected = (int)HttpStatusCode.BadRequest;

            // Assert
            Assert.NotNull(response);
            Assert.Equal(expected, response.StatusCode);
        }

        [Fact]
        public void ChecarTokenValido()
        {
            // Arrange
            var controller = new AutenticacaoController();
            Startup.loggedUsers.Add(new User { Username = "user", Password = "asdasd", Token = "asd123" });
            var token = "asd123";

            // Act
            var response = controller.CheckAuthetication(token).Value as User;

            // Assert
            Assert.NotNull(response);
        }

        [Fact]
        public void RealizarLoginValido()
        {
            // Arrange
            var controller = new AutenticacaoController();
            Startup.createdUsers.Add(new User { Username = "asd", Password = "123" });
            var usuario = new UserCreateDTO
            {
                Username = "asd",
                Password = "123"
            };

            // Act
            var response = controller.Login(usuario).Value as User;

            // Assert
            Assert.NotNull(response);
        }

        [Fact]
        public void RealizarLogoutValido()
        {
            // Arrange
            var controller = new AutenticacaoController();
            Startup.loggedUsers.Add(new User { Username = "user", Password = "asdasd", Token = "asd" });
            var token = "asd";

            // Act
            var response = controller.Logout(token) as OkResult;
            var expected = (int)HttpStatusCode.OK;

            // Assert
            Assert.NotNull(response);
            Assert.Equal(expected, response.StatusCode);
        }
    }
}