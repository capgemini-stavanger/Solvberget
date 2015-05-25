using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Web;
using Nancy;
using Nancy.LightningCache.Extensions;
using Nancy.ViewEngines;
using Newtonsoft.Json;
using Solvberget.Core.DTOs;
using Solvberget.Domain.Utils;
using HttpStatusCode = Nancy.HttpStatusCode;

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
                    var slideConfigs = GetSlideConfigurationsFromFile(pathProvider);
                    
                    var cfg = slideConfigs.FirstOrDefault(s => s.Id == args.id) ??
                              slideConfigs.FirstOrDefault(s => s.Id == "default");

                    if (null == cfg) // 404 if neither args.id nor "default" exists in config
                    {
                        return Response.AsJson(new{message = "No screen config matching ID and no default screen config found."}, HttpStatusCode.NotFound);
                    }
                    
                    AppendInstagramBlacklistToSlideOptions(cfg.Config);
                    return Response.AsJson(cfg.Config);
                };
        }

        private void AppendInstagramBlacklistToSlideOptions(SlideConfigDto[] slideConfig)
        {
            // instagram blacklist
            foreach (var dto in slideConfig.Where(dto => dto.Template == "instagram"))
            {
                if (null == dto.SlideOptions) dto.SlideOptions = new Dictionary<string, string>();

                dto.SlideOptions["blacklist"] = String.Join(",",GetInstagramBlacklistFromFile());
            }
        }

        private string[] GetInstagramBlacklistFromFile()
        {
            var file = _pathProvider.GetInstagramBlacklistPath();
            return !File.Exists(file) ? new string[0] : File.ReadAllLines(file);
        }

        public static ScreenConfigDto[] GetSlideConfigurationsFromFile(IEnvironmentPathProvider path)
        {
            var file = path.GetSlideConfigurationPath();

            if (!File.Exists(file)) return new ScreenConfigDto[0];
            var rawConfigJson = File.ReadAllText(file);

            return JsonConvert.DeserializeObject<ScreenConfigDto[]>(rawConfigJson);
        }
    }
}