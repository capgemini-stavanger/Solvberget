using System;
using System.Collections.Generic;
using System.IO;
using Nancy;
using Nancy.LightningCache.Extensions;
using Nancy.Responses;
using Newtonsoft.Json;
using Solvberget.Core.DTOs;
using Solvberget.Domain.Utils;

namespace Solvberget.Nancy.Modules
{
    public class SlideConfigModule : NancyModule
    {
        private readonly IEnvironmentPathProvider _pathProvider;

        public SlideConfigModule(IEnvironmentPathProvider pathProvider) : base("/slides")
        {
            _pathProvider = pathProvider;

            Get["/{id}"] = args =>
                {
                    var slideConfigs = GetSlideConfigurationsFromFile();
                    var configName = "default";
                    if (slideConfigs.ContainsKey(args.id))
                    {
                        configName = args.id;
                    } else if (!slideConfigs.ContainsKey(configName)) // 404 if neither args.id nor "default" exists in config
                    {
                        return 404;
                    }

                    var slideConfig = slideConfigs[configName];

                    var blackList = GetInstagramBlacklistFromFile();
                    return
                        Response.AsJson(new {slides = slideConfig, instagramBlacklist = blackList})
                            .AsCacheable(DateTime.Now.AddMinutes(20));
                };
        }

        private string[] GetInstagramBlacklistFromFile()
        {
            var file = _pathProvider.GetInstagramBlacklistPath();

            return !File.Exists(file) ? new string[0] : File.ReadAllLines(file);
        }

        private Dictionary<string, SlideConfigDto[]> GetSlideConfigurationsFromFile()
        {
            var file = _pathProvider.GetSlideConfigurationPath();

            if (!File.Exists(file)) return new Dictionary<string, SlideConfigDto[]>();
            var rawConfigJson = File.ReadAllText(file);

            return JsonConvert.DeserializeObject<Dictionary<string, SlideConfigDto[]>>(rawConfigJson);
        }
    }

    public class InfoscreenImageModule : NancyModule
    {
        public InfoscreenImageModule(IEnvironmentPathProvider pathProvider) : base("/infoscreen")
        {
            Get["/image/{name}"] = args =>
            {
                var imageFile = Path.Combine(pathProvider.GetInfoscreenImagesPath(), args.name);
                string mimeType = MimeTypes.GetMimeType(imageFile);

                return new GenericFileResponse(imageFile, mimeType);
            };
        }
    }
}