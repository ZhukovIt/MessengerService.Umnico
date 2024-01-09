using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MessengerService.Umnico.Options;
using MessagesWebSender;

[assembly: InternalsVisibleTo("MessengerServiceUmnicoTests")]

namespace MessengerService.Umnico
{
    public sealed class MessengerServiceUmnico : IMessengerService
    {
        private ChatMessagesWebSender m_ChatMessagesWebSender;
        private MessengerServiceOptionsXMLParser m_Options;
        //------------------------------------------------------------------------------------------------
        public MessengerServiceOptionsXMLParser Options
        {
            get
            {
                return m_Options;
            }
        }
        //------------------------------------------------------------------------------------------------
        public MessengerServiceUmnico()
        {
            m_Options = ObjectsFactory.CreateMessengerServiceOptionsXMLParser();

            m_ChatMessagesWebSender = ObjectsFactory.CreateChatMessagesWebSender(WebServerWorkerTypes.Umnico);
        }
        //------------------------------------------------------------------------------------------------
        public MessengerServiceUmnico(AbstractWebServerWorker _WebServerWorker)
        {
            m_Options = ObjectsFactory.CreateMessengerServiceOptionsXMLParser();

            m_ChatMessagesWebSender = ObjectsFactory.CreateChatMessagesWebSender(_WebServerWorker);
        }
        //------------------------------------------------------------------------------------------------
        public string GetBalance()
        {
            return "0.00";
        }
        //------------------------------------------------------------------------------------------------
        public MessengerServiceResponse GetCostSendingMessage(string recipient, Notification message)
        {
            MessengerServiceResponse _Response = new MessengerServiceResponse();

            _Response.result = true;

            return _Response;
        }
        //------------------------------------------------------------------------------------------------
        public MessengerServiceResponse GetCostSendingNewsletter(List<string> recipients, Notification message)
        {
            MessengerServiceResponse _Response = new MessengerServiceResponse();

            _Response.result = true;

            return _Response;
        }
        //------------------------------------------------------------------------------------------------
        public MessengerServiceResponse GetCostSendingPersonalNewsletter(Dictionary<string, Notification> recipient_message)
        {
            MessengerServiceResponse _Response = new MessengerServiceResponse();

            _Response.result = true;

            return _Response;
        }
        //------------------------------------------------------------------------------------------------
        public string GetName()
        {
            return "Умнико чат-мессенджер";
        }
        //------------------------------------------------------------------------------------------------
        public BaseOptions GetOptions()
        {
            return m_Options;
        }
        //------------------------------------------------------------------------------------------------
        public string GetOptionsSerializedString()
        {
            return m_Options.Pack();
        }
        //------------------------------------------------------------------------------------------------
        public MessengerServiceResponse SendMessage(string recipient, Notification message)
        {
            MessengerServiceResponse _Result = new MessengerServiceResponse()
            {
                notificationIds = new Dictionary<string, string>()
            };

            if (!OptionsIsInit(ref _Result))
            {
                return _Result;
            }

            List<PersonInfo> _PersonsInfo = new List<PersonInfo>()
            {
                new PersonInfo()
                {
                    PersonId = int.Parse(recipient),
                    Text = message.Message
                }
            };

            PersonsRequestData _RequestData = new PersonsRequestData()
            {
                PersonsInfo = _PersonsInfo,
                Image = message.Image,
                Guid = message.Guid
            };

            PersonsResponseData _Response = m_ChatMessagesWebSender.SendMessages(_RequestData);

            foreach (KeyValuePair<string, string> pair in _Response.MesUmnGuids)
            {
                _Result.notificationIds[pair.Key] = pair.Value;
            }

            _Result.result = true;
            _Result.notificationSendState = SendState.PrepareToSend;

            return _Result;
        }
        //------------------------------------------------------------------------------------------------
        public MessengerServiceResponse SendNewsletter(List<string> recipients, Notification message)
        {
            MessengerServiceResponse _Result = new MessengerServiceResponse()
            {
                notificationIds = new Dictionary<string, string>()
            };

            if (!OptionsIsInit(ref _Result))
            {
                return _Result;
            }

            List<PersonInfo> _PersonsInfo = new List<PersonInfo>();

            foreach (string recipient in recipients)
            {
                var _PerInfo = new PersonInfo()
                {
                    PersonId = int.Parse(recipient),
                    Text = message.Message
                };

                _PersonsInfo.Add(_PerInfo);
            }

            PersonsRequestData _RequestData = new PersonsRequestData()
            {
                PersonsInfo = _PersonsInfo,
                Image = message.Image,
                Guid = message.Guid
            };

            PersonsResponseData _Response = m_ChatMessagesWebSender.SendMessages(_RequestData);

            foreach (KeyValuePair<string, string> pair in _Response.MesUmnGuids)
            {
                _Result.notificationIds[pair.Key] = pair.Value;
            }

            _Result.result = true;
            _Result.notificationSendState = SendState.PrepareToSend;

            return _Result;
        }
        //------------------------------------------------------------------------------------------------
        public MessengerServiceResponse SendPersonalNewsletter(Dictionary<string, Notification> recipient_message)
        {
            string _Image = null;

            MessengerServiceResponse _Result = new MessengerServiceResponse()
            {
                notificationIds = new Dictionary<string, string>()
            };

            if (!OptionsIsInit(ref _Result))
            {
                return _Result;
            }

            List<PersonInfo> _PersonsInfo = new List<PersonInfo>();

            foreach (KeyValuePair<string, Notification> rec_mes_pair in recipient_message)
            {
                if (_Image == null)
                {
                    _Image = rec_mes_pair.Value.Image;
                }

                var _PerInfo = new PersonInfo()
                {
                    PersonId = int.Parse(rec_mes_pair.Key),
                    Text = rec_mes_pair.Value.Message,
                    Guid = rec_mes_pair.Value.Guid
                };

                _PersonsInfo.Add(_PerInfo);
            }

            PersonsRequestData _RequestData = new PersonsRequestData()
            {
                PersonsInfo = _PersonsInfo,
                Image = _Image
            };

            PersonsResponseData _Response = m_ChatMessagesWebSender.SendMessages(_RequestData);

            foreach (KeyValuePair<string, string> pair in _Response.MesUmnGuids)
            {
                _Result.notificationIds[pair.Key] = pair.Value;
            }

            _Result.result = true;
            _Result.notificationSendState = SendState.PrepareToSend;

            return _Result;
        }
        //------------------------------------------------------------------------------------------------
        public bool SetOptions(string options)
        {
            if (string.IsNullOrWhiteSpace(options))
            {
                return false;
            }

            MessengerServiceOptionsDomainModel _OptionsDomainModel = new MessengerServiceOptionsDomainModel();
            MessengerServiceOptionsXMLParser _Options = new MessengerServiceOptionsXMLParser(_OptionsDomainModel);

            try
            {
                m_Options = (MessengerServiceOptionsXMLParser)m_Options.Unpack(options);

                MessagesWebSenderOptions _WebSenderOptions = new MessagesWebSenderOptions()
                {
                    Domain = m_Options.DomainModel.Domain,
                    Port = m_Options.DomainModel.Port,
                    Guid = m_Options.DomainModel.Guid
                };

                m_ChatMessagesWebSender.InitOptions(_WebSenderOptions);
            }
            catch (InvalidOperationException)
            {
                return false;
            }

            return true;
        }
        //------------------------------------------------------------------------------------------------
        public bool ShowOptions(ref string _Options)
        {
            SetOptions(_Options);

            FormOptions form = null;

            try
            {
                MessengerServiceOptionsDomainModel _OptionsDomainModel = m_Options.DomainModel;
                form = new FormOptions(_OptionsDomainModel);
                form.SetChatMessagesWebSender(m_ChatMessagesWebSender);

                DialogResult showResult = form.ShowDialog();

                if (showResult == DialogResult.OK)
                {
                    _OptionsDomainModel.Domain = form.URLDomain;
                    _OptionsDomainModel.Guid = form.ClinicGuid;
                    _OptionsDomainModel.Port = form.URLPort;

                    _Options = m_Options.Pack();
                }
            }
            finally
            {
                form?.Dispose();
            }

            return true;
        }
        //------------------------------------------------------------------------------------------------
        internal bool TestShowOptions(ref string _Options, bool _DialogResultEqualOK, out FormOptions _Form)
        {
            SetOptions(_Options);

            MessengerServiceOptionsDomainModel _OptionsDomainModel = m_Options.DomainModel;
            _Form = new FormOptions(_OptionsDomainModel, _DialogResultEqualOK);
            _Form.SetChatMessagesWebSender(m_ChatMessagesWebSender);

            DialogResult showResult = _Form.ShowDialog();

            if (showResult == DialogResult.OK)
            {
                _OptionsDomainModel.Domain = _Form.URLDomain;
                _OptionsDomainModel.Guid = _Form.ClinicGuid;
                _OptionsDomainModel.Port = _Form.URLPort;

                _Options = m_Options.Pack();
            }

            return true;
        }
        //------------------------------------------------------------------------------------------------
        internal bool OptionsIsInit(ref MessengerServiceResponse _Response)
        {
            if (!m_Options.DomainModel.IsInit)
            {
                _Response.result = false;
                _Response.error = "Необходимо выполнить настройку модуля!";
                return false;
            }

            return true;
        }
        //------------------------------------------------------------------------------------------------
    }
}
