using MessengerService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using MessengerService.Umnico;
using MessengerService.Umnico.Options;
using Moq;
using MessagesWebSender;

namespace MessengerServiceUmnicoTests.Tests.Unit
{
    public sealed class MessengerServiceUmnicoTests
    {
        [Fact]
        public void Get_Expected_Name_For_Messenger_Service()
        {
            string _ExpectedName = "Умнико чат-мессенджер";
            Mock<AbstractWebServerWorker> mock = new Mock<AbstractWebServerWorker>();
            IMessengerService sut = new MessengerServiceUmnico(mock.Object);

            string result = sut.GetName();

            Assert.Equal(_ExpectedName, result);
        }
        //------------------------------------------------------------------------------------------------
        [Fact]
        public void Current_Balance_When_Create_Messenger_Service_Equal_Zero()
        {
            Mock<AbstractWebServerWorker> mock = new Mock<AbstractWebServerWorker>();
            IMessengerService sut = new MessengerServiceUmnico(mock.Object);

            string result = sut.GetBalance();

            Assert.Equal("0.00", result);
        }
        //------------------------------------------------------------------------------------------------
        [Fact]
        public void Operation_Get_Cost_Sending_Message_Is_Execute_Success()
        {
            Mock<AbstractWebServerWorker> mock = new Mock<AbstractWebServerWorker>();
            IMessengerService sut = new MessengerServiceUmnico(mock.Object);

            MessengerServiceResponse _Response = sut.GetCostSendingMessage(null, null);

            Assert.True(_Response.result);
        }
        //------------------------------------------------------------------------------------------------
        [Fact]
        public void Operation_Get_Cost_Sending_News_Letter_Is_Execute_Success()
        {
            Mock<AbstractWebServerWorker> mock = new Mock<AbstractWebServerWorker>();
            IMessengerService sut = new MessengerServiceUmnico(mock.Object);

            MessengerServiceResponse _Response = sut.GetCostSendingNewsletter(null, null);

            Assert.True(_Response.result);
        }
        //------------------------------------------------------------------------------------------------
        [Fact]
        public void Operation_Get_Cost_Sending_Personal_News_Letter_Is_Execute_Success()
        {
            Mock<AbstractWebServerWorker> mock = new Mock<AbstractWebServerWorker>();
            IMessengerService sut = new MessengerServiceUmnico(mock.Object);

            MessengerServiceResponse _Response = sut.GetCostSendingPersonalNewsletter(null);

            Assert.True(_Response.result);
        }
        //------------------------------------------------------------------------------------------------
        [Fact]
        public void Options_Created_In_Messenger_Service()
        {
            Mock<AbstractWebServerWorker> mock = new Mock<AbstractWebServerWorker>();
            IMessengerService sut = new MessengerServiceUmnico(mock.Object);
            string _Options = MotherObject.CreateUmnicoXMLSerializedString();
            sut.SetOptions(_Options);
            BaseOptions sutOptions = ((MessengerServiceUmnico)sut).Options;

            BaseOptions result = sut.GetOptions();

            Assert.Equal(sutOptions, result);
            Assert.NotNull(result);
        }
        //------------------------------------------------------------------------------------------------
        [Fact]
        public void If_Set_Options_Not_Executed_Then_Options_Not_Created()
        {
            Mock<AbstractWebServerWorker> mock = new Mock<AbstractWebServerWorker>();
            IMessengerService sut = new MessengerServiceUmnico(mock.Object);

            BaseOptions result = sut.GetOptions();

            Assert.NotNull(result);
        }
        //------------------------------------------------------------------------------------------------
        [Fact]
        public void Operation_Get_Serialized_Options_Is_Execute_Success()
        {
            Mock<AbstractWebServerWorker> mock = new Mock<AbstractWebServerWorker>();
            IMessengerService sut = new MessengerServiceUmnico(mock.Object);
            string _Options = MotherObject.CreateUmnicoXMLSerializedString();
            sut.SetOptions(_Options);
            string sutOptionsSerializedString = ((MessengerServiceUmnico)sut).Options.Pack();

            string result = sut.GetOptionsSerializedString();

            Assert.Equal(sutOptionsSerializedString, result);
        }
        //------------------------------------------------------------------------------------------------
        /// <summary>
        /// Данный тест может оказаться хрупким, так как мы проверяем свойство DomainModel 
        /// класса MessengerServiceOptionsXMLParser, а это деталь имплементации
        /// </summary>
        [Fact]
        public void Operation_Set_Options_Is_Execute_Success_And_Then_Options_State_Changed()
        {
            Mock<AbstractWebServerWorker> mock = new Mock<AbstractWebServerWorker>();
            IMessengerService sut = new MessengerServiceUmnico(mock.Object);
            string _Domain = "localhost";
            int _Port = 100;
            string _Guid = "7a93e9a3-ce8b-4b4c-937f-239e33c50b2d";
            MessengerServiceOptionsDomainModel _TempOptionsDomainModel = MotherObject.CreateOptionsDomainModel(_Domain, _Guid, _Port);
            string _Options = MotherObject.CreateXMLSerializedString(_TempOptionsDomainModel);

            bool result = sut.SetOptions(_Options);
            MessengerServiceOptionsDomainModel _OptionsDomainModel = ((MessengerServiceUmnico)sut).Options.DomainModel;

            Assert.True(result);
            Assert.Equal(_Domain, _OptionsDomainModel.Domain);
            Assert.Equal(_Guid, _OptionsDomainModel.Guid);
            Assert.Equal(_Port, _OptionsDomainModel.Port);
            Assert.True(_OptionsDomainModel.IsInit);
        }
        //------------------------------------------------------------------------------------------------
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("amksoifiojsfojis213901823@^#&*^@#&*")]
        public void Operation_Set_Options_Is_Execute_Failure(string _Options)
        {
            Mock<AbstractWebServerWorker> mock = new Mock<AbstractWebServerWorker>();
            IMessengerService sut = new MessengerServiceUmnico(mock.Object);
            MessengerServiceOptionsDomainModel _OptionsDomainModel = ((MessengerServiceUmnico)sut).Options.DomainModel;

            bool result = sut.SetOptions(_Options);

            Assert.False(result);
            Assert.False(_OptionsDomainModel.IsInit);
        }
        //------------------------------------------------------------------------------------------------
        [Fact]
        public void Show_Form_Options_With_Empty_Fields_And_Click_Button_OK()
        {
            FormOptions _Form = null;

            try
            {
                Mock<AbstractWebServerWorker> mock = new Mock<AbstractWebServerWorker>();
                IMessengerService sut = new MessengerServiceUmnico(mock.Object);
                string _Options = "";
                MessengerServiceOptionsDomainModel _ExpectedOptionsDomainModel = MotherObject.CreateOptionsDomainModel("", "", 100);
                string _ExpectedOptions = MotherObject.CreateXMLSerializedString(_ExpectedOptionsDomainModel);

                bool result = ((MessengerServiceUmnico)sut).TestShowOptions(ref _Options, _DialogResultEqualOK: true, out _Form);

                Assert.True(result);
                Assert.Equal(_ExpectedOptions, _Options);
                Assert.Equal("", _Form.URLDomain);
                Assert.Equal("", _Form.ClinicGuid);
                Assert.Equal(100, _Form.URLPort);
            }
            finally
            {
                _Form?.Dispose();
            }
        }
        //------------------------------------------------------------------------------------------------
        [Fact]
        public void Show_Form_Options_With_Fill_Fields_And_Click_Button_OK()
        {
            FormOptions _Form = null;

            try
            {
                Mock<AbstractWebServerWorker> mock = new Mock<AbstractWebServerWorker>();
                IMessengerService sut = new MessengerServiceUmnico(mock.Object);
                MessengerServiceOptionsDomainModel _OptionsDomainModel = MotherObject.CreateUmnicoOptionsDomainModel();
                string _ExpectedOptions = MotherObject.CreateXMLSerializedString(_OptionsDomainModel);
                string _Options = MotherObject.CreateXMLSerializedString(_OptionsDomainModel);

                bool result = ((MessengerServiceUmnico)sut).TestShowOptions(ref _Options, _DialogResultEqualOK: true, out _Form);

                Assert.True(result);
                Assert.Equal(_ExpectedOptions, _Options);
                Assert.Equal(_OptionsDomainModel.Domain, _Form.URLDomain);
                Assert.Equal(_OptionsDomainModel.Guid, _Form.ClinicGuid);
                Assert.Equal(_OptionsDomainModel.Port, _Form.URLPort);
            }
            finally
            {
                _Form?.Dispose();
            }
        }
        //------------------------------------------------------------------------------------------------
        [Fact]
        public void Show_Form_Options_With_Empty_Fields_And_Click_Button_Cancel()
        {
            FormOptions _Form = null;

            try
            {
                Mock<AbstractWebServerWorker> mock = new Mock<AbstractWebServerWorker>();
                IMessengerService sut = new MessengerServiceUmnico(mock.Object);
                string _Options = "";

                bool result = ((MessengerServiceUmnico)sut).TestShowOptions(ref _Options, _DialogResultEqualOK: false, out _Form);

                Assert.True(result);
                Assert.Equal("", _Options);
                Assert.Equal("", _Form.URLDomain);
                Assert.Equal("", _Form.ClinicGuid);
                Assert.Equal(100, _Form.URLPort);
            }
            finally
            {
                _Form?.Dispose();
            }
        }
        //------------------------------------------------------------------------------------------------
        [Fact]
        public void Show_Form_Options_With_Fill_Fields_And_Click_Button_Cancel()
        {
            FormOptions _Form = null;

            try
            {
                Mock<AbstractWebServerWorker> mock = new Mock<AbstractWebServerWorker>();
                IMessengerService sut = new MessengerServiceUmnico(mock.Object);
                MessengerServiceOptionsDomainModel _OptionsDomainModel = MotherObject.CreateUmnicoOptionsDomainModel();
                string _ExpectedOptions = MotherObject.CreateXMLSerializedString(_OptionsDomainModel);
                string _Options = MotherObject.CreateXMLSerializedString(_OptionsDomainModel);

                bool result = ((MessengerServiceUmnico)sut).TestShowOptions(ref _Options, _DialogResultEqualOK: false, out _Form);

                Assert.True(result);
                Assert.Equal(_ExpectedOptions, _Options);
                Assert.Equal(_OptionsDomainModel.Domain, _Form.URLDomain);
                Assert.Equal(_OptionsDomainModel.Guid, _Form.ClinicGuid);
                Assert.Equal(_OptionsDomainModel.Port, _Form.URLPort);
            }
            finally
            {
                _Form?.Dispose();
            }
        }
        //------------------------------------------------------------------------------------------------
        [Fact]
        public void Operation_Options_Init_Is_Execute_Success()
        {
            Mock<AbstractWebServerWorker> mock = new Mock<AbstractWebServerWorker>();
            IMessengerService sut = new MessengerServiceUmnico(mock.Object);
            string _Options = MotherObject.CreateUmnicoXMLSerializedString();
            sut.SetOptions(_Options);
            MessengerServiceResponse _Response = new MessengerServiceResponse();

            bool result = ((MessengerServiceUmnico)sut).OptionsIsInit(ref _Response);

            Assert.True(result);
            Assert.False(_Response.result);
            Assert.Null(_Response.error);
        }
        //------------------------------------------------------------------------------------------------
        [Fact]
        public void Operation_Options_Init_Is_Execute_Failure()
        {
            Mock<AbstractWebServerWorker> mock = new Mock<AbstractWebServerWorker>();
            IMessengerService sut = new MessengerServiceUmnico(mock.Object);
            MessengerServiceResponse _Response = new MessengerServiceResponse();

            bool result = ((MessengerServiceUmnico)sut).OptionsIsInit(ref _Response);

            Assert.False(result);
            Assert.False(_Response.result);
            Assert.Equal("Необходимо выполнить настройку модуля!", _Response.error);
        }
        //------------------------------------------------------------------------------------------------
        [Fact]
        public void Can_Send_One_Message_To_One_Recipient()
        {
            // Подготовка (Arrange)
            string _NotificationGuid = Guid.NewGuid().ToString();
            string _ExternalGuid = Guid.NewGuid().ToString();
            Notification _Notification = new Notification()
            {
                Message = "TestMessage",
                Guid = _NotificationGuid
            };
            string _Recipient = "4995";

            Mock<AbstractWebServerWorker> mock = new Mock<AbstractWebServerWorker>();
            mock.Setup(x => x.SendMessages(It.IsAny<Uri>(), It.IsAny<string>(), It.IsAny<PersonsRequestData>()))
                .Returns(new PersonsResponseData()
                {
                    MesUmnGuids = new Dictionary<string, string>()
                    {
                        {
                            _NotificationGuid,
                            _ExternalGuid
                        }
                    }
                });

            IMessengerService sut = new MessengerServiceUmnico(mock.Object);
            string _Options = MotherObject.CreateUmnicoXMLSerializedString();
            sut.SetOptions(_Options);

            // Действие (Act)
            MessengerServiceResponse result = sut.SendMessage(_Recipient, _Notification);

            // Проверка (Assert)
            Assert.True(result.result);
            Assert.Equal(SendState.PrepareToSend, result.notificationSendState);
            Assert.Null(result.error);
            Assert.Equal(1, result.notificationIds.Count);
            Assert.Equal(_NotificationGuid, result.notificationIds.Keys.First());
            Assert.Equal(_ExternalGuid, result.notificationIds[result.notificationIds.Keys.First()]);
            mock.Verify(x => x.SendMessages(It.IsAny<Uri>(), It.IsAny<string>(), It.IsAny<PersonsRequestData>()), Times.Once);
        }
        //------------------------------------------------------------------------------------------------
        [Fact]
        public void Can_Send_One_Message_To_Many_Recipients()
        {
            // Подготовка (Arrange)
            string _NotificationGuid = Guid.NewGuid().ToString();
            string _ExternalGuid = Guid.NewGuid().ToString();
            Notification _Notification = new Notification()
            {
                Message = "TestMessage",
                Guid = _NotificationGuid
            };
            List<string> _Recipients = new List<string>()
            {
                "4995",
                "3110",
                "2112"
            };

            Mock<AbstractWebServerWorker> mock = new Mock<AbstractWebServerWorker>();
            mock.Setup(x => x.SendMessages(It.IsAny<Uri>(), It.IsAny<string>(), It.IsAny<PersonsRequestData>()))
                .Returns(new PersonsResponseData()
                {
                    MesUmnGuids = new Dictionary<string, string>()
                    {
                        {
                            _NotificationGuid,
                            _ExternalGuid
                        }
                    }
                });

            IMessengerService sut = new MessengerServiceUmnico(mock.Object);
            string _Options = MotherObject.CreateUmnicoXMLSerializedString();
            sut.SetOptions(_Options);


            // Действие (Act)
            MessengerServiceResponse result = sut.SendNewsletter(_Recipients, _Notification);


            // Проверка (Assert)
            Assert.True(result.result);
            Assert.Equal(SendState.PrepareToSend, result.notificationSendState);
            Assert.Null(result.error);
            Assert.Equal(1, result.notificationIds.Count);
            Assert.Equal(_NotificationGuid, result.notificationIds.Keys.First());
            Assert.Equal(_ExternalGuid, result.notificationIds[result.notificationIds.Keys.First()]);
            mock.Verify(x => x.SendMessages(It.IsAny<Uri>(), It.IsAny<string>(), It.IsAny<PersonsRequestData>()), Times.Once);
        }
        //------------------------------------------------------------------------------------------------
        [Fact]
        public void Can_Send_Many_Personal_Messages_To_Many_Recipients()
        {
            // Подготовка (Arrange)
            string _FirstNotificationGuid = Guid.NewGuid().ToString();
            string _SecondNotificationGuid = Guid.NewGuid().ToString();
            string _ThirdNotificationGuid = Guid.NewGuid().ToString();

            string _FirstExternalGuid = Guid.NewGuid().ToString();
            string _SecondExternalGuid = Guid.NewGuid().ToString();
            string _ThirdExternalGuid = Guid.NewGuid().ToString();

            var _RecipientMessages = new Dictionary<string, Notification>()
            {
                { 
                    "4995", 
                    new Notification()
                    {
                        Message = "TestMessage",
                        Guid = _FirstNotificationGuid
                    }
                },

                {
                    "3110",
                    new Notification()
                    {
                        Message = "TestMessage",
                        Guid = _SecondNotificationGuid
                    }
                },

                {
                    "2112",
                    new Notification()
                    {
                        Message = "TestMessage",
                        Guid = _ThirdNotificationGuid
                    }
                }
            };

            Mock<AbstractWebServerWorker> mock = new Mock<AbstractWebServerWorker>();
            mock.Setup(x => x.SendMessages(It.IsAny<Uri>(), It.IsAny<string>(), It.IsAny<PersonsRequestData>()))
                .Returns(new PersonsResponseData()
                {
                    MesUmnGuids = new Dictionary<string, string>()
                    {
                        {
                            _FirstNotificationGuid,
                            _FirstExternalGuid
                        },

                        {
                            _SecondNotificationGuid,
                            _SecondExternalGuid
                        },

                        {
                            _ThirdNotificationGuid,
                            _ThirdExternalGuid
                        }
                    }
                });

            IMessengerService sut = new MessengerServiceUmnico(mock.Object);
            string _Options = MotherObject.CreateUmnicoXMLSerializedString();
            sut.SetOptions(_Options);


            // Действие (Act)
            MessengerServiceResponse result = sut.SendPersonalNewsletter(_RecipientMessages);


            // Проверка (Assert)
            Assert.True(result.result);
            Assert.Equal(SendState.PrepareToSend, result.notificationSendState);
            Assert.Null(result.error);
            Assert.Equal(3, result.notificationIds.Count);
            Assert.Contains(_FirstNotificationGuid, result.notificationIds);
            Assert.Contains(_SecondNotificationGuid, result.notificationIds);
            Assert.Contains(_ThirdNotificationGuid, result.notificationIds);
            Assert.Equal(_FirstExternalGuid, result.notificationIds[_FirstNotificationGuid]);
            Assert.Equal(_SecondExternalGuid, result.notificationIds[_SecondNotificationGuid]);
            Assert.Equal(_ThirdExternalGuid, result.notificationIds[_ThirdNotificationGuid]);
            mock.Verify(x => x.SendMessages(It.IsAny<Uri>(), It.IsAny<string>(), It.IsAny<PersonsRequestData>()), Times.Once);
        }
        //------------------------------------------------------------------------------------------------
    }
}