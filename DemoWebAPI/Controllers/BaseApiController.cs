using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace DemoWebAPI.Controllers
{
    public abstract class BaseApiController : ApiController
    {
        protected string m_ControllerName = "";
        public BaseApiController()
        {
            m_ControllerName = GetType().Name;
        }
    }
}