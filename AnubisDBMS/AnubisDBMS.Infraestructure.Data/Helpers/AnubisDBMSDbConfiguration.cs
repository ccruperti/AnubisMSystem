using NLog;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Interception;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnubisDBMS.Infraestructure.Data.Helpers
{
    public class AnubisDBMSDbConfiguration : DbConfiguration
    {
        public AnubisDBMSDbConfiguration()
        {
            DbInterception.Add(new AnubisDBMSDbInterceptorLogger());
        }
    }
}
