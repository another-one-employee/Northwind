using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace Northwind.Middleware
{
    public class ImageCachingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string[] _expectedExtensions;
        private readonly DirectoryInfo _directoryInfo;
        private readonly Timer _timer;
        private readonly int _maxImagesCount;

        public ImageCachingMiddleware(RequestDelegate next, IConfiguration _configuration)
        {
            _next = next;

            var cacheSettings = _configuration.GetSection("CacheSettings");
            _directoryInfo =
                new DirectoryInfo(string.Concat(Directory.GetCurrentDirectory(),
                cacheSettings.GetValue<string>("FolderPathFromCurrentDirectory")));

            _expectedExtensions = cacheSettings.GetValue<string>("CachedExtensions").Split("|");

            _maxImagesCount = cacheSettings.GetValue<int>("MaxImagesCount");

            _timer = new Timer(cacheSettings.GetValue<double>("CacheTimerCleanValue"))
            {
                AutoReset = true
            };
            _timer.Elapsed += (sender, e) => CleanCache();
            _timer.Start();
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.Value.ToLower().Contains("image"))
            {
                var id = context.Request.RouteValues["id"].ToString();

                Stream originalBody = context.Response.Body;

                try
                {
                    using MemoryStream memoryStream = new MemoryStream();
                    context.Response.Body = memoryStream;

                    await _next(context);

                    memoryStream.Position = 0;

                    string extension = GetExtensionFromContentType(context.Response.ContentType);
                    int currentImagesCount = GetCurrentImagesCount();

                    if (_expectedExtensions.Contains(extension) &&
                        currentImagesCount < _maxImagesCount &&
                        !IsThisImageExist(id))
                    {
                        await ImageCachingAsync(id, memoryStream);
                        _timer.Start();
                    }
                    else if (currentImagesCount >= _maxImagesCount)
                    {
                        CleanCache();
                    }
                    else
                    {
                        context.Response.Body = File.OpenRead(string.Concat(_directoryInfo.FullName, id));
                    }

                    memoryStream.Position = 0;
                    await memoryStream.CopyToAsync(originalBody);

                }
                finally
                {
                    context.Response.Body = originalBody;
                }
            }

            await _next(context);
        }

        private int GetCurrentImagesCount()
            => _directoryInfo.Exists ? _directoryInfo.GetFiles().Length : 0;

        private bool IsThisImageExist(string imageId)
            => File.Exists(string.Concat(_directoryInfo.FullName, imageId));

        private string GetExtensionFromContentType(string contentType)
            => contentType.Replace("image/", ".");

        private async Task ImageCachingAsync(string imageId, Stream stream)
        {
            if (!_directoryInfo.Exists)
            {
                _directoryInfo.Create();
            }

            string fileName = string.Concat(_directoryInfo.FullName, imageId);

            using FileStream fileStream = new FileStream(fileName, FileMode.Create);
            await stream.CopyToAsync(fileStream);
        }

        private void CleanCache()
        {
            var files = _directoryInfo.GetFiles();

            foreach (var file in files)
            {
                file.Delete();
            }
        }
    }
}
