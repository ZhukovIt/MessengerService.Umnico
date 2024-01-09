using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using MessagesWebSender;
using Moq;

namespace MessagesWebSender.Tests.Integration
{
    /// <summary>
    /// Перед выполнением данных тестов необходимо убедиться, что
    /// сервер для приёма сообщений запущен!
    /// Иначе большая часть тестов просто обвалится!
    /// </summary>
    public sealed class ChatMessagesWebSenderTests
    {
        [Fact]
        public void Verify_Guid_Is_Success_If_Guid_Is_Right()
        {
            // Подготовка (Arrange)
            MessagesWebSenderOptions _Options = CreateOptions();

            Mock<AbstractWebServerWorker> mock = new Mock<AbstractWebServerWorker>();
            mock.Setup(x => x.VerifyGuid(It.IsAny<Uri>(), _Options.Guid))
                .Returns(new PersonsResponseData()
                {
                    Success = true
                });

            ChatMessagesWebSender sut = new ChatMessagesWebSender(mock.Object);
            sut.InitOptions(_Options);

            // Действие (Act)
            PersonsResponseData result = sut.VerifyGuid();

            // Проверка (Assert)
            Assert.True(result.Success);
            Assert.Null(result.InfoMessage);
            mock.Verify(x => x.VerifyGuid(It.IsAny<Uri>(), _Options.Guid), Times.Once);
        }
        //-------------------------------------------------------------------------------
        [Fact]
        public void Verify_Guid_Is_Failure_If_Guid_Is_Not_Right()
        {
            // Подготовка (Arrange)
            Mock<AbstractWebServerWorker> mock = new Mock<AbstractWebServerWorker>();
            mock.Setup(x => x.VerifyGuid(It.IsAny<Uri>(), ""))
                .Returns(new PersonsResponseData()
                {
                    Success = false,
                    InfoMessage = "GUID для авторизации отсутствует!"
                });

            ChatMessagesWebSender sut = new ChatMessagesWebSender(mock.Object);
            MessagesWebSenderOptions _Options = CreateOptions();
            _Options.Guid = "";
            sut.InitOptions(_Options);

            // Действие (Act)
            PersonsResponseData result = sut.VerifyGuid();

            // Проверка (Assert)
            Assert.False(result.Success);
            Assert.NotNull(result.InfoMessage);
            mock.Verify(x => x.VerifyGuid(It.IsAny<Uri>(), ""));
        }
        //-------------------------------------------------------------------------------
        [Fact]
        public void Check_Connection_Is_Failure_If_Options_Is_Not_Init()
        {
            Mock<AbstractWebServerWorker> mock = new Mock<AbstractWebServerWorker>();
            ChatMessagesWebSender sut = new ChatMessagesWebSender(mock.Object);

            PersonsResponseData result = sut.CheckConnection();

            Assert.False(result.Success);
            Assert.NotNull(result.InfoMessage);
            mock.Verify(x => x.DefaultRoute(It.IsAny<Uri>()), Times.Never);
        }
        //-------------------------------------------------------------------------------
        [Fact]
        public void Verify_Guid_Is_Failure_If_Options_Is_Not_Init()
        {
            Mock<AbstractWebServerWorker> mock = new Mock<AbstractWebServerWorker>();
            ChatMessagesWebSender sut = new ChatMessagesWebSender(mock.Object);

            PersonsResponseData result = sut.VerifyGuid();

            Assert.False(result.Success);
            Assert.NotNull(result.InfoMessage);
            mock.Verify(x => x.VerifyGuid(It.IsAny<Uri>(), It.IsAny<string>()), Times.Never);
        }
        //-------------------------------------------------------------------------------
        [Fact]
        public void Check_Connection_Is_Success()
        {
            // Подготовка (Arrange)
            Mock<AbstractWebServerWorker> mock = new Mock<AbstractWebServerWorker>();
            mock.Setup(x => x.DefaultRoute(It.IsAny<Uri>()))
                .Returns(new PersonsResponseData()
                {
                    Success = true,
                    InfoMessage = "СиМед - Сервер получения сообщений массовой рассылки для службы чат-мессенджеров. На связи!"
                });

            ChatMessagesWebSender sut = new ChatMessagesWebSender(mock.Object);
            sut.InitOptions(CreateOptions());

            // Действие (Act)
            PersonsResponseData result = sut.CheckConnection();

            // Проверка (Assert)
            Assert.True(result.Success);
            Assert.NotNull(result.InfoMessage);
            mock.Verify(x => x.DefaultRoute(It.IsAny<Uri>()), Times.Once);
        }
        //-------------------------------------------------------------------------------
        private MessagesWebSenderOptions CreateOptions()
        {
            return new MessagesWebSenderOptions()
            {
                Domain = "localhost",
                Port = 100,
                Guid = "7a93e9a3-ce8b-4b4c-937f-239e33c50b2d"
            };
        }
        //-------------------------------------------------------------------------------
    }
}
