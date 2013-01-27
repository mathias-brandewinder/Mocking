namespace MoqTests
{
    using System;
    using CodeBase;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class TestsTranslator
    {
        [Test]
        public void Translate_Should_Return_Successful_Service_Response()
        {
            var input = "Hello";
            var output = "Kitty";

            var service = new Mock<IService>();
            service.Setup(s => s.Translate(input)).Returns(output);

            var logger = new Mock<ILogger>();

            var translator = new Translator(logger.Object, service.Object);

            var result = translator.Translate(input);

            Assert.That(result, Is.EqualTo(output));
        }

        [Test]
        public void When_Service_Fails_Translate_Should_Return_ErrorMessage()
        {
            var service = new Mock<IService>();
            service.Setup(s => s.Translate(It.IsAny<string>())).Throws<Exception>();

            var logger = new Mock<ILogger>();

            var translator = new Translator(logger.Object, service.Object);

            var result = translator.Translate("Hello");

            Assert.That(result, Is.EqualTo(Translator.ErrorMessage));
        }

        [Test]
        public void When_Service_Fails_Exception_Should_Be_Logged()
        {
            var error = new Exception();
            var service = new Mock<IService>();
            service.Setup(s => s.Translate(It.IsAny<string>())).Throws(error);

            var logger = new Mock<ILogger>();

            var translator = new Translator(logger.Object, service.Object);

            translator.Translate("Hello");

            logger.Verify(l => l.Log(error));
        }
    }
}
