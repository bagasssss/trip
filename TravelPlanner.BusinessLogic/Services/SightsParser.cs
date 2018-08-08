using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using TravelPlanner.DomainModel;

namespace TravelPlanner.BusinessLogic.Services
{
    public static class SightsParser
    {
        private static string SiteUrl = "https://funtime.kiev.ua";
        private static string PagePath = "/where-to-go?page={0}";

        public static IEnumerable<SightObject> Parse(int totalPages = 2)
        {
            var linkClass = "grid-inner-wrapper";
            var tableClass = "ui very basic unstackable table";
            var titleClass = "location-title";
            var descriptionClass = "main-text";

            var sights = new List<SightObject>();

            for (var i = 1; i <= totalPages; i++)
            {
                var web = new HtmlWeb();
                var doc = web.Load(String.Format(SiteUrl + PagePath, i));

                var desc = doc.DocumentNode
                    .Descendants("a").ToList();

                var linkNodes = desc.Where(node => node.Attributes.Any(a => a.Name == "class")
                                                   && node.Attributes["class"].Value == linkClass).ToList();

                var links = linkNodes.Select(node => node.Attributes.FirstOrDefault(a => a.Name == "href").Value).ToList();

                foreach (var link in links)
                {
                    doc = web.Load(SiteUrl + link);

                    var title = doc.DocumentNode.Descendants("h1")
                        .FirstOrDefault(node => node.Attributes["class"].Value == titleClass).InnerText;

                    var descriptions = doc.DocumentNode.Descendants("div")
                        .FirstOrDefault(node => node.Attributes["class"].Value == descriptionClass)
                        .Descendants("p").Select(node => node.InnerText).ToList();

                    var description = String.Join(" ", descriptions);

                    var geoLocation = doc.DocumentNode
                        .Descendants("table")
                        .FirstOrDefault(node => tableClass.Contains(node.Attributes["class"].Value))
                        .Descendants("td").ElementAt(1).InnerText;

                    geoLocation = geoLocation
                        .Replace("с.ш.", "")
                        .Replace("в.д.", "")
                        .Replace("°", ".")
                        .Replace("N", "")
                        .Replace("E", "")
                        .Replace("′", "")
                        .Replace("″", "")
                        .Trim();

                    try
                    {
                        var lat = geoLocation.Split(',')[0];
                        var lng = geoLocation.Split(',')[1];

                        sights.Add(new SightObject()
                        {
                            Label = title,
                            LatLng = new LatLng()
                            {
                                Lat = double.Parse(lat),
                                Lng = double.Parse(lng)
                            }
                        });
                    }
                    catch (Exception e)
                    {
                        var m = e.Message;
                    }
                }
            }

            return sights;
        }
    }
}
