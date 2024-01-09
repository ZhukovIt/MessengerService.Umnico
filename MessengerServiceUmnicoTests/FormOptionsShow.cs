using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessagesWebSender;
using MessengerService.Umnico;
using MessengerService.Umnico.Options;
using MessengerServiceUmnicoTests.Tests.Unit;
using Moq;
using Xunit;

namespace MessengerServiceUmnicoTests.Tests.EndToEnd
{
    public sealed class FormOptionsShow
    {
        [Fact]
        public void ShowCorrectForm()
        {
            MessengerServiceOptionsDomainModel _Options = MotherObject.CreateUmnicoOptionsDomainModel();
            FormOptions _FormOptions = new FormOptions(_Options);
            AbstractWebServerWorker _WebServerWorker = new UmnicoWebServerWorker();
            ChatMessagesWebSender _ChatMessagesWebSender = new ChatMessagesWebSender(_WebServerWorker);
            MessagesWebSenderOptions _WebSenderOptions = new MessagesWebSenderOptions()
            {
                Domain = _Options.Domain,
                Port = _Options.Port,
                Guid = _Options.Guid
            };
            _ChatMessagesWebSender.InitOptions(_WebSenderOptions);
            _FormOptions.SetChatMessagesWebSender(_ChatMessagesWebSender);


            _FormOptions.ShowDialog();
        }
        //-------------------------------------------------------------------------------------------------
        [Fact]
        public void ShowFailureForm()
        {
            MessengerServiceOptionsDomainModel _Options = MotherObject.CreateUmnicoOptionsDomainModel();
            _Options.Port = 1;
            _Options.Guid = Guid.NewGuid().ToString();
            FormOptions _FormOptions = new FormOptions(_Options);
            AbstractWebServerWorker _WebServerWorker = new UmnicoWebServerWorker();
            ChatMessagesWebSender _ChatMessagesWebSender = new ChatMessagesWebSender(_WebServerWorker);
            MessagesWebSenderOptions _WebSenderOptions = new MessagesWebSenderOptions()
            {
                Domain = _Options.Domain,
                Port = _Options.Port,
                Guid = _Options.Guid
            };
            _ChatMessagesWebSender.InitOptions(_WebSenderOptions);
            _FormOptions.SetChatMessagesWebSender(_ChatMessagesWebSender);


            _FormOptions.ShowDialog();
        }
        //-------------------------------------------------------------------------------------------------
    }
}
