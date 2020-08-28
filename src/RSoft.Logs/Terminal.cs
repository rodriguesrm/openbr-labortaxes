using Microsoft.Extensions.Logging;
using RSoft.Logs.Model;
using System;

namespace RSoft.Logs
{

    /// <summary>
    /// Provides methods for displaying formatted messages on the console
    /// </summary>
    public static class Terminal
    {

        #region Local objects/variables

        private readonly static string[] levelNames = new string[]
        {
            "TRC",
            "DBG",
            "INF",
            "WRN",
            "ERR",
            "CRITICAL"
        };


        #endregion

        #region Public methods

        /// <summary>
        /// Print message in console terminal
        /// </summary>
        /// <param name="category">Category logger</param>
        /// <param name="level">Entry will be written on this level.</param>
        /// <param name="message">Entry message</param>
        /// <param name="ex">The exception related to this entry.</param>
        public static void Print(string category, LogLevel level, string message, Exception ex = null)
            => Print(category, level, message, new LogExceptionInfo(ex));

        /// <summary>
        /// Print message in console terminal
        /// </summary>
        /// <param name="category">Category logger</param>
        /// <param name="level">Entry will be written on this level.</param>
        /// <param name="message">Entry message</param>
        /// <param name="ex">The exception related to this entry.</param>
        public static void Print(string category, LogLevel level, string message, LogExceptionInfo ex = null)
        {

            if (level == LogLevel.None)
                return;

            // Set defaults
            Console.ResetColor();
            ConsoleColor levelForegroundColor = Console.ForegroundColor;
            ConsoleColor levelBackgroundColor = Console.BackgroundColor;
            ConsoleColor messageForegroundColor = Console.ForegroundColor;
            ConsoleColor messageBackgroundColor = Console.BackgroundColor;
            ConsoleColor errorForegroundColor = Console.ForegroundColor;
            ConsoleColor errorBackgroundColor = Console.BackgroundColor;

            // Date
            string date = $"{DateTime.Now:yyyy-MM-dd hh:mm:ss.fff} [";
            Console.Write(date);

            // Level
            switch (level)
            {

                case LogLevel.Trace:
                    levelForegroundColor = ConsoleColor.DarkCyan;
                    break;

                case LogLevel.Debug:
                    levelForegroundColor = ConsoleColor.Gray;
                    messageForegroundColor = ConsoleColor.DarkGray;
                    break;

                case LogLevel.Information:
                    levelForegroundColor = ConsoleColor.Green;
                    break;

                case LogLevel.Warning:
                    levelForegroundColor = ConsoleColor.DarkYellow;
                    messageForegroundColor = ConsoleColor.White;
                    break;

                case LogLevel.Error:
                    levelForegroundColor = ConsoleColor.DarkRed;
                    messageForegroundColor = ConsoleColor.Yellow;
                    errorForegroundColor = ConsoleColor.Yellow;
                    break;

                case LogLevel.Critical:
                    levelForegroundColor = ConsoleColor.Yellow;
                    levelBackgroundColor = ConsoleColor.DarkRed;
                    messageForegroundColor = ConsoleColor.DarkRed;
                    messageBackgroundColor = ConsoleColor.Yellow;
                    errorForegroundColor = ConsoleColor.DarkRed;
                    break;

            }

            Console.ForegroundColor = levelForegroundColor;
            Console.BackgroundColor = levelBackgroundColor;
            Console.Write(levelNames[(int)level]);

            // Source
            Console.ResetColor();
            Console.Write($"]: [{category}] | ");

            // Message

            Console.ForegroundColor = messageForegroundColor;
            Console.BackgroundColor = messageBackgroundColor;
            Console.Write(message);
            Console.ResetColor();
            Console.WriteLine(" |");

            // Exception
            if (ex != null)
            {
                Console.ForegroundColor = errorForegroundColor;
                Console.BackgroundColor = errorBackgroundColor;
                Console.WriteLine($"  ErrorMessage: {ex.Message}");
                Console.WriteLine($"  Type: {ex.GetType()}");
                Console.WriteLine($"  Source: {ex.Source}");
                Console.WriteLine($"  StackTrace:");
                Console.WriteLine($"   {ex.StackTrace.Trim()}");
            }

            Console.ResetColor();

        }

        #endregion

    }
}
