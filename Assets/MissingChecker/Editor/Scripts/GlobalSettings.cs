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

    public static class GlobalSettings 
    {
        /// <summary>
        /// Global setter to change log level inside MissingChecker
        /// </summary>
        public static LogLevel LogLevel
        {
            set
            {
                LogUtility.Level = value;
            }
        }
    }
}
