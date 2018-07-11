using WS.Finances.Core.Lib.Data;

namespace WS.Finances.Core.Console
{
    public class BaseDirectoryProvider : IBaseDirectoryProvider
    {
        public string BaseDirectory { get; }

        public BaseDirectoryProvider(AppSettings appSettings)
        {
            BaseDirectory = appSettings.BaseDirectory;
        }
    }
}
