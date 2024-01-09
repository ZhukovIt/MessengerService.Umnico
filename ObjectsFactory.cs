using MessagesWebSender;
using MessengerService.Umnico.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessengerService.Umnico
{
    public static class ObjectsFactory
    {
        public static MessengerServiceOptionsXMLParser CreateMessengerServiceOptionsXMLParser()
        {
            MessengerServiceOptionsDomainModel _OptionsDomainModel = new MessengerServiceOptionsDomainModel();

            _OptionsDomainModel.Domain = "";
            _OptionsDomainModel.Guid = "7a93e9a3-ce8b-4b4c-937f-239e33c50b2d";
            _OptionsDomainModel.Port = 12341;

            return new MessengerServiceOptionsXMLParser(_OptionsDomainModel);
        }
        //----------------------------------------------------------------------------------------------------------
        public static ChatMessagesWebSender CreateChatMessagesWebSender(string _WebServerWorkerType)
        {
            AbstractWebServerWorker _WebServerWorker;

            switch (_WebServerWorkerType)
            {
                case WebServerWorkerTypes.Umnico:
                    _WebServerWorker = new UmnicoWebServerWorker();
                    break;
                default:
                    throw new NotImplementedException($"Объект {_WebServerWorkerType} не поддерживается!");
            }

            return CreateChatMessagesWebSender(_WebServerWorker);
        }
        //----------------------------------------------------------------------------------------------------------
        public static ChatMessagesWebSender CreateChatMessagesWebSender(AbstractWebServerWorker _WebServerWorker)
        {
            return new ChatMessagesWebSender(_WebServerWorker);
        }
        //----------------------------------------------------------------------------------------------------------
    }
}
