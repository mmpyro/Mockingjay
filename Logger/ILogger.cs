using System;

namespace CustomLogger
{
    public interface ILogger
    {
        void Info(string message);
        void Warn(string message);
        void Error(string message);
        void Debug(string message);
        void Info(Exception ex);
        void Warn(Exception ex);
        void Error(Exception ex);
        void Fatal(Exception ex);
        void Info(string message, Exception ex);
        void Warn(string message, Exception ex);
        void Error(string message, Exception ex);
    }
}
