using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DesignPatternCmsInlupp.Services
{
    public class Logger
    {
        public enum Actions{
            CallReceived,
            ViewCustomerPage,
            ListCustomersPage,
            ParametrarPage,
            CreatingCustomer,
            CreatingLoan

        };
        private static Logger logger = null;
        private Logger()
        {

        }
        public static Logger GetLogger()
        {
            if (logger == null)
            {
                logger = new Logger();
            }
            return logger;
        }
        public void LogAction(Actions action, string message)
        {
            System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("~/log.txt"),  $"{action.ToString()} - {DateTime.Now.ToString("yyyy-MM-dd HH:mm:SS")}  {message}\n");
        }
    }
}