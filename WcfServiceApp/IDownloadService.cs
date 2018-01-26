using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using DAL.Model;
using Messages;
using System.IO;

namespace WcfServiceApp
{
    [ServiceContract]
    public interface IDownloadService
    {
        [OperationContract]
        [WebInvoke(UriTemplate = "/download?url={url}", Method = "POST", ResponseFormat = WebMessageFormat.Json)]
        PlaceTask CreateTask(string url);

        [OperationContract]
        [WebGet(UriTemplate = "/download/{id}/status", ResponseFormat = WebMessageFormat.Json)]
        TaskStatus GetTaskStatus(string id);

        [OperationContract]
        [WebGet(UriTemplate = "/download/{id}/artifact", ResponseFormat = WebMessageFormat.Json)]
        Stream Download(string id);
    }
}