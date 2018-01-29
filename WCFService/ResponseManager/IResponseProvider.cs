using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace WCFService.ResponseManager
{
    public interface IResponseProvider
    {
        HttpStatusCode ResponseCreated(WebOperationContext currentContext);

        HttpStatusCode ResponseOk(WebOperationContext currentContext);

        HttpStatusCode ResponseBadRequest(WebOperationContext currentContext);

        string ResponseContentType(WebOperationContext currentContext);

        void ResponseAddHeaders(WebOperationContext currentContext, string name);
    }
}
