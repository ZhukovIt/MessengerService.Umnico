using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using MessagesWebSender;
using MessengerService.Umnico.Options;

namespace MessengerServiceUmnicoTests.Tests.EndToEnd
{
    // Перед запуском тестов необходимо запустить сервер!
    public sealed class UmnicoWebServerWorkerTests
    {
        [Fact]
        public void Can_Redirect_To_Default_Route()
        {
            MessengerServiceOptionsDomainModel _Options = MotherObject.CreateUmnicoOptionsDomainModel();
            Uri _UrlAddress = new Uri($"http://{_Options.Domain}:{_Options.Port}/");
            AbstractWebServerWorker sut = new UmnicoWebServerWorker();

            PersonsResponseData _Response = sut.DefaultRoute(_UrlAddress);

            Assert.True(_Response.Success);
            Assert.NotNull(_Response.InfoMessage);
        }
        //----------------------------------------------------------------------------------------------------
        [Fact]
        public void Can_Verify_Guid_Value_Is_Correct()
        {
            MessengerServiceOptionsDomainModel _Options = MotherObject.CreateUmnicoOptionsDomainModel();
            Uri _UrlAddress = new Uri($"http://{_Options.Domain}:{_Options.Port}/api/UmnicoBulkMessagesWebServer/VerifyGuid");
            AbstractWebServerWorker sut = new UmnicoWebServerWorker();

            PersonsResponseData _Response = sut.VerifyGuid(_UrlAddress, _Options.Guid);

            Assert.True(_Response.Success);
            Assert.Null(_Response.InfoMessage);
        }
        //----------------------------------------------------------------------------------------------------
        [Fact]
        public void Can_Send_One_Message_To_One_Recepient()
        {
            MessengerServiceOptionsDomainModel _Options = MotherObject.CreateUmnicoOptionsDomainModel();
            Uri _UrlAddress = new Uri($"http://{_Options.Domain}:{_Options.Port}/api/UmnicoBulkMessagesWebServer/SendMessages");
            AbstractWebServerWorker sut = new UmnicoWebServerWorker();
            string _RequestGuid = Guid.NewGuid().ToString();
            PersonsRequestData _RequestData = new PersonsRequestData()
            {
                PersonsInfo = new List<PersonInfo>()
                {
                    new PersonInfo()
                    {
                        PersonId = 1,
                        Text = "Одно сообщение для одного получателя"
                    }
                },

                Guid = _RequestGuid
            };

            PersonsResponseData _Response = sut.SendMessages(_UrlAddress, _Options.Guid, _RequestData);

            Assert.True(_Response.Success);
            Assert.Null(_Response.InfoMessage);
            Assert.Equal(1, _Response.MesUmnGuids.Count);
            Assert.Contains(_RequestGuid, _Response.MesUmnGuids.Keys);
        }
        //----------------------------------------------------------------------------------------------------
        [Fact]
        public void Can_Send_One_Message_To_Many_Recepients()
        {
            MessengerServiceOptionsDomainModel _Options = MotherObject.CreateUmnicoOptionsDomainModel();
            Uri _UrlAddress = new Uri($"http://{_Options.Domain}:{_Options.Port}/api/UmnicoBulkMessagesWebServer/SendMessages");
            AbstractWebServerWorker sut = new UmnicoWebServerWorker();
            string _RequestGuid = Guid.NewGuid().ToString();
            PersonsRequestData _RequestData = new PersonsRequestData()
            {
                PersonsInfo = new List<PersonInfo>()
                {
                    new PersonInfo()
                    {
                        PersonId = 2,
                        Text = "Одно сообщение для многих получателей"
                    },

                    new PersonInfo()
                    {
                        PersonId = 3,
                        Text = "Одно сообщение для многих получателей"
                    },

                    new PersonInfo()
                    {
                        PersonId = 4,
                        Text = "Одно сообщение для многих получателей"
                    }
                },

                Guid = _RequestGuid
            };

            PersonsResponseData _Response = sut.SendMessages(_UrlAddress, _Options.Guid, _RequestData);

            Assert.True(_Response.Success);
            Assert.Null(_Response.InfoMessage);
            Assert.Equal(1, _Response.MesUmnGuids.Count);
            Assert.Contains(_RequestGuid, _Response.MesUmnGuids.Keys);
        }
        //----------------------------------------------------------------------------------------------------
        [Fact]
        public void Can_Send_Many_Personal_Messages_To_Many_Recepients()
        {
            MessengerServiceOptionsDomainModel _Options = MotherObject.CreateUmnicoOptionsDomainModel();
            Uri _UrlAddress = new Uri($"http://{_Options.Domain}:{_Options.Port}/api/UmnicoBulkMessagesWebServer/SendMessages");
            AbstractWebServerWorker sut = new UmnicoWebServerWorker();
            string _FirstRequestGuid = Guid.NewGuid().ToString();
            string _SecondRequestGuid = Guid.NewGuid().ToString();
            string _ThirdRequestGuid = Guid.NewGuid().ToString();
            PersonsRequestData _RequestData = new PersonsRequestData()
            {
                PersonsInfo = new List<PersonInfo>()
                {
                    new PersonInfo()
                    {
                        PersonId = 2,
                        Text = "Много персональных сообщений для многих получателей",
                        Guid = _FirstRequestGuid
                    },

                    new PersonInfo()
                    {
                        PersonId = 3,
                        Text = "Много персональных сообщений для многих получателей",
                        Guid = _SecondRequestGuid
                    },

                    new PersonInfo()
                    {
                        PersonId = 4,
                        Text = "Много персональных сообщений для многих получателей",
                        Guid = _ThirdRequestGuid
                    }
                }
            };

            PersonsResponseData _Response = sut.SendMessages(_UrlAddress, _Options.Guid, _RequestData);

            Assert.True(_Response.Success);
            Assert.Null(_Response.InfoMessage);
            Assert.Equal(3, _Response.MesUmnGuids.Count);
            Assert.Contains(_FirstRequestGuid, _Response.MesUmnGuids.Keys);
            Assert.Contains(_SecondRequestGuid, _Response.MesUmnGuids.Keys);
            Assert.Contains(_ThirdRequestGuid, _Response.MesUmnGuids.Keys);
        }
        //----------------------------------------------------------------------------------------------------
    }
}
