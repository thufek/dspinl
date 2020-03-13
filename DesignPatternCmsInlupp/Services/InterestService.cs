using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DesignPatternCmsInlupp.Services
{
    public class InterestService : IInterestService
    {
        public decimal GetRiksbankensBaseRate()
        {
            int retries = 5;
            int sleepWhenFailBeforeRetry = 3000;
            while (retries > 0)
            {
                try
                {
                    //Fake slow call
                    System.Threading.Thread.Sleep(5000);
                    using (var c = new SweaWebService.SweaWebServicePortTypeClient())
                    {
                        var r = c.getLatestInterestAndExchangeRates(SweaWebService.LanguageType.sv, new[] { "SEDP2MSTIBOR" });
                        return Convert.ToDecimal(r.groups[0].series[0].resultrows[0].value);
                    }
                }
                catch
                {
                    System.Threading.Thread.Sleep(sleepWhenFailBeforeRetry);
                    retries--;
                }
            }
            return 0.125m;
        }
    }
}