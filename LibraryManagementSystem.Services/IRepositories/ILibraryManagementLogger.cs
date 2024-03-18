using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Repository.IRepositories
{
    public interface ILibraryManagementLogger
    {
        /// <summary>
        /// The LogInformation
        /// </summary>
        /// <param name="message"></param>
        void LogInformation(string message);

        /// <summary>
        /// The LogWarning
        /// </summary>
        /// <param name="message"></param>
        void LogWarning(string message);

        /// <summary>
        /// The LogDebug
        /// </summary>
        /// <param name="message"></param>
        void LogDebug(string message);

        /// <summary>
        /// The LogError
        /// </summary>
        /// <param name="message"></param>
        void LogError(string message);
    }
}
