using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using System.Windows.Forms;

namespace MessagesWebSender
{
    public sealed class ChatMessagesWebSender
    {
        private AbstractWebServerWorker m_WebServerWorker;
        private MessagesWebSenderOptions m_Options;
        //--------------------------------------------------------
        public ChatMessagesWebSender(AbstractWebServerWorker _WebServerWorker)
        {
            m_WebServerWorker = _WebServerWorker;
            m_Options = null;
        }
        //--------------------------------------------------------
        public void InitOptions(MessagesWebSenderOptions _Options)
        {
            m_Options = _Options;
        }
        //--------------------------------------------------------
        public PersonsResponseData CheckConnection()
        {
            if (m_Options == null)
            {
                return HandleEmptyOptions();
            }

            Uri _UrlAddress = new Uri($"http://{m_Options.Domain}:{m_Options.Port}/");

            var _Result = m_WebServerWorker.DefaultRoute(_UrlAddress);

            return _Result;
        }
        //--------------------------------------------------------
        public PersonsResponseData VerifyGuid()
        {
            if (m_Options == null)
            {
                return HandleEmptyOptions();
            }

            Uri _UrlAddress = new Uri($"http://{m_Options.Domain}:{m_Options.Port}/api/UmnicoBulkMessagesWebServer/VerifyGuid");

            var _Result = m_WebServerWorker.VerifyGuid(_UrlAddress, m_Options.Guid);

            return _Result;
        }
        //--------------------------------------------------------
        public PersonsResponseData SendMessages(PersonsRequestData _RequestData)
        {
            if (m_Options == null)
            {
                return HandleEmptyOptions();
            }

            Uri _UrlAddress = new Uri($"http://{m_Options.Domain}:{m_Options.Port}/api/UmnicoBulkMessagesWebServer/SendMessages");

            var _Result = m_WebServerWorker.SendMessages(_UrlAddress, m_Options.Guid, _RequestData);

            if (!_Result.Success && _Result.InfoMessage != null)
            {
                MessageBox.Show(_Result.InfoMessage);
            }

            return _Result;
        }
        //--------------------------------------------------------
        private PersonsResponseData HandleEmptyOptions()
        {
            PersonsResponseData _Result = new PersonsResponseData();

            _Result.Success = false;
            _Result.InfoMessage = "Корректные настройки не заданы!";

            return _Result;
        }
        //--------------------------------------------------------
    }
}
