using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using WS.Finances.Core.Lib.Data;

namespace WS.Finances.Core.Web.Services
{
    public class BaseDirectoryProvider : IBaseDirectoryProvider
    {
        private readonly AppSettings _appSettings;
        private readonly IHostingEnvironment _hostingEnvironment;

        public BaseDirectoryProvider(IOptions<AppSettings> appSettings, IHostingEnvironment hostingEnvironment)
        {
            _appSettings = appSettings.Value;
            _hostingEnvironment = hostingEnvironment;
        }

        public string BaseDirectory
        {
            get
            {
                var webRoot = _hostingEnvironment.ContentRootPath;
                return _appSettings.BaseDirectory.Replace("~", webRoot);
            }
        }
    }
}
