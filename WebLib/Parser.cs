using System;
using System.IO;
using System.Collections.Generic;
using HtmlAgilityPack;
using System.Linq;
using WebLib.CustomExceptions;

namespace WebLib
{
    internal class Parser
    {
        private string htmlContent;
        private HtmlDocument htmlSnippet;
        public Parser(string htmlContent)
        {
            this.htmlContent = htmlContent;
            htmlSnippet = new HtmlDocument();
            htmlSnippet.LoadHtml(htmlContent);
        }

        public IEnumerable<string> GetAllLinksFromHTML(string baseUrl, Dictionary<string, bool> linkSearchRulesRef)
        {
            if (linkSearchRulesRef != null)
            {
                foreach (var rule in linkSearchRulesRef)
                {
                    if (rule.Value)
                    {
                        switch (rule.Key)
                        {
                            case "AllSearch":
                                return GetAllLinksFromHTML();
                            case "DomainSearch":
                                return GetAllLinksFromHTMLbyDomain(GetBaseUrl(baseUrl));
                            case "ParentSearch":
                                return GetAllLinksFromHTMLbyParent(baseUrl);
                            default: throw new UrnownRule("Uknown Rule for links");
                        }
                    }
                }
            }
            return GetAllLinksFromHTML();
        }
        private IEnumerable<string> GetAllLinksFromHTML()
        {
            return GetAllLinks().Distinct();
        }
        private IEnumerable<string> GetAllLinksFromHTMLbyDomain(string baseUrl)
        {
            return GetAllLinks().Where(l => l.Contains(baseUrl)).Distinct();
        }
        private IEnumerable<string> GetAllLinksFromHTMLbyParent(string ParentUrl)
        {
            return GetAllLinks().Where(l => l.Contains(ParentUrl)).Distinct();
        }
        public IEnumerable<string> GetAllLinks()
        {
            return htmlSnippet.DocumentNode.SelectNodes("//a[@href]")
                .SelectMany(a => a.Attributes
                .Where(h => h.Name == "href" && (h.Value.StartsWith("http")
                || h.Value.StartsWith("https")))).Select(l => l.Value);
        }
        public IEnumerable<string> GetAllImgFromHTML(string baseURL)
        {
            return htmlSnippet.DocumentNode.SelectNodes("//img")
                  .Select(g => g.GetAttributeValue("src", "")).Distinct();
            
        }
        public Dictionary<string, string> GetAllImgFromHTMLWithoutFilter(string baseURL)
        {
            var res = GetAllImgFromHTML(baseURL);
            return LinkHelper(res, baseURL);
        }
            public Dictionary<string, string> GetAllImgFromHTMLWithFilter(string baseURL, List<string> contentFiltering)
        {
            var res = GetAllImgFromHTML(baseURL);
            List<string> tempList = new List<string>();
            foreach (var f in contentFiltering)
            {
                foreach (var r in res)
                {
                    if (string.Equals(GetImgExtension(r), f, StringComparison.OrdinalIgnoreCase))
                    {
                        tempList.Add(r);
                    }
                }
            }
            var filteredList=res.Except(tempList);
            return LinkHelper(filteredList, baseURL);
        }
        public Dictionary<string, string> GetAllJSFromHTML(string baseURL)
        {
            var res = htmlSnippet.DocumentNode.SelectNodes("//script")
                 .Where(a => a.GetAttributeValue("type", "") == "text/javascript")
                 .Select(g => g.GetAttributeValue("src", "")).Distinct();
            return LinkHelper(res, baseURL);
        }
        public Dictionary<string, string> GetAllCssFromHTML(string baseURL)
        {
            var res = htmlSnippet.DocumentNode.SelectNodes("//link")
                .Where(a => a.GetAttributeValue("rel", "") == "stylesheet")
                .Select(g => g.GetAttributeValue("href", "")).Distinct();
            return LinkHelper(res, baseURL);
        }
        private Dictionary<string, string> LinkHelper(IEnumerable<string> sequence, string baseURL)
        {
            Dictionary<string, string> keyValueTable = new Dictionary<string, string>();
            foreach (var el in sequence)
            {
                if (el.Length > 0)
                {
                    if (el.StartsWith("http") || el.StartsWith("https"))
                    {
                        keyValueTable.Add(el, el);
                    }
                    else
                    {
                        keyValueTable.Add(el, GetBaseUrl(baseURL) + el);
                    }
                }
            }
            return keyValueTable;
        }
        private string GetBaseUrl(string url)
        {
            var splitedURL = url.Split('.');
            return splitedURL[0] + "." + splitedURL[1] + "." + (splitedURL[2].IndexOf('/') != -1 ? splitedURL[2].Substring(0, splitedURL[2].IndexOf('/')) : splitedURL[2]);
        }
        private string GetImgExtension(string name)
        {
            if (name.LastIndexOf("?") != -1)
            {
                var res = name.Substring(name.LastIndexOf('/'), name.LastIndexOf("?") - name.LastIndexOf("/") - 1);
                string fileName = name.Substring(name.LastIndexOf('/') + 1, name.LastIndexOf("?") - name.LastIndexOf("/") - 1);
                if (fileName.LastIndexOf('.') != -1)
                    return fileName.Substring(fileName.LastIndexOf('.')+1);
                return fileName;
            }
            else
            {
                var r1 = name.Substring(name.LastIndexOf('/') + 1);
                string fileName = name.Substring(name.LastIndexOf('/') + 1);
                if (fileName.LastIndexOf('.') != -1)
                    return fileName.Substring(fileName.LastIndexOf('.')+1);
                return fileName;
            }
        }
    }
}
