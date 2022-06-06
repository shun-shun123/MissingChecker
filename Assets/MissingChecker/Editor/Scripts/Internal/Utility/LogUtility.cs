using System;
using UnityEngine;

namespace MissingChecker
{
    internal static class LogUtility 
    {
        internal static LogLevel Level;

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
