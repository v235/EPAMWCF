using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DBProvider
{
    public interface IDBProvider
    {
        void DeployOrUpdateDB(string connectionString);
    }
}
