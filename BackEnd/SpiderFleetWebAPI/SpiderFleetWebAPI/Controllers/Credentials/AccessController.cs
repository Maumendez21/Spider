using CredencialSpiderFleet.Models.Credentials;
using CredencialSpiderFleet.Models.Log;
using CredencialSpiderFleet.Models.Useful;
using SpiderFleetWebAPI.Models.Request.Credentials;
using SpiderFleetWebAPI.Models.Response.Credentials;
using SpiderFleetWebAPI.Utils.Log;
using SpiderFleetWebAPI.Utils.User;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

namespace SpiderFleetWebAPI.Controllers.Credentials
{
    public class AccessController : ApiController
    {
        [AllowAnonymous]
        [HttpPost]
        [Route("api/access/login")]
        public async System.Threading.Tasks.Task<LoginCredentialsResponse> AccessLogin([FromBody] LoginCredentialsRequest LoginCredentials)
        {
            LoginCredentialsResponse loginCredetialsResponse = new LoginCredentialsResponse();
            UseFul use = new UseFul();
            Token token = new Token();

            try
            {


                if(!(new UserDao()).Exists(LoginCredentials.Username.Trim()))
                {
                    loginCredetialsResponse.success = false;
                    loginCredetialsResponse.messages.Add("Usuario o Contaseña incorrecta, favor de validar");
                    loginCredetialsResponse.refresh_token = "";
                    return loginCredetialsResponse;
                }

                string hierarchy = string.Empty;
                hierarchy = (new UserDao()).ReadUserHierarchy(LoginCredentials.Username.Trim());
                int validacion = 0;
                if (!hierarchy.Equals("/"))
                {
                    hierarchy = use.hierarchyPrincipalToken(hierarchy);
                    validacion = token.Validation(hierarchy);
                }
                else
                {
                    validacion = 1;
                }

                if(validacion > 0)
                {
                    var _token = await token.GetToken(LoginCredentials.Username, UseFul.MD5Cifrar(LoginCredentials.Password));

                    if (_token.access_token == null)
                    {
                        loginCredetialsResponse.success = false;
                        loginCredetialsResponse.messages.Add("Usuario o Contaseña incorrecta, favor de validar");
                        loginCredetialsResponse.refresh_token = "";

                    }
                    else
                    {

                        loginCredetialsResponse.success = true;
                        loginCredetialsResponse.messages.Add("Acceso correcto");
                        loginCredetialsResponse.access_token = _token.access_token;
                        loginCredetialsResponse.refresh_token = _token.refresh_token;
                        //loginCredetialsResponse.expires_in = _token.expires_in;
                        loginCredetialsResponse.expires_in = 21600;

                        CredencialSpiderFleet.Models.User.UserLogin user = new CredencialSpiderFleet.Models.User.UserLogin();
                        user = token.GetDataUser(LoginCredentials.Username);
                        List<string> listPermission = new List<string>();
                        listPermission = token.GetPermission(LoginCredentials.Username);

                        loginCredetialsResponse.name = user.Name + " " + user.LastName;
                        loginCredetialsResponse.role = user.DescripcionRole;
                        loginCredetialsResponse.idu = user.IdU;
                        loginCredetialsResponse.image = user.Image;
                        loginCredetialsResponse.spider = user.Spider;
                        loginCredetialsResponse.listPermissions = listPermission;

                        LogAccess access = new LogAccess();
                        access.Username = LoginCredentials.Username;
                        access.Function = "Login";

                        (new LogAccessDao()).Create(access);
                    }
                }
                else
                {
                    loginCredetialsResponse.success = false;
                    loginCredetialsResponse.messages.Add("Su cuenta ha sido desactivada, favor de contactar a su Administrador");
                    loginCredetialsResponse.refresh_token = "";
                }
                
            }
            catch (Exception ex)
            {
                loginCredetialsResponse.success = false;
                loginCredetialsResponse.messages.Add(ex.ToString());

                return loginCredetialsResponse;
            }

            return loginCredetialsResponse;
        }

        //[AllowAnonymous]
        //[HttpGet]
        //[Route("api/access/token")]
        //public async System.Threading.Tasks.Task<IHttpActionResult> GetToken(string Username, string Password)
        //{
        //    Token token = new Token();

        //    var _token = await token.GetToken(Username,Password);

       //    return Ok(_token);
       // }

        [AllowAnonymous]
        [HttpGet]
        [Route("api/access/refreshtoken")]
        public async System.Threading.Tasks.Task<IHttpActionResult> GetNewToken(string token)
        {
            RefreshToken tokenRefresh = new RefreshToken();

            var _token = await tokenRefresh.GetNewToken(token);

            return Ok(_token);
        }

        [Authorize]
        [HttpGet]
        [Route("api/data/authenticate")]
        public IHttpActionResult GetForAuthenticate()
        {
            var identity = (ClaimsIdentity)User.Identity;
            return Ok("Hello " + identity.Name);
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        [Route("api/data/authorize")]
        public IHttpActionResult GetForAdmin()
        {
            var identity = (ClaimsIdentity)User.Identity;
            var roles = identity.Claims
                        .Where(c => c.Type == ClaimTypes.Role)
                        .Select(c => c.Value);
            return Ok("Hello " + identity.Name + " Role: " + string.Join(",", roles.ToList()));
        }
    }
}
