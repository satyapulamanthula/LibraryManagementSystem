using LibraryManagementSystem.Repository.IRepositories;
using NLog;
using ILogger = NLog.ILogger;

namespace LibraryManagementSystem.Repository.Repositories
{
    public class LibraryManagementLogger:ILibraryManagementLogger
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();
        /// <summary>
        /// The LogDebug
        /// </summary>
        /// <param name="message"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void LogDebug(string message)
        {
            logger.Debug(message);
        }

        /// <summary>
        /// The LogError
        /// </summary>
        /// <param name="message"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void LogError(string message)
        {
            logger.Error(message);
        }

        /// <summary>
        /// The LogInformation
        /// </summary>
        /// <param name="message"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void LogInformation(string message)
        {
            logger.Info(message);
        }

        /// <summary>
        /// The LogWarning
        /// </summary>
        /// <param name="message"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void LogWarning(string message)
        {
            logger.Warn(message);
        }
    }
}
