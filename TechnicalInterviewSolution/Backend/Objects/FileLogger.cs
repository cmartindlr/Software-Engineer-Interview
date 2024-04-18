using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Objects
{
    /// <summary>
    /// Contains methods for logging to files.
    /// </summary>
    public static class FileLogger
    {
        /// <summary>
        /// Logs excceptions to a file.
        /// </summary>
        /// <param name="message">
        /// The message to log.
        /// </param>
        public static void Log(string message) 
        {
            Task.Run(() =>
            {
                using(StreamWriter file = File.AppendText("Log-" + DateTime.Today.ToShortDateString().Replace("/", "-") + ".txt"))
                {
                    file.WriteLine(DateTime.Now + ": " + message);
                }
            });
        }
    }
}
