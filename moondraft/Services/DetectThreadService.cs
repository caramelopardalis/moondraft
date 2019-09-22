using moondraft.Logging;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace moondraft.Services
{
    public class DetectThreadService
    {
        static int MainThreadId = -1;

        public async static Task<bool> IsInvokedInMainThreadAsync()
        {
            if (MainThreadId == -1)
            {
                await Device.InvokeOnMainThreadAsync(() =>
                {
                    MainThreadId = Environment.CurrentManagedThreadId;
                });
            }
            return Environment.CurrentManagedThreadId == MainThreadId;
        }

        public async static Task LogAsync([CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0, [CallerMemberName] string methodName = "")
        {
#if DEBUG
            Logger.DebugWithoutCallerInfo(string.Format($"{filePath}:{lineNumber} - {methodName}. Invoked in the main thread: ") + await IsInvokedInMainThreadAsync());
#endif
        }
    }
}
