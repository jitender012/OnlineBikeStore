using System.Web.Mvc;
using OnlineBikeStore.Models;

namespace OnlineBikeStore.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult UnauthorisedAccessError(string msg)
        {
            return View("UnauthorisedAccessError", new ErrorViewModel { Message = msg });
        }
    }
}