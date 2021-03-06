﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Threading.Tasks;
using AngleSharp.Dom;
using AngleSharp.Dom.Html;
using AngleSharp.Parser.Html;
using System.Diagnostics;
using SalieriCore.Dao;
using System.IO;
using System.Net;
using System.Threading;
using NMeCab;
using System.Web.Hosting;

namespace SalieriCore.Loader
{
    public class GoogleLoader : WebLoader
    {
        //static string baseURL = "https://www.google.co.jp/search?num=100&lr=lang_en&q=";
        static string baseURL = "https://www.google.co.jp/search?num=100&lr=lang_ja&q=";
        public string keywords { get; set;}


        public GoogleLoader():base() {
        }

        public string Load()
        {
            Console.WriteLine("Load Async start");
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            var doc = default(IHtmlDocument);
            using (var client = new HttpClient())
            {
                Task<Stream> t  =  client.GetStreamAsync(new Uri(baseURL + HttpUtility.UrlEncode(keywords)));
                t.Wait();
                Stream stream = t.Result;

                Console.WriteLine("Loading!!");
                // AngleSharp.Parser.Html.HtmlParserオブジェクトにHTMLをパースさせる
                var parser = new HtmlParser();
                Task<IHtmlDocument> task = parser.ParseAsync(stream);
                task.Wait();
                doc = task.Result;

                Contents = new List<ContentDao>();

                IHtmlCollection<IElement> resultcollection = doc.GetElementsByClassName("r");
                foreach(IElement resultElement in resultcollection)
                {
                    Debug.WriteLine(resultElement.InnerHtml);
                    ContentDao content = new ContentDao();

                    MatchCollection mc = Regex.Matches(resultElement.InnerHtml, "q=(?<url>.+)&amp;sa");
                    if(mc.Count> 0)
                    {
                        content.URL = mc[0].Groups["url"].Value;   
                    }
                    Contents.Add(content);
                }
                stream.Close();
            }
            LoadPages();
            return "success";
        }


    }
}
