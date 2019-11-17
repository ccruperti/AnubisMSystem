using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnubisDBMS.Infraestructure.Data.Security.EmailModels
{
    public class NewUserCratedEmailModel
    {
        public string Username { get; set; }
        public string MainRole { get; set; }
        public string Email { get; set; }
    }
}
