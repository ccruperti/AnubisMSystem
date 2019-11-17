using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AnubisDBMS.Infraestructure.Helpers
{
    public class ModalActionResult : ActionResult
    {
        public override void ExecuteResult(ControllerContext context)
        {
            if (context.HttpContext.Request.IsAjaxRequest())
            {
                if (!context.ParentActionViewContext.ViewData.ModelState.IsValid)
                {
                    context.HttpContext.Response.StatusCode = 400;
                }
            }
        }
    }
}
