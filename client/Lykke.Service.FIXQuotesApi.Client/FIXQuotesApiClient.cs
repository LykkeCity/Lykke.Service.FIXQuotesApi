using System;
using Common.Log;

namespace Lykke.Service.FIXQuotesApi.Client
{
    public class FIXQuotesApiClient : IFIXQuotesApiClient, IDisposable
    {
        private readonly ILog _log;

        public FIXQuotesApiClient(string serviceUrl, ILog log)
        {
            _log = log;
        }

        public void Dispose()
        {
            //if (_service == null)
            //    return;
            //_service.Dispose();
            //_service = null;
        }
    }
}
