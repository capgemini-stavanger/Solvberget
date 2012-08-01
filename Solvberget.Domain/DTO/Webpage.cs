﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.XPath;
using HtmlAgilityPack;

namespace Solvberget.Domain.DTO
{
    public class WebPage
    {
        WebClient client = new WebClient();

        public string Link { get; set; }
        public string Html { get; set; }

        public string GetHtml()
        {
            client.Encoding = Encoding.UTF8;
            string html = client.DownloadString(Link);
            //html = html.Replace("\n", "");
            html = html.Replace("\\", "");
            return html;
        }

        public static string StripHtmlTags(string strHtml)
        {
            return Regex.Replace(strHtml, "<(.|\n)*?>", "");
        }

        public static HtmlNode GetDiv(string strHtml, string divName)
        {

            var expression = "//div[@class='"+divName+"']";
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(strHtml);
            if (htmlDoc.DocumentNode != null)
            {
                HtmlNode node = htmlDoc.DocumentNode.SelectSingleNode(expression);

                if (node != null)
                {
                    // Do something with bodyNode
                    return node;
                }
            }
            return null;
        }

        public static string CleanHtml(string strHtml)
        {

            //Fix \n
            var cleanedHtml = strHtml.Replace("\n", "");
            cleanedHtml = cleanedHtml.Replace("\t", " ");

            
            
             cleanedHtml = cleanedHtml.Replace("<li>", "\n");
             //cleanedHtml = cleanedHtml.Replace("</tr>", "\n");
             cleanedHtml = cleanedHtml.Replace("&nbsp;", " ");

            
            //Remove html tags
            cleanedHtml = StripHtmlTags(cleanedHtml);


            //Remove information
            cleanedHtml = cleanedHtml.Replace("Hvor er vi? Se kart på Google maps", "");
            cleanedHtml = cleanedHtml.Replace("» ", "\n");
            cleanedHtml = cleanedHtml.Replace("►", "");
            
            cleanedHtml = cleanedHtml.Replace(",", "");
           

            //Add \n on uppercase letters..quickfix
            cleanedHtml = cleanedHtml.Replace("STAVANGER", "Stavanger");
            cleanedHtml = cleanedHtml.Replace("AMFI", "Amfi");
            cleanedHtml = cleanedHtml.Replace("SF Kino", "sfkino");
            //cleanedHtml= Regex.Replace(cleanedHtml, @"(?<!_)([A-Z])", "\n$1");
            cleanedHtml = cleanedHtml.Replace("sfkino", "SF Kino");



            //Remove multipe whitespaces
            RegexOptions options = RegexOptions.None;
            Regex regex = new Regex(@"[ ]{2,}", options);
            cleanedHtml = regex.Replace(cleanedHtml, @" ");
          

            return cleanedHtml;
        }

        public static List<String> GetValue(HtmlNode node, string div)
        {
            var nodes = node.Descendants().Where(n => n.Name.StartsWith(div));
           var list = nodes.Select(nodex => nodex.InnerHtml).Select(CleanHtml).ToList();
            return list;
        }

    }
}
