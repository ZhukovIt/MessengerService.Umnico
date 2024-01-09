using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MessagesWebSender
{
    public abstract class AbstractWebServerWorker
    {
        protected Mutex m_Mutex;
        protected int m_MutexWaitMS;
        //-------------------------------------------------------------------------
        public AbstractWebServerWorker()
        {
            m_Mutex = new Mutex();
            m_MutexWaitMS = 10000;
        }
        //-------------------------------------------------------------------------
        public abstract PersonsResponseData DefaultRoute(Uri _Uri);
        //-------------------------------------------------------------------------
        public abstract PersonsResponseData VerifyGuid(Uri _Uri, string _Guid);
        //-------------------------------------------------------------------------
        public abstract PersonsResponseData SendMessages(Uri _Uri, string _Guid, PersonsRequestData _InputRequest);
        //-------------------------------------------------------------------------
    }
}
