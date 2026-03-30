using Movies.Application.Interfaces;

namespace Movies.API.Providers
{
    public class WebRootPathProvider : IWebRootPathProvider
    {
        public WebRootPathProvider(IWebHostEnvironment env)
        {
            WebRootPath = env.WebRootPath;
        }

        public string WebRootPath { get; }

    }
}
