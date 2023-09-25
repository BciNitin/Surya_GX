using ELog.Web.Core.Controllers;
using Microsoft.AspNetCore.Antiforgery;

namespace ELog.Web.Host.Controllers
{
    public class AntiForgeryController : PMMSControllerBase
    {
        private readonly IAntiforgery _antiforgery;

        public AntiForgeryController(IAntiforgery antiforgery)
        {
            _antiforgery = antiforgery;
        }

        //Method GetToken replaced with GenerateToken for readability
        public void GenerateToken()
        {
            _antiforgery.SetCookieTokenAndHeader(HttpContext);
        }
    }
}
