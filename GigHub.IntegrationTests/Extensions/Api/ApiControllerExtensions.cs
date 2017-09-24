using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Mvc;
using Moq;

namespace GigHub.IntegrationTests.Extensions.Api
{
    public static class ApiControllerExtensions
    {

        public static void MockCurrentUser(this ApiController controller, string userId, string username)
        {
            var identity = new GenericIdentity("chandan@domain.com");
            identity.AddClaim(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", username));
            identity.AddClaim(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", userId));
            var principal = new GenericPrincipal(identity, null);



            controller.ControllerContext = Mock.Of<HttpControllerContext>(ctx =>
                ctx.RequestContext == Mock.Of<HttpRequestContext>(httpctx => httpctx.Principal == principal));
        }
    }
}
