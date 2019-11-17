namespace AnubisDBMS.Infraestructure.ApiFilters
{
    /// <summary>
    ///     Establece que la solicitud esperada para el metodo debe ser Mime Multipart Content
    /// </summary>
    //public class MimeMultipartContentFilter : ActionFilterAttribute
    //{
    //    public string allowedExtensions;

    //    public override void OnActionExecuting(HttpActionContext actionContext)
    //    {
    //        if (!actionContext.Request.Content.IsMimeMultipartContent())
    //            throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
    //        if (!string.IsNullOrEmpty(allowedExtensions))
    //        {
    //            var fileExtensions = allowedExtensions.Split(',');
    //        }
    //    }

    //    public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
    //    {
    //    }
    //}
}