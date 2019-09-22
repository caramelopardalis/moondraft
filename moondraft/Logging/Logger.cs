using System;
using System.Runtime.CompilerServices;
using Utf8Json;

namespace moondraft.Logging
{
    public class Logger
    {
        public static void DebugWithoutCallerInfo(string message)
        {
#if DEBUG
            System.Diagnostics.Debug.WriteLine(message);
#endif
        }

        public static void Debug(string message, [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0, [CallerMemberName] string methodName = "")
        {
#if DEBUG
            System.Diagnostics.Debug.WriteLine(string.Format($"Logger: {filePath}:{lineNumber} - {methodName}: {message}"));
#endif
        }

        public static void Debug(string message, object value, [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0, [CallerMemberName] string methodName = "")
        {
#if DEBUG
            string serialized;
            try
            {
                serialized = JsonSerializer.ToJsonString(value);
            }
            catch (Exception)
            {
                serialized = "Occured serializaion exception.";
            }
            var type = value?.GetType();
            System.Diagnostics.Debug.WriteLine(string.Format($"Logger: {filePath}:{lineNumber} - {methodName}: {type}: {message}", serialized));
#endif
        }
    }
}
