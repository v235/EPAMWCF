using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.IO;
using DAL.Model;
using DAL.Repository;
using System.ServiceModel.Web;
using Messages;
using NServiceBus;
using System.ServiceModel;
using WCFService.ResponseManager;
using WCFService.BL;

namespace WCFService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class DownloadService : IDownloadService
    {
        private readonly IMainController _mainController;
        private readonly IResponseProvider _responseProvider;

        public DownloadService(IMainController mainController,
            IResponseProvider responseProvider)
        {
            _mainController = mainController;
            _responseProvider = responseProvider;
        }

        public PlaceTask CreateNewTask(string url)
        {
            try
            {
                if (!string.IsNullOrEmpty(url))
                {
                    var newTask = _mainController.Create(url);
                    _responseProvider.ResponseCreated(WebOperationContext.Current);
                    return newTask;
                }
            }
            catch
            {
                _responseProvider.ResponseBadRequest(WebOperationContext.Current);
                return null;
            }

            _responseProvider.ResponseBadRequest(WebOperationContext.Current);
            return null;
        }

        public TaskStatus GetTaskStatus(string id)
        {
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    var taskStatus = _mainController.GetStatus(id);
                    _responseProvider.ResponseOk(WebOperationContext.Current);
                    return taskStatus;
                }
            }
            catch
            {
                _responseProvider.ResponseBadRequest(WebOperationContext.Current);
                return null;
            }

            _responseProvider.ResponseBadRequest(WebOperationContext.Current);
            return null;
        }

        public Stream Download(string id)
        {
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    string fileName = string.Empty;
                    var outPutStream = _mainController.Download(id, ref fileName);
                    if (outPutStream != null)
                    {
                        _responseProvider.ResponseContentType(WebOperationContext.Current);
                        _responseProvider.ResponseAddHeaders(WebOperationContext.Current, fileName);
                        _responseProvider.ResponseOk(WebOperationContext.Current);
                        return outPutStream;
                    }
                }
            }
            catch
            {
                _responseProvider.ResponseBadRequest(WebOperationContext.Current);
                return null;
            }

            _responseProvider.ResponseBadRequest(WebOperationContext.Current);
            return null;
        }
    }
}
