using System.IO;
using Nancy;
using Nancy.Responses;
using Solvberget.Domain.Utils;

namespace Solvberget.Nancy.Modules
{
    public class InfoscreenImageModule : NancyModule
    {
        public InfoscreenImageModule(IEnvironmentPathProvider pathProvider) : base("/infoscreen")
        {
            Get["/image/{name}"] = args =>
            {
                var imageFile = Path.Combine(pathProvider.GetInfoscreenImagesPath(), args.name);
                string mimeType = MimeTypes.GetMimeType(imageFile);

                return new StreamResponse(() => File.OpenRead(imageFile), mimeType);
            };
        }
    }
}