using CredencialSpiderFleet.Models.Credentials;
using CredencialSpiderFleet.Models.DAO.Credentials;
using CredencialSpiderFleet.Models.Useful;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace SpiderFleetWebAPI.Models.OAuth
{
    public class MyAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated(); // 
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            CredentialsDAO credentialsDAO = new CredentialsDAO();
            AuthenticationRequest authenticationRequestDAO = new AuthenticationRequest();

            authenticationRequestDAO.Login = context.UserName;
            authenticationRequestDAO.Password = context.Password;

            try
            {
                string role = credentialsDAO.ReadLogin(authenticationRequestDAO);
                
                if (role.Length > 1)
                {
                    var identity = new ClaimsIdentity(context.Options.AuthenticationType);

                    identity.AddClaim(new Claim(ClaimTypes.Role, role));
                    identity.AddClaim(new Claim("username", context.UserName));
                    //identity.AddClaim(new Claim(ClaimTypes.Name, "Sourav Mondal"));
                    context.Validated(identity);

                    return;                    
                }
                else 
                {
                    context.SetError("invalid_grant", "Usuario o contraseña invalida, favor de verificar");
                    return;
                }
            }
            catch (Exception ex)
            {
                //response.Success = false;
                //response.Messages.Add(ex.Message);
                //return response;
                context.SetError(ex.Message);
            }
        }
    }
}