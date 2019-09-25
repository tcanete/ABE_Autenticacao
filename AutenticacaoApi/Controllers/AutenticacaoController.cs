using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutenticacaoApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace AutenticacaoApi.Controllers {
    [Route ("api/v1/[controller]")]
    [ApiController]
    public class AutenticacaoController : ControllerBase {
        // GET api/autenticacao/create
        /// <summary>
        /// Cria um novo usuario
        /// </summary>
        [HttpPost ("create")]
        public ActionResult CreateUser ([FromBody] UserCreateDTO userRequest) {
            if (!Startup.createdUsers.Any (u => u.Username.Equals (userRequest.Username))) {
                Startup.createdUsers.Add (new User {
                    Id = Startup.createdUsers.Count + 1,
                    Username = userRequest.Username,
                    Password = userRequest.Password
                });
                return Ok ();
            } else {
                return BadRequest ();
            }
        }

        // GET api/autenticacao/check
        /// <summary>
        /// Verifica se o token é valido
        /// </summary>
        [HttpPost ("check/{token}")]
        public ActionResult<User> CheckAuthetication (string token) {
            var user = Startup.loggedUsers.Where (u => u.Token.Equals (token)).FirstOrDefault ();

            if (user != null) {
                return user;
            } else {
                return NotFound ();
            }
        }

        // GET api/autenticacao/login
        /// <summary>
        /// Realiza o login do usuario e gera um token
        /// </summary>
        [HttpPost ("login")]
        public ActionResult<User> Login ([FromBody] UserCreateDTO userRequest) {

            var databaseUser = Startup.createdUsers
                .Where (u => u.Username.Equals (userRequest.Username) && u.Password.Equals (userRequest.Password))
                .FirstOrDefault ();

            if (databaseUser != null) {
                databaseUser.Token = new Guid().ToString();
                Startup.loggedUsers.Add (databaseUser);

                return databaseUser;
            } else {
                return NotFound ();
            }
        }

        // POST api/autenticacao/logout
        /// <summary>
        /// Realiza o logout do usuario
        /// </summary>
        [HttpPost ("logout/{token}")]
        public ActionResult Logout (string token) {
            var loggedUser = Startup.loggedUsers
                .Where (u => u.Token.Equals (token))
                .FirstOrDefault ();

            if (loggedUser != null) {
                Startup.loggedUsers.Remove (loggedUser);
                return Ok ();
            } else {
                return BadRequest ();
            }
        }
    }
}