using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace WCFService.ResponseManager
{
    public class ResponceProvider:IResponseProvider
    {
        public HttpStatusCode ResponseCreated(WebOperationContext currentContext)
        {
            return currentContext.OutgoingResponse.StatusCode = HttpStatusCode.Created;
        }

        public HttpStatusCode ResponseOk(WebOperationContext currentContext)
        {
            return currentContext.OutgoingResponse.StatusCode = HttpStatusCode.OK;
        }

        public HttpStatusCode ResponseInternalServerError(WebOperationContext currentContext)
        {
            return currentContext.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
        }

        public string ResponseContentType(WebOperationContext currentContext)
        {
            return currentContext.OutgoingResponse.ContentType = "application/octet-stream";
        }

        public void ResponseAddHeaders(WebOperationContext currentContext, string name)
        {
            currentContext.OutgoingResponse.Headers.Add("content-disposition", "inline; filename=" + name);
        }
    }
}
