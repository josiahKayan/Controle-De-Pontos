using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Apassos.DataAccess
{
    public class GlobalVariablesCommon
    {
        public static string ENVIRONMENT = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();
    }
}