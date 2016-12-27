
using Apassos.Models;
using System.Configuration;

namespace Apassos.DataAccess
{
    public abstract class BaseDataAccess
    {
        protected static TimesheetContext db;

        protected static string enviroment;

        public BaseDataAccess()
        {
            db = new TimesheetContext();
            enviroment = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();
        }
    }
}