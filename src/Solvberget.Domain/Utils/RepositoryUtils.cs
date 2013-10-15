﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml.Linq;

namespace Solvberget.Domain.Utils
{
    public static class RepositoryUtils
    {
        
        public static XDocument GetXmlFromStream(string url)
        {
            var request = WebRequest.Create(url);
            WebResponse response;
            try
            {
                response = request.GetResponse();
            }
            catch (WebException)
            {
                return null;
            }
            var xml = string.Empty;
            using (var stream = response.GetResponseStream())
            {
                var readStream = new StreamReader(stream, Encoding.UTF8);
                xml = readStream.ReadToEnd();
            }

            //The Star-Wars-Beatles-PeerGynt-bug
            //Aleph XML-repsonses somtimes returns Unicode SOH instead of SP 
            //a few places in the document.
            //This causes the parsing to fail..
            const char soh ='\u0001';
            const char sp = '\u0020';
            var xmlEscaped = xml.Replace(soh, sp);

            return XDocument.Parse(xmlEscaped);

        }

        public static XDocument GetXmlFromStreamWithParam(string uri, string param)
        {
            var url = uri + param;
            url = url.Replace(" ", "+");

            return GetXmlFromStream(url);
        }

        public static string GetJsonFromStream(string url)
        {
            var request = WebRequest.Create(url);
            var response = request.GetResponse();
            string json = string.Empty;
            using (var stream = response.GetResponseStream())
            {
                var readStream = new StreamReader(stream, Encoding.UTF8);
                json = readStream.ReadToEnd();
            }

            return json;
        }

        public static void DownloadImageFromUrl(string imageUrl, string imageName, string pathToCache)
        {
            System.Drawing.Image image = null;

            if (!Directory.Exists(pathToCache))
                Directory.CreateDirectory(pathToCache);

            var fileName = Path.Combine(pathToCache, imageName);
            if (File.Exists(fileName))
                return;


            try
            {
                var webRequest = (System.Net.HttpWebRequest)WebRequest.Create(imageUrl);
                webRequest.AllowWriteStreamBuffering = true;
                webRequest.Timeout = 30000;

                var webResponse = webRequest.GetResponse();

                var stream = webResponse.GetResponseStream();

                image = System.Drawing.Image.FromStream(stream);

                webResponse.Close();

                image.Save(fileName);
            }
            catch (Exception)
            {
                //Todo add logging here
            }

        }

        public static string GetJsonFromStreamWithParam(string uri, string param)
        {
            var url = uri + param;
            url = url.Replace(" ", "+");
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = WebRequestMethods.Http.Get;
            request.Accept = "application/json";
            try
            {
                var response = request.GetResponse();

                string json = string.Empty;
                using (var stream = response.GetResponseStream())
                {

                    var readStream = new StreamReader(stream, Encoding.UTF8);
                    json = readStream.ReadToEnd();

                }
                return json;

            } catch(Exception)
            {
                //Todo add logging here
                return string.Empty;
            }
        }

    }
}

