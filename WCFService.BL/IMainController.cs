using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DAL.Model;
using Messages;

namespace WCFService.BL
{
    public interface IMainController
    {
        PlaceTask Create(string url);

        TaskStatus GetStatus(string id);

        Stream Download(string id, ref string fileName);
    }
}
