using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MtxApi.Controllers
{
    public class HomeApiController : Controller
    {
        // GET: HomeApi
        public ActionResult Index()
        {
            return View();
        }
    }
}