using System;
using UnityEngine;

namespace MissingChecker
{
    public enum LogLevel
    {
        All = 0,
        Log,
        Warn,
        Error,
        Exception,
        None
    }

    public static class LogUtility 
    {
        private const string OUTPUT_LOG_DEFINE = "MISSING_CHECKER_OUTPUT_LOG";

        public static LogLevel Level;


        [System.Diagnostics.Conditional(OUTPUT_LOG_DEFINE)]
        internal static void Log(string message)
        {
            if (Level > LogLevel.Log)
            {
                return;
            }
            Debug.Log(message);
        }

        internal static void LogWarn(string message)
        {
            if (Level > LogLevel.Warn)
            {
                return;
            }
            Debug.LogWarning(message);
        }

        internal static void LogError(string message)
        {
            if (Level > LogLevel.Error)
            {
                return;
            }
            Debug.LogError(message);
        }

        internal static void LogException(Exception ex)
        {
            if (Level > LogLevel.Exception)
            {
                return;
            }
            Debug.LogException(ex);
        }
    }
}
