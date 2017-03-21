using Nancy;
using Nancy.Helpers;
using Solvberget.Domain.Documents;
using Solvberget.Domain.Utils;
using System.IO;
using System.Web.Configuration;

namespace Solvberget.Nancy
{
    public class EnvironmentPathProvider : IEnvironmentPathProvider
    {
        private readonly string _applicationAppDataPath;
        private readonly string _applicationContentDataPath;
        private readonly string _rootPath;
        private readonly string _rootUrl;

        public EnvironmentPathProvider(IRootPathProvider rootPathProvider)
        {
            _rootPath = rootPathProvider.GetRootPath();
            _applicationAppDataPath = WebConfigurationManager.AppSettings["DataPath"];
            _applicationContentDataPath = rootPathProvider.GetRootPath() + @"Content";
        }

        public string GetDictionaryPath()
        {
            return Path.Combine(_applicationAppDataPath, @"106m61f5h69wbi0/ord_bm.txt");
        }

        public string GetDictionaryIndexPath()
        {
            return Path.Combine(_applicationAppDataPath, "ordlister_index");
        }

        public string GetStopwordsPath()
        {
            return Path.Combine(_applicationAppDataPath, @"5v9a428lhvxdhdp/stopwords.txt");
        }

        public string GetImageCachePath()
        {
            return Path.Combine(_applicationContentDataPath, @"CachedImages\");
        }

        public string GetSuggestionListPath()
        {
            return Path.Combine(_applicationAppDataPath, @"eok5vpr6zpt7dnh/ord_forslag.txt");
        }

        public string GetTestDictPath()
        {
            return Path.Combine(_applicationAppDataPath, @"a093xni8ohm1p0e/ord_test.txt");
        }

        public string GetXmlListPath()
        {
            return Path.Combine(_applicationAppDataPath, @"");
        }

        public string GetXmlFilePath()
        {
            return Path.Combine(_applicationAppDataPath, @"");
        }

        public string GetRulesPath()
        {
            return Path.Combine(_applicationAppDataPath, @"qpyppu2dsbj8oag/itemrules.txt");
        }

        public string GetBlogFeedPath()
        {
            return Path.Combine(_applicationAppDataPath, @"jfeg7wiml4bmzqn/feeds.xml");
        }

        public string GetOpeningInfoAsXmlPath()
        {
            return Path.Combine(_applicationAppDataPath, @"h2i780pp85a29wo/opening.xml");
        }

        public string GetContactInfoAsXmlPath()
        {
            return Path.Combine(_applicationAppDataPath, @"eb6xjqr5w0peufn/contact.xml");
        }

        public string GetPlaceHolderImagesPath()
        {
            return Path.Combine(_applicationContentDataPath, @"placeholder_images\");
        }

        public string GetFavoritesPath(string userId)
        {
            var favPath = Path.Combine(_applicationContentDataPath, @"favorites\");
            if (!Directory.Exists(favPath)) Directory.CreateDirectory(favPath);
            return Path.Combine(favPath, userId);
        }

        public string GetEventsPath()
        {
            return Path.Combine(_applicationAppDataPath, @"0fznr2xeg1e3ydb/events.xml");
        }

        public string ResolveUrl(string serverPath)
        {
            var relative = serverPath.ToLowerInvariant().Replace(_rootPath.ToLowerInvariant(), "").Replace('\\', '/');
            var absolute = Path.Combine(_rootUrl, relative);
            return absolute;
        }

        public string ResolveUrl(string baseUrl, string serverPath)
        {
            var relative = serverPath.ToLowerInvariant().Replace(_rootPath.ToLowerInvariant(), "").Replace('\\', '/');
            var absolute = Path.Combine(baseUrl, relative);
            return absolute;
        }

        public string GetWebAppUrl()
        {
            return "http://app.solvberget.no/#/";
        }

        public string GetWebAppDocumentDetailsPath(Document document)
        {
            var docUrl = Path.Combine(GetWebAppUrl(), GetWebType(document.DocType), document.DocumentNumber, HttpUtility.UrlEncode(document.Title)).Replace("\\", "/");
            return docUrl;
        }

        private string GetWebType(string docType)
        {
            switch (docType)
            {
                case "Book":
                    return "bok";
                case "Cd":
                    return "cd";
                case "Film":
                    return "film";
                case "AudioBook":
                    return "lydbok";
                case "SheetMusic":
                    return "noter";
                case "Game":
                    return "spill";
                case "OtherJournal":
                    return "journal";
                default:
                    return "annet";
            }
        }

        public string GetSlideConfigurationPath()
        {
            return Path.Combine(_applicationAppDataPath, @"");
        }

        public string GetInfoscreenImagesPath()
        {
            return Path.Combine(_applicationAppDataPath, @"");
        }
        public string GetInstagramBlacklistPath()
        {
            return Path.Combine(_applicationAppDataPath, @"");
        }
    }
}