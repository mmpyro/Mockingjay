using NLog;
using System;

namespace CustomLogger
{
    public class GenericLogger : ILogger
    {
        private readonly Logger _logger;

        public GenericLogger(string loggerName)
        {
            _logger = LogManager.GetLogger(loggerName);
        }

        public GenericLogger()
        {
            string uniqueName = (new Guid()).ToString();
            _logger = LogManager.GetLogger(uniqueName);
        }

        public void Debug(string message)
        {
            _logger.Debug(message);
        }

        public void Error(Exception ex)
        {
            _logger.Error(ex);
        }

        public void Error(string message)
        {
            _logger.Error(message);
        }

        public void Error(string message, Exception ex)
        {
            _logger.Error(message, ex);
        }

        public void Fatal(Exception ex)
        {
            _logger.Fatal(ex);
        }

        public void Info(Exception ex)
        {
            _logger.Info(ex);
        }

        public void Info(string message)
        {
            _logger.Info(message);
        }

        public void Info(string message, Exception ex)
        {
            _logger.Info(message, ex);
        }

        public void Warn(Exception ex)
        {
            _logger.Warn(ex);
        }

        public void Warn(string message)
        {
            _logger.Warn(message);
        }

        public void Warn(string message, Exception ex)
        {
            _logger.Warn(message, ex);
        }
    }
}
