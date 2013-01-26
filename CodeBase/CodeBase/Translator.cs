namespace CodeBase
{
    using System;

    public class Translator
    {
        private readonly ILogger logger;
        private readonly IService service;

        public Translator(ILogger logger, IService service)
        {
            this.logger = logger;
            this.service = service;
        }

        public string Translate(string input)
        {
            try
            {
                return this.service.Translate(input);
            }
            catch (Exception exception)
            {
                this.logger.Log(exception);
                throw;
            }
        }
    }

    public interface ILogger
    {
        void Log(Exception exception);
    }

    public interface IService
    {
        string Translate(string input);
    }
}
