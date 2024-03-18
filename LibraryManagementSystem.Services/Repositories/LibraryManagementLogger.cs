using LibraryManagementSystem.Repository.IRepositories;
using Microsoft.Extensions.Logging;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
