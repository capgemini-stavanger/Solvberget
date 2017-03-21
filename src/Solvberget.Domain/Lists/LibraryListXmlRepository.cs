using Solvberget.Domain.Aleph;
using Solvberget.Domain.Documents.Images;
using Solvberget.Domain.Utils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Solvberget.Domain.Lists
{
    public class LibraryListXmlRepository : IListRepositoryStatic
    {
        private const string StdFolderPath = @"App_Data\librarylists\static\";
        private readonly string _folderPath;
        private string examplelist1 = "5ayzwk2qmjv8d9b/example_list1.xml";
        private string examplelist2 = "30d9xnxs18s8kfb/example_list2.xml";
        private string examplelist3 = "arcoxmgw4aoysee/example_list3.xml";
        private string examplelist4 = "cwl5qrk3jyudwqc/example_list4.xml";
        private string examplelist5 = "q6u7bh35ewtl3gr/example_list5.xml";
        private string examplelist6 = "h4j7qn8q5cs572o/example_list6.xml";

        public LibraryListXmlRepository(IRepository repository, IImageRepository imageRepository, IEnvironmentPathProvider environment)
        {
            var folderPath = environment.GetXmlListPath();
            _folderPath = string.IsNullOrEmpty(folderPath) ? StdFolderPath : folderPath;
        }

        public List<LibraryList> GetLists(int? limit = null)
        {
            var lists = new ConcurrentBag<LibraryList>
            {
                GetLibraryListFromXmlFile(_folderPath + examplelist1),
                GetLibraryListFromXmlFile(_folderPath + examplelist2),
                GetLibraryListFromXmlFile(_folderPath + examplelist3),
                GetLibraryListFromXmlFile(_folderPath + examplelist4),
                GetLibraryListFromXmlFile(_folderPath + examplelist5),
                GetLibraryListFromXmlFile(_folderPath + examplelist6)
            };

            return limit != null
                ? lists.OrderBy(list => list.Priority).Take((int)limit).ToList()
                : lists.OrderBy(list => list.Priority).ToList();
        }

        public DateTime? GetTimestampForLatestChange()
        {
            var newestFile = GetNewestFile(new DirectoryInfo(_folderPath));
            return newestFile?.LastWriteTimeUtc;
        }

        private static FileInfo GetNewestFile(DirectoryInfo directory)
        {
            return directory.GetFiles()
                .Union(directory.GetDirectories().Select(GetNewestFile))
                .OrderByDescending(f => f?.LastWriteTime ?? DateTime.MinValue)
                .FirstOrDefault();
        }

        public static LibraryList GetLibraryListFromXmlFile(string xmlFilePath)
        {
            var xmlDoc = XElement.Load(xmlFilePath);
            var libraryList = FillProperties(xmlDoc);
            libraryList.Id = "static_" + Path.GetFileNameWithoutExtension(xmlFilePath);
            return libraryList;
        }

        public static LibraryList GetLibraryListFromXml(XElement xml)
        {
            return FillProperties(xml);
        }

        private static LibraryList FillProperties(XElement xml)
        {
            var libList = new LibraryList();
            if (xml.Attribute("name") == null) return null;

            var name = xml.Attribute("name");
            if (name != null) libList.Name = name.Value;

            var pri = xml.Attribute("pri");
            if (pri != null)
            {
                var priAsString = pri.Value;
                int priAsInt;
                if (int.TryParse(priAsString, out priAsInt))
                {
                    //Set a lowest priority if priority range is invalid (below 0 or above 10) 
                    libList.Priority = priAsInt < 1 || priAsInt > 10 ? 10 : priAsInt;
                }
                else
                {
                    //Set a lowest priority if null or wrong type
                    libList.Priority = 10;
                }
            }

            var isRanked = xml.Attribute("ranked");
            if (isRanked != null)
            {
                libList.IsRanked = isRanked.Value.Equals("true") ? true : false;
            }

            xml.Elements().Where(e => e.Name == "docnumber").ToList().ForEach(element => libList.DocumentNumbers.Add(element.Value, false));
            return libList;
        }
    }
}
