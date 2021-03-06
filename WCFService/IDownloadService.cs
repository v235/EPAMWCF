﻿using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using DAL.Model;
using Messages;
using System.IO;
using System.Net.Http;

namespace WCFService
{
    [ServiceContract (Namespace = "http://localhost:8780/WCFService")]
    public interface IDownloadService
    {
        [OperationContract]
        [WebInvoke(UriTemplate = "/download?url={url}", Method = "POST", ResponseFormat = WebMessageFormat.Json)]
        PlaceTask CreateNewTask(string url);

        [OperationContract]
        [WebGet(UriTemplate = "/download/{id}/status", ResponseFormat = WebMessageFormat.Json)]
        TaskStatus GetTaskStatus(string id);

        [OperationContract]
        [WebGet(UriTemplate = "/download/{id}/artifact", ResponseFormat = WebMessageFormat.Json)]
        Stream Download(string id);
    }
}
